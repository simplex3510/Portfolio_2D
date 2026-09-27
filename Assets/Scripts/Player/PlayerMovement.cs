using UnityEngine;
using UnityEngine.InputSystem;

using Game.Core.Events;
using Unity.VisualScripting;
using Unity.Mathematics;

namespace Game.Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference _moveActRef;
        [SerializeField] private float _moveForce = 7f;
        [SerializeField] private float _jumpForce = 14f;

        [Header("Ground Check")]
        [SerializeField] private Transform _groundChecker;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _checkDistance = 0.1f;
        [SerializeField] private float _checkOffset = 0.25f;

        [Header("Event Channels")]
        [SerializeField] private FloatEventChannelSO _horizontalVelocityChannel;
        [SerializeField] private FloatEventChannelSO _verticalVelocityChannel;

        private Rigidbody2D _rigidbody2D;

        private Vector2 _input;
        private Vector2 _velocity;

        private bool _isGrounded;

        #region Unity Methods
        private void Awake()
        {
            if (_moveActRef == null)
            {
                Debug.LogError("Move Action Reference is not assigned in the inspector.", this);
                enabled = false;
                return;
            }

            if (_groundChecker == null)
            {
                Debug.LogError("Ground Checker transform is not assigned. Jumping will not work correctly.", this);
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

        private void OnEnable()
        {
            _moveActRef.action.Enable();
        }

        private void OnDisable()
        {
            _moveActRef.action.Disable();
        }

        private void FixedUpdate()
        {
            CheckGrounded();

            Move();
            Jump();
        }

        private void Update()
        {
            UpdateInput();
        }
        #endregion

        #region Update Relative Methods
        private void UpdateInput()
        {
            _input = _moveActRef.action.ReadValue<Vector2>();
        }
        #endregion

        #region FixedUpdate Relative Methods
        private void Move()
        {
            _rigidbody2D.linearVelocity = new Vector2(_input.x * _moveForce, _rigidbody2D.linearVelocityY);

            float VelocityX = _rigidbody2D.linearVelocityX;
            _horizontalVelocityChannel?.Raise(VelocityX);
        }

        private void Jump()
        {
            if (_isGrounded && 0 < _input.y)
            {
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