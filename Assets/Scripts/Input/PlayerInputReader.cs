using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Game.Input
{
    /// <summary>판정 대상 마우스 버튼</summary>
    public enum MouseInputButton
    {
        None,
        LeftButton,
        RightButton
    }

    /// <summary>입력 제스처 종류</summary>
    public enum MouseInputGesture
    {
        None,
        Tap,
        Hold
    }

    /// <summary>제스처를 확정하는 시점</summary>
    public enum MouseInputPhase
    {
        None,
        Pressed,    // 누르는 순간(Tap) / HoldTime 도달 순간(Hold)에 확정
        Released    // 뗄 때 확정 (Hold는 HoldTime 이상 눌렀을 때만)
    }

    public enum MouseInputModifier
    {
        None,
        Shift
    }

    public class PlayerInputReader : MonoBehaviour
    {
        [Header("Move Input Settings")]
        [SerializeField] private InputActionReference _moveActRef;

        [Header("Attack Input Settings")]
        [SerializeField] private InputActionReference _leftClickAttackActRef;
        [SerializeField] private InputActionReference _rightClickAttackActRef;
        [SerializeField] private InputActionReference _attackModifierActRef;

        public Vector2 MovementInput { get; private set; } = Vector2.zero;

        // Shift(조합키)가 현재 눌려 있는지 여부
        // 판정 계층(PlayerInputJudge)이 버튼 press 시점에 읽어 Shift 유무를 구분하는 용도
        public bool IsShiftHeld => _attackModifierActRef.action.IsPressed();

        // 마우스 버튼 입력 이벤트: 어떤 조작법인지는 모르고, 물리적 입력 사실만 전달한다
        public event Action<MouseInputButton> MouseButtonPressed;
        public event Action<MouseInputButton> MouseButtonReleased;

        private void Awake()
        {
            if (_moveActRef == null
                || _leftClickAttackActRef == null
                || _rightClickAttackActRef == null
                || _attackModifierActRef == null)
            {
                Debug.LogError("Input Action Reference is not assigned in the inspector.", this);
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            _moveActRef.action.Enable();

            _leftClickAttackActRef.action.Enable();
            _rightClickAttackActRef.action.Enable();
            _attackModifierActRef.action.Enable();

            // Button 액션은 performed = 눌림, canceled = 뗌
            _leftClickAttackActRef.action.performed += HandleMouseLeftButtonPressed;
            _leftClickAttackActRef.action.canceled += HandleMouseLeftButtonReleased;

            _rightClickAttackActRef.action.performed += HandleMouseRightButtonPressed;
            _rightClickAttackActRef.action.canceled += HandleMouseRightButtonReleased;
        }

        private void OnDisable()
        {
            // 구독 해제 누락 시 비활성화된 객체로 이벤트가 전달되므로 반드시 해제
            _leftClickAttackActRef.action.performed -= HandleMouseLeftButtonPressed;
            _leftClickAttackActRef.action.canceled -= HandleMouseLeftButtonReleased;


            _rightClickAttackActRef.action.performed -= HandleMouseRightButtonPressed;
            _rightClickAttackActRef.action.canceled -= HandleMouseRightButtonReleased;

            _moveActRef.action.Disable();

            _leftClickAttackActRef.action.Disable();
            _rightClickAttackActRef.action.Disable();
            _attackModifierActRef.action.Disable();
        }

        private void Update()
        {
            UpdateMovementInput();
        }

        #region Update Relative Methods
        private void UpdateMovementInput()
        {
            MovementInput = _moveActRef.action.ReadValue<Vector2>();
        }
        #endregion

        #region Attack Input Callbacks
        private void HandleMouseLeftButtonPressed(InputAction.CallbackContext context)
        {
            RaiseButtonPressed(MouseInputButton.LeftButton);
        }

        private void HandleMouseLeftButtonReleased(InputAction.CallbackContext context)
        {
            RaiseButtonReleased(MouseInputButton.LeftButton);
        }

        private void HandleMouseRightButtonPressed(InputAction.CallbackContext context)
        {
            RaiseButtonPressed(MouseInputButton.RightButton);
        }

        private void HandleMouseRightButtonReleased(InputAction.CallbackContext context)
        {
            RaiseButtonReleased(MouseInputButton.RightButton);
        }

        private void RaiseButtonPressed(MouseInputButton button)
        {
            MouseButtonPressed?.Invoke(button);
        }

        private void RaiseButtonReleased(MouseInputButton button)
        {
            MouseButtonReleased?.Invoke(button);
        }
        #endregion
    }

}