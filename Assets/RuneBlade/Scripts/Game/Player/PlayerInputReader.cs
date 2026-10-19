using UnityEngine;
using UnityEngine.InputSystem;

using RuneBlade.Simulation;
using RuneBlade.Client;

namespace RuneBlade.Game
{
    public class PlayerInputReader : MonoBehaviour
    {
        // _actions의 순서와 같다.
        private static readonly LogicalKey[] ActionKeys = { LogicalKey.Left, LogicalKey.Right, LogicalKey.Up, LogicalKey.Down };

        [Header("Input Actions")]
        [SerializeField] private InputActionReference _leftActionReference;
        [SerializeField] private InputActionReference _rightActionReference;
        [SerializeField] private InputActionReference _upActionReference;
        [SerializeField] private InputActionReference _downActionReference;

        private InputAction[] _actions;

        private readonly LocalInputCollector _inputCollector = new();

        private void OnEnable()
        {
            if (_leftActionReference == null || _rightActionReference == null
                || _upActionReference == null || _downActionReference == null)
            {
                Debug.LogError("InputActionReference가 모두 연결되지 않았습니다.", this);
                enabled = false;
                return;
            }

            _actions = new[]
            {
                _leftActionReference.action,
                _rightActionReference.action,
                _upActionReference.action,
                _downActionReference.action,
            };

            // 액션은 에셋에 속한 공유 객체이므로 이 컴포넌트가 연결한 콜백은 OnDisable에서 반드시 해제한다.
            foreach (InputAction action in _actions)
            {
                action.performed += OnActionPerformed;
                action.canceled += OnActionCanceled;
                action.Enable();
            }
        }

        private void OnDisable()
        {
            if (_actions == null)
            {
                return;
            }

            foreach (InputAction action in _actions)
            {
                action.performed -= OnActionPerformed;
                action.canceled -= OnActionCanceled;
                action.Disable();
            }

            _actions = null;
        }

        public TickInput GetTickInput()
        {
            return _inputCollector.GetTickInput();
        }

        // 콜백은 입력 시스템이 발생 순서대로 호출하므로 같은 프레임 안의 순서가 보존된다.
        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            _inputCollector.AddKeyInput(new KeyInputEvent(ToLogicalKey(context.action), KeyTransition.Press));
        }

        private void OnActionCanceled(InputAction.CallbackContext context)
        {
            _inputCollector.AddKeyInput(new KeyInputEvent(ToLogicalKey(context.action), KeyTransition.Release));
        }

        private LogicalKey ToLogicalKey(InputAction action)
        {
            return ActionKeys[System.Array.IndexOf(_actions, action)];
        }
    }
}
