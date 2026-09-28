using UnityEngine;

using Game.Core.Events;

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
        [SerializeField] private PlayerAnimStateEventChannelSO _animStateChannel;

        [Header("Receive Event Channels")]
        [SerializeField] private FloatEventChannelSO _horizontalVelocityChannel;
        [SerializeField] private FloatEventChannelSO _verticalVelocityChannel;

        private Animator _animator;
        private PlayerAnimState _animState;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (_horizontalVelocityChannel == null)
            {
                Debug.LogError("Horizontal Velocity Event Channel is not assigned. Horizontal velocity will not be broadcasted.", this);
            }

            if (_verticalVelocityChannel == null)
            {
                Debug.LogError("Vertical Velocity Event Channel is not assigned. Vertical velocity will not be broadcasted.", this);
            }

            if (_flipRoot == null)
            {
                Debug.LogError("Flip Object is not assigned. Character flipping will not work correctly.", this);
            }

            if (_animStateChannel == null)
            {
                Debug.LogError("Player Anim State Event Channel is not assigned. Animation state changes will not be broadcasted.", this);
            }
        }

        private void OnEnable()
        {
            if (_horizontalVelocityChannel != null) _horizontalVelocityChannel.OnRaised += SetHorizontalVelocity;
            if (_verticalVelocityChannel != null) _verticalVelocityChannel.OnRaised += SetVerticalVelocity;
        }

        private void OnDisable()
        {
            if (_horizontalVelocityChannel != null) _horizontalVelocityChannel.OnRaised -= SetHorizontalVelocity;
            if (_verticalVelocityChannel != null) _verticalVelocityChannel.OnRaised -= SetVerticalVelocity;
        }

        public void NotifyStateEntered(PlayerAnimState state)
        {
            if (_animState == state)
            {
                return; // 같은 상태 중복 발행 방지
            }

            _animState = state;

            if (_animStateChannel != null) 
            {
                _animStateChannel.Raise(state);
            }
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
            // _flipOjbect의 null 검사는 Awake()에서 이미 수행되었으므로 여기서는 생략

            if (velocityX < 0)
            {
                _flipRoot.localScale = new Vector3(-1, 1, 1);
            }
            else if (velocityX > 0)
            {
                _flipRoot.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
