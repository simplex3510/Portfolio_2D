using System.Collections.Generic;
using Game.Core.Events;
using Game.Player.Attack;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Sword.Anim
{
    public enum SwordAnimState { None, Idle, Other }

    public class SwordAnimatorController : MonoBehaviour
    {
        [Header("Animator References")]
        [SerializeField] private Animator _swordAnimator;

        [Header("Send & Receive Event Channels")]
        [SerializeField] private PlayerAttackEventChannelSO _attackChannel;

        [Header("Receive Event Channels")]
        [SerializeField] private IntEventChannelSO _facingChannel;

        [Header("Attack Triggers")]
        [SerializeField] private List<string> _attackTriggerList;

        // 1: 오른쪽, -1: 왼쪽
        private int _facing = 1;

        private readonly Dictionary<string, int> _attackHashDict = new();

        // 공격 중에는 중복 트리거를 무시하기 위한 플래그
        private bool _isAttacking;
        private int _currentAttackHash;

        #region Unity Methods
        public void Awake()
        {
            #region Animator References Validation
            if (_swordAnimator == null)
            {
                Debug.LogError("Sword Animator is not assigned in the inspector.", this);
                enabled = false;
            }

            if (_attackChannel == null)
            {
                Debug.LogError("Attack Event Channel is not assigned in the inspector.", this);
                enabled = false;
            }

            if (_facingChannel == null)
            {
                Debug.LogError("Facing Direction Event Channel is not assigned in the inspector.", this);
                enabled = false;
            }

            if (_attackTriggerList == null || _attackTriggerList.Count == 0)
            {
                Debug.LogError("No attack to trigger mappings are assigned in the inspector.", this);
                enabled = false;
            }
            else
            {
                foreach (var s in _attackTriggerList)
                {
                    _attackHashDict[s] = Animator.StringToHash(s);
                }
            }
            #endregion
        }

        private void OnEnable()
        {
            _attackChannel.Started += HandleAttackStarted;
            _attackChannel.Canceled += HandleAttackCanceled;

            _facingChannel.OnRaised += SetFacing;
        }

        private void OnDisable()
        {
            _attackChannel.Started -= HandleAttackStarted;
            _attackChannel.Canceled -= HandleAttackCanceled;

            _facingChannel.OnRaised -= SetFacing;
        }
        #endregion

        private int SetAttackTrigger(string attackTrigger)
        {
            if (!_attackHashDict.TryGetValue(attackTrigger, out int hash))
            {
                Debug.LogWarning($"Attack trigger '{attackTrigger}' is not found in the mapping list.", this);
                hash = Animator.StringToHash(attackTrigger);
                _attackHashDict[attackTrigger] = hash;
            }

            return hash;
        }

        #region Attack Event Handlers
        private void HandleAttackStarted(AttackDataSO data)
        {
            // 재생 중에 들어온 입력은 무시한다.
            // 소비되지 못한 공격 트리거가 남아 있다가, 종료 직후 공격이 한 번 더 재생되는 것을 막는다
            if (_isAttacking)
            {
                return;
            }

            _isAttacking = true;
            _currentAttackHash = SetAttackTrigger(data.AnimationTrigger);

            _swordAnimator.ResetTrigger(_currentAttackHash);
            _swordAnimator.SetTrigger(_currentAttackHash);
        }

        private void HandleAttackCanceled(AttackDataSO data)
        {
            // 현재는 slash만 존재하므로 버튼 release(탭)는 무시하고 애니메이션을 끝까지 재생한다.
            // hold형 공격(smash/thrust/spin)의 release 처리와 실제 캔슬은 해당 시점에 결정한다
        }

        private void HandleAttackEnded()
        {
            if (!_isAttacking)
            {
                return;
            }

            // 종료 전환은 Animator의 Exit Time 전이가 담당하므로 여기서 트리거를 세팅하지 않는다.
            // (소비할 전이가 없는 트리거가 남아 다음 공격이 즉시 종료되는 것을 방지)
            _isAttacking = false;
            _attackChannel.RaiseEnded();
        }
        #endregion

        #region Sword Anim State Notifications
        public void NotifyStateEntered(SwordAnimState state)
        {
            
        }

        public void NotifyStateExited(SwordAnimState state)
        {
            Debug.Log($"SwordAnimState exited: {state}", this);

            if (state != SwordAnimState.Other)
            {
                return;
            }
            
            HandleAttackEnded();
        }
        #endregion

        public void SetFacing(int facing)
        {
            if (facing != 1 && facing != -1)
            {
                return;
            }

            transform.localScale = new Vector3(facing * Mathf.Abs(transform.localScale.x) * facing, transform.localScale.y, transform.localScale.z);
        }
    }
}