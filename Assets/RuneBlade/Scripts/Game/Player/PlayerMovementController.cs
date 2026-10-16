using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using RuneBlade.Simulation;

namespace RuneBlade.Game
{
    /// <summary>
    /// 플레이 모드 테스트용. 화살표 입력을 모아 60Hz로 PlayerMovement를 진행하고 위치를 transform에 반영한다.
    /// TODO: 네트워크 단계에서 입력 수집기와 View로 나눈다.
    /// </summary>
    public class PlayerMovementController : MonoBehaviour
    {
        // 렌더 프레임이 느려도 시뮬레이션은 이 간격으로 따라잡되, 한 프레임에 진행하는 틱 수는 제한한다.
        private const float TickDurationSeconds = 1f / 60f;
        private const int MaxTicksPerFrame = 5;

        [Header("Movement Settings")]
        [SerializeField] private PlayerMovementSettingSO _movementSettingSO;

        // _actions의 순서와 같다.
        private static readonly LogicalKey[] ActionKeys = { LogicalKey.Left, LogicalKey.Right, LogicalKey.Up, LogicalKey.Down };

        [Header("Input Actions")]
        [SerializeField] private InputActionReference _leftActionReference;
        [SerializeField] private InputActionReference _rightActionReference;
        [SerializeField] private InputActionReference _upActionReference;
        [SerializeField] private InputActionReference _downActionReference;

        private readonly Queue<KeyInputEvent> _pendingInputEvents = new();
        private InputAction[] _actions;

        private PlayerMovement _movement;
        private MovementSettings _settings;
        private float _tickAccumulator;

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

        private void Start()
        {
            if (_movementSettingSO == null)
            {
                Debug.LogError("PlayerMovementSettingSO가 연결되지 않았습니다.", this);
                enabled = false;
                return;
            }

            // 매치 중 값이 바뀌면 예측과 서버 결과가 어긋나므로 시작 시 복사해서 고정한다.
            _settings = _movementSettingSO.Settings;
            _movement = PlayerMovement.CreateOnGround(0f);
            ApplyToTransform();
        }

        private void Update()
        {
            _tickAccumulator += Time.deltaTime;

            int ticksThisFrame = 0;
            while (_tickAccumulator >= TickDurationSeconds && ticksThisFrame < MaxTicksPerFrame)
            {
                _tickAccumulator -= TickDurationSeconds;
                ticksThisFrame++;
                RunTick();
            }

            // 상한에 걸려 못 따라잡은 시간은 버린다(일시 정지·긴 프레임 뒤의 폭주 방지).
            if (ticksThisFrame == MaxTicksPerFrame)
            {
                _tickAccumulator = 0f;
            }
        }

        private void RunTick()
        {
            // 한 틱에 담는 이벤트는 TickInput.Capacity까지이며, 넘친 이벤트는 큐에 남아 다음 틱으로 이월된다.
            TickInput input = default;
            while (_pendingInputEvents.Count > 0 && !input.IsFull)
            {
                input.TryAdd(_pendingInputEvents.Dequeue());
            }

            _movement.Tick(input, _settings);
            ApplyToTransform();
        }

        private void ApplyToTransform()
        {
            transform.position = new Vector3(_movement.PositionX, _movement.PositionY, transform.position.z);
        }

        // 콜백은 입력 시스템이 발생 순서대로 호출하므로 같은 프레임 안의 순서가 보존된다.
        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            _pendingInputEvents.Enqueue(new KeyInputEvent(ToLogicalKey(context.action), KeyTransition.Press));
        }

        private void OnActionCanceled(InputAction.CallbackContext context)
        {
            _pendingInputEvents.Enqueue(new KeyInputEvent(ToLogicalKey(context.action), KeyTransition.Release));
        }

        private LogicalKey ToLogicalKey(InputAction action)
        {
            return ActionKeys[System.Array.IndexOf(_actions, action)];
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10f, 10f, 800f, 24f), _movement.ToString());
        }
    }
}
