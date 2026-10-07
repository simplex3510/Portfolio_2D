using UnityEngine;
using UnityEngine.InputSystem;

using Game.Core.Events;
using Game.Input;

namespace Game.Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Event Channels")]
        [SerializeField] private FloatEventChannelSO _horizontalVelocityChannel;
        [SerializeField] private FloatEventChannelSO _verticalVelocityChannel;

        [Header("Input Reference")]
        [SerializeField] private PlayerInputReader _playerInputReader;

        [Header("Movement Settings")]
        [SerializeField] private float _moveForce = 7f;
        [SerializeField] private float _jumpForce = 14f;

        [Header("Ground Check")]
        [SerializeField] private Transform _groundChecker;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _checkDistance = 0.1f;
        [SerializeField] private float _checkOffset = 0.25f;

        private Rigidbody2D _rigidbody2D;

        private bool _isGrounded;

        #region Unity Methods
        private void Awake()
        {
            if (_groundChecker == null)
            {
                Debug.LogError("Ground Checker transform is not assigned. Jumping will not work correctly.", this);
                enabled = false;
                return;
            }

            if (_playerInputReader == null)
            {
                Debug.LogError("Player Input reference is not assigned. Movement will not work correctly.", this);
                enabled = false;
                return;
            }

            if (_horizontalVelocityChannel == null)
            {
                Debug.LogError("Horizontal Velocity Event Channel is not assigned. Horizontal velocity will not be broadcasted.", this);
            }

            if (_verticalVelocityChannel == null)
            {
                Debug.LogError("Vertical Velocity Event Channel is not assigned. Vertical velocity will not be broadcasted.", this);
            }

            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            CheckGrounded();

            Move();
            Jump();
        }
        #endregion

        #region FixedUpdate Relative Methods
        private void Move()
        {
            _rigidbody2D.linearVelocity = new Vector2(_playerInputReader.MovementInput.x * _moveForce, _rigidbody2D.linearVelocityY);

            float VelocityX = _rigidbody2D.linearVelocityX;
            _horizontalVelocityChannel?.Raise(VelocityX);
        }

        private void Jump()
        {
            if (_isGrounded && 0 < _playerInputReader.MovementInput.y)
            {
                // 경사로 에서 점프 시 기존 속도를 초기화하여 점프 높이를 일정하게 유지
                _rigidbody2D.linearVelocityY = 0f; 
                _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }

            float VelocityY = _isGrounded ? 0f : _rigidbody2D.linearVelocityY;
            _verticalVelocityChannel?.Raise(VelocityY);
        }

        private void CheckGrounded()
        {
            // _groundChecker의 null 체크는 Awake에서 이미 수행되었으므로 여기서는 생략

            bool leftCheck = Physics2D.Raycast(_groundChecker.position + Vector3.left * _checkOffset, Vector2.down, _checkDistance, _groundLayer);
            bool middleCheck = Physics2D.Raycast(_groundChecker.position, Vector2.down, _checkDistance, _groundLayer);
            bool rightCheck = Physics2D.Raycast(_groundChecker.position + Vector3.right * _checkOffset, Vector2.down, _checkDistance, _groundLayer);

            _isGrounded = leftCheck || middleCheck || rightCheck;
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_groundChecker == null) return;

            Gizmos.color = Color.red;

            Gizmos.DrawRay(_groundChecker.position + Vector3.left * _checkOffset, Vector2.down * _checkDistance);
            Gizmos.DrawRay(_groundChecker.position, Vector2.down * _checkDistance);
            Gizmos.DrawRay(_groundChecker.position + Vector3.right * _checkOffset, Vector2.down * _checkDistance);
        }
#endif
    }
}