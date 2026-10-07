using UnityEngine;

using Game.Core.Events;
using Unity.Mathematics;

namespace Game.Player.Anim
{
    public enum PlayerAnimState { Idle, Move, Jump, Fall }

    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        // 애니메이션 파라미터 해시값 - 성능 최적화를 위해 사용
        private static readonly int HorizontalVelocityHash = Animator.StringToHash("HorizontalVelocity");
        private static readonly int VerticalVelocityHash = Animator.StringToHash("VerticalVelocity");

        [Header("Flip Object")]
        [SerializeField] private Transform _flipRoot;

        [Header("Send Event Channels")]
        [SerializeField] private IntEventChannelSO _facingChannel;

        [Header("Receive Event Channels")]
        [SerializeField] private FloatEventChannelSO _horizontalVelocityChannel;
        [SerializeField] private FloatEventChannelSO _verticalVelocityChannel;

        [SerializeField] private PlayerAttackEventChannelSO _attackChannel;

        private Animator _animator;
        private PlayerAnimState _animState;

        private int _lastFacing = 1;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (_horizontalVelocityChannel == null)
            {
                Debug.LogError("Horizontal Velocity Event Channel is not assigned. Horizontal velocity will not be broadcasted.", this);
                enabled = false;
            }

            if (_verticalVelocityChannel == null)
            {
                Debug.LogError("Vertical Velocity Event Channel is not assigned. Vertical velocity will not be broadcasted.", this);
                enabled = false;
            }

            if (_flipRoot == null)
            {
                Debug.LogError("Flip Object is not assigned. Character flipping will not work correctly.", this);
                enabled = false;
            }

            if (_attackChannel == null)
            {
                Debug.LogError("Player Attack Event Channel is not assigned. Attack events will not be broadcasted.", this);
                enabled = false;
            }

            if (_facingChannel == null)
            {
                Debug.LogError("Facing Direction Event Channel is not assigned. Facing direction changes will not be broadcasted.", this);
                enabled = false;
            }
        }

        private void OnEnable()
        {
            if (_horizontalVelocityChannel != null) _horizontalVelocityChannel.OnRaised += SetHorizontalVelocity;
            if (_verticalVelocityChannel != null) _verticalVelocityChannel.OnRaised += SetVerticalVelocity;
;
        }

        private void OnDisable()
        {
            if (_horizontalVelocityChannel != null) _horizontalVelocityChannel.OnRaised -= SetHorizontalVelocity;
            if (_verticalVelocityChannel != null) _verticalVelocityChannel.OnRaised -= SetVerticalVelocity;
        }

        private void SetHorizontalVelocity(float velocityX)
        {
            _animator.SetFloat(HorizontalVelocityHash, velocityX);
            FlipCharacter(velocityX);
        }

        private void SetVerticalVelocity(float velocityY)
        {
            _animator.SetFloat(VerticalVelocityHash, velocityY);
        }

        private void FlipCharacter(float velocityX)
        {
            if (_flipRoot == null || _facingChannel == null)
            {
                return;
            }

            int currentFacing = 0;
            if (0.01f < velocityX)
            {
                currentFacing = 1;
            }
            else if (velocityX < -0.01f)
            {
                currentFacing = -1;
            }
            else
            {
                // 현재 이동 속도가 0에 가까우면 캐릭터의 방향을 변경하지 않는다.
                return;
            }

            if (currentFacing == _lastFacing)
            {
                // 현재 방향과 마지막 방향이 동일하면 캐릭터의 방향을 변경하지 않는다.
                return;
            }
            else
            {
                _flipRoot.localScale = new Vector3(currentFacing, 1, 1);
                _lastFacing = currentFacing;
                _facingChannel.Raise((int)_flipRoot.localScale.x);
            }
        }
    }
}
