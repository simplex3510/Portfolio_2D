using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player.Input
{

    public class PlayerInputReader : MonoBehaviour
    {
        [Header("Move Input Settings")]
        [SerializeField] private InputActionReference _moveActRef;

        [Header("Attack Input Settings")]
        [SerializeField] private InputActionReference _LeftClickAttackActRef;
        [SerializeField] private InputActionReference _attackModifierActRef;

        public Vector2 Input { get; private set; } = Vector2.zero;

        // 조합키가 현재 눌려 있는지 여부
        // 판정 계층이 AttackLC press 시점에 읽어 LC / LC+Shift를 구분하는 용도
        public bool IsAttackModifierHeld => _attackModifierActRef.action.IsPressed();

        // 공격 입력 이벤트: 어떤 공격인지는 모르고, 물리적 입력 사실만 전달한다
        public event Action OnLeftClickAttackPressed;
        public event Action OnLeftClickAttackReleased;
        public event Action OnAttackModifierPressed;

        private void Awake()
        {
            if (_moveActRef == null || _LeftClickAttackActRef == null || _attackModifierActRef == null)
            {
                Debug.LogError("Input Action Reference is not assigned in the inspector.", this);
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            _moveActRef.action.Enable();

            _LeftClickAttackActRef.action.Enable();
            _attackModifierActRef.action.Enable();

            // Button 액션은 performed = 눌림, canceled = 뗌
            _LeftClickAttackActRef.action.performed += HandleLeftClickAttackPerformed;
            _LeftClickAttackActRef.action.canceled += HandleLeftClickAttackCanceled;

            _attackModifierActRef.action.performed += HandleAttackModifierPerformed;
        }

        private void OnDisable()
        {
            // 구독 해제 누락 시 비활성화된 객체로 이벤트가 전달되므로 반드시 해제
            _LeftClickAttackActRef.action.performed -= HandleLeftClickAttackPerformed;
            _LeftClickAttackActRef.action.canceled -= HandleLeftClickAttackCanceled;

            _attackModifierActRef.action.performed -= HandleAttackModifierPerformed;

            _moveActRef.action.Disable();

            _LeftClickAttackActRef.action.Disable();
            _attackModifierActRef.action.Disable();
        }

        private void Update()
        {
            // 폴링
            UpdateMoveInput();
        }

        #region Update Relative Methods
        private void UpdateMoveInput()
        {
            Input = _moveActRef.action.ReadValue<Vector2>();
        }
        #endregion

        #region Attack Input Callbacks
        private void HandleLeftClickAttackPerformed(InputAction.CallbackContext context)
        {
            OnLeftClickAttackPressed?.Invoke();
            Debug.Log("Left Click Attack Pressed");
        }

        private void HandleLeftClickAttackCanceled(InputAction.CallbackContext context)
        {
            OnLeftClickAttackReleased?.Invoke();
            Debug.Log("Left Click Attack Released");
        }

        private void HandleAttackModifierPerformed(InputAction.CallbackContext context)
        {
            OnAttackModifierPressed?.Invoke();
            Debug.Log("Attack Modifier Pressed");
        }
        #endregion
    }

}