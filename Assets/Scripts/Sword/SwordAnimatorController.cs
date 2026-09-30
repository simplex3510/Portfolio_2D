using System.Collections.Generic;
using Game.Core.Events;
using Game.Player.Attack;
using Game.Player.Input;
using UnityEngine;

namespace Game.Sword.Anim
{
    public enum SwordAnimState { None, Idle, Other }

    public class SwordAnimatorController : MonoBehaviour
    {
        [Header("Aim References")]
        [SerializeField] private PlayerInputReader _inputReader;
        // 각도 계산의 원점. Damping이 적용된 검 위치가 아니라 플레이어 앵커를 사용한다
        [SerializeField] private Transform _swordAnchor;
        // 비어 있으면 Camera.main 사용
        [SerializeField] private Camera _camera;

        [Header("Aim Settings")]
        // facing 수평축을 0도로 두고 위/아래로 허용하는 최대 각도
        [SerializeField, Range(0f, 180f)] private float _maxAimAngle = 90f;

        [Header("Animator References")]
        [SerializeField] private Animator _swordAnimator;

        [Header("Send & Receive Event Channels")]
        [SerializeField] private PlayerAttackEventChannelSO _attackChannel;

        [Header("Receive Event Channels")]
        [SerializeField] private IntEventChannelSO _facingChannel;

        [Header("Attack Triggers")]
        [SerializeField] private List<string> _attackTriggerList;

        // 마우스가 앵커와 거의 겹치면 방향이 불안정하므로 회전을 갱신하지 않는 최소 거리(제곱)
        private static readonly float MinAimSqrDistance = 0.0001f;
        // 1: 오른쪽, -1: 왼쪽
        private int _facing = 1;

        private readonly Dictionary<string, int> _attackHashDict = new();

        // 공격 중에는 중복 트리거를 무시하기 위한 플래그
        private bool _isAttacking;
        private int _currentAttackHash;

        #region Unity Methods
        public void Awake()
        {
            #region Aim References Validation
            if (_inputReader == null)
            {
                Debug.LogError("Player Input Reader is not assigned in the inspector.", this);
                enabled = false;
            }

            if (_swordAnchor == null)
            {
                Debug.LogError("Sword Anchor is not assigned in the inspector.", this);
                enabled = false;
            }

            if (_camera == null)
            {
                Debug.LogWarning("Camera is not assigned in the inspector.\nUsing Camera.main.", this);
                _camera = Camera.main;
            }
            #endregion

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

        private void LateUpdate()
        {
            UpdateAimRotation();
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
            _isAttacking = true;
            _currentAttackHash = SetAttackTrigger(data.AnimationTrigger);

            _swordAnimator.ResetTrigger(_currentAttackHash);
            _swordAnimator.SetTrigger(_currentAttackHash);
        }

        private void HandleAttackCanceled(AttackDataSO data)
        {
            _isAttacking = false;
            _swordAnimator.ResetTrigger(_currentAttackHash);
        }

        private void HandleAttackEnded()
        {
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
            HandleAttackEnded();
        }
        #endregion

        public void SetFacing(int facing)
        {
            if (facing == 0) return;

            _facing = facing;

            // 왼쪽을 볼 때 회전각이 180도 부근이 되어 스프라이트가 뒤집혀 보이므로 Y축을 반전한다
            Vector3 scale = transform.localScale;
            scale.y = Mathf.Abs(scale.y) * _facing;
            transform.localScale = scale;
        }

        private void UpdateAimRotation()
        {
            Vector2 aimWorldPosition = ScreenToWorld(_inputReader.AimScreenPosition);
            Vector2 aimDirection = aimWorldPosition - (Vector2)_swordAnchor.position;

            // 마우스 방향이 너무 가까우면 회전을 무시한다
            if (aimDirection.sqrMagnitude < MinAimSqrDistance)
            {
                return;
            }

            // facing이 왼쪽이면 x를 뒤집어 "오른쪽 기준"으로 통일한 상대 각도를 구한다
            float relativeAngle = Mathf.Atan2(aimDirection.y, aimDirection.x * _facing) * Mathf.Rad2Deg;
            relativeAngle = Mathf.Clamp(relativeAngle, -_maxAimAngle, _maxAimAngle);

            // 상대 각도를 월드 각도로 환원: 왼쪽 facing은 180도를 기준으로 좌우 대칭
            float worldAngle = _facing > 0 ? relativeAngle : 180f - relativeAngle;
            transform.rotation = Quaternion.Euler(0f, 0f, worldAngle);
        }

        private Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            // z는 카메라로부터의 거리. 원근 카메라여도 z=0 평면에 맞도록 카메라 z를 사용한다
            Vector3 screenPoint = new Vector3(screenPosition.x, screenPosition.y, -_camera.transform.position.z);
            return _camera.ScreenToWorldPoint(screenPoint);
        }
    }
}