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

        [Header("Aim Input Settings")]
        [SerializeField] private InputActionReference _aimActRef;

        public Vector2 Input { get; private set; } = Vector2.zero;

        // 마우스 스크린 좌표(픽셀)
        // 월드 좌표 변환은 카메라가 필요하므로 입력 계층에서는 하지 않고,
        // 소비자(조준 회전 컴포넌트)가 담당한다
        public Vector2 AimScreenPosition { get; private set; } = Vector2.zero;

        // 조합키가 현재 눌려 있는지 여부
        // 판정 계층이 AttackLC press 시점에 읽어 LC / LC+Shift를 구분하는 용도
        public bool IsAttackModifierHeld => _attackModifierActRef.action.IsPressed();

        // 공격 입력 이벤트: 어떤 공격인지는 모르고, 물리적 입력 사실만 전달한다
        public event Action OnLeftClickAttackPressed;
        public event Action OnLeftClickAttackReleased;
        public event Action OnAttackModifierPressed;

        private void Awake()
        {
            if (_moveActRef == null || _LeftClickAttackActRef == null || _attackModifierActRef == null || _aimActRef == null)
            {
                Debug.LogError("Input Action Reference is not assigned in the inspector.", this);
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            _moveActRef.action.Enable();
            _aimActRef.action.Enable();

            _LeftClickAttackActRef.action.Enable();
            _attackModifierActRef.action.Enable();

            // Button 액션은 performed = 눌림, canceled = 뗌
            _LeftClickAttackActRef.action.performed += HandleLeftButtonAttackPressed;
            _LeftClickAttackActRef.action.canceled += HandleLeftButtonAttackReleased;

            _attackModifierActRef.action.performed += HandleAttackModifierPressed;
        }

        private void OnDisable()
        {
            // 구독 해제 누락 시 비활성화된 객체로 이벤트가 전달되므로 반드시 해제
            _LeftClickAttackActRef.action.performed -= HandleLeftButtonAttackPressed;
            _LeftClickAttackActRef.action.canceled -= HandleLeftButtonAttackReleased;

            _attackModifierActRef.action.performed -= HandleAttackModifierPressed;

            _moveActRef.action.Disable();
            _aimActRef.action.Disable();

            _LeftClickAttackActRef.action.Disable();
            _attackModifierActRef.action.Disable();
        }

        private void Update()
        {
            // 폴링
            UpdateMoveInput();
            UpdateAimInput();
        }

        #region Update Relative Methods
        private void UpdateMoveInput()
        {
            Input = _moveActRef.action.ReadValue<Vector2>();
        }

        private void UpdateAimInput()
        {
            AimScreenPosition = _aimActRef.action.ReadValue<Vector2>();
        }
        #endregion

        #region Attack Input Callbacks
        private void HandleLeftButtonAttackPressed(InputAction.CallbackContext context)
        {
            OnLeftClickAttackPressed?.Invoke();
            Debug.Log("Left Click Attack Pressed");
        }

        private void HandleLeftButtonAttackReleased(InputAction.CallbackContext context)
        {
            OnLeftClickAttackReleased?.Invoke();
            Debug.Log("Left Click Attack Released");
        }

        private void HandleAttackModifierPressed(InputAction.CallbackContext context)
        {
            OnAttackModifierPressed?.Invoke();
            Debug.Log("Attack Modifier Pressed");
        }
        #endregion
    }

}