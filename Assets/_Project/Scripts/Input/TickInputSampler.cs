using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using RuneBlade.Simulation;

namespace RuneBlade.Input
{
    // 키보드 이벤트를 받아 틱마다 TickInput 하나를 만든다.
    public class TickInputSampler : IDisposable
    {
        // 물리 키 하나가 어떤 논리 입력에 대응되는지 적어 둔 항목
        private readonly struct KeyBinding
        {
            public readonly Key KeyboardKey;
            public readonly LogicalKeyFlags LogicalKeyFlag;
            public readonly int SlotNumber;                 // 1~7, 슬롯 키가 아니면 NoSlot
            public readonly HorizontalDirection Horizontal; // 좌우 키가 아니면 None

            public KeyBinding(Key keyboardKey, LogicalKeyFlags logicalKeyFlag, int slotNumber, HorizontalDirection horizontal)
            {
                KeyboardKey = keyboardKey;
                LogicalKeyFlag = logicalKeyFlag;
                SlotNumber = slotNumber;
                Horizontal = horizontal;
            }
        }

        // 키의 사건(변화, 전환). 큐에서 틱이 가져가기를 기다린다.
        private readonly struct KeyStateTransition
        {
            public readonly double Timestamp;
            public readonly int BindingIndex;
            public readonly bool IsPressed;

            public KeyStateTransition(double timestamp, int bindingIndex, bool isPressed)
            {
                Timestamp = timestamp;
                BindingIndex = bindingIndex;
                IsPressed = isPressed;
            }
        }

        // 물리 키 → 논리 입력 대응표 (슬롯 Q W E R A S D = 1~7번, 방향은 화살표 키)
        private static readonly KeyBinding[] KeyBindings =
        {
            // 슬롯 키는 슬롯 번호를 기록하고
            CreateSlotBinding(Key.Q, 1),
            CreateSlotBinding(Key.W, 2),
            CreateSlotBinding(Key.E, 3),
            CreateSlotBinding(Key.R, 4),
            CreateSlotBinding(Key.A, 5),
            CreateSlotBinding(Key.S, 6),
            CreateSlotBinding(Key.D, 7),

            // 방향키는 슬롯이 아니므로 NO_SLOT을 기록
            new KeyBinding(Key.LeftArrow,   LogicalKeyFlags.Left,  LogicalKeySlots.NoSlot,   HorizontalDirection.Left),
            new KeyBinding(Key.RightArrow,  LogicalKeyFlags.Right, LogicalKeySlots.NoSlot,   HorizontalDirection.Right),
            new KeyBinding(Key.UpArrow,     LogicalKeyFlags.Up,    LogicalKeySlots.NoSlot,   HorizontalDirection.None),
            new KeyBinding(Key.DownArrow,   LogicalKeyFlags.Down,  LogicalKeySlots.NoSlot,   HorizontalDirection.None),
        };

        // 이벤트 쪽에서 쓰는 값: 각 키가 지금 눌려 있다고 알고 있는지 (키 트랜지션 감지용)
        private readonly bool[] _isKeyboardKeyDown = new bool[KeyBindings.Length];

        // 아직 어느 틱에도 반영되지 않은 사건들의 기록. 도착한 순서 그대로 쌓인다.
        private readonly Queue<KeyStateTransition> _pendingTransitions = new();

#region Tick-scoped state
        private LogicalKeyFlags _currentPressedKeyMask;   // 지금 눌려 있는 키
        private LogicalKeyFlags _latchedKeyMask;          // 이번 틱 구간에서 한 번이라도 눌린 키 (OR 래치)

        private int _lastPressedSlotNumber = LogicalKeySlots.NoSlot;     // 이번 틱 구간에서 마지막으로 눌린 슬롯 (0 = 없음)
        private HorizontalDirection _lastPressedHorizontal = HorizontalDirection.None;
#endregion

        public TickInputSampler()
        {
            InputSystem.onEvent += OnInputEvent;
            Application.focusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            InputSystem.onEvent -= OnInputEvent;
            Application.focusChanged -= OnFocusChanged;
        }

        private static KeyBinding CreateSlotBinding(Key keyboardKey, int slotNumber)
        {
            return new KeyBinding(keyboardKey, LogicalKeySlots.ConvertToFlag(slotNumber), slotNumber, HorizontalDirection.None);
        }

        // 키보드 이벤트가 올 때마다 호출된다. 눌림/뗌이 바뀐 키만 선택적으로 Queue에 저장한다.
        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (device is not Keyboard keyboard)
            {
                return;
            }

            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            {
                return;
            }

            for (int i = 0; i < KeyBindings.Length; i++)
            {
                KeyControl control = keyboard[KeyBindings[i].KeyboardKey];

                // 이 이벤트에 해당 키 정보가 없으면 건너뛴다.
                if (!control.ReadValueFromEvent(eventPtr, out float value))
                {
                    continue;
                }

                // 이미 알고 있는 상태와 같으면 변화가 아니므로 무시한다.
                bool isPressed = control.IsValueConsideredPressed(value);
                if (isPressed == _isKeyboardKeyDown[i])
                {
                    continue;
                }

                _isKeyboardKeyDown[i] = isPressed;
                _pendingTransitions.Enqueue(new KeyStateTransition(eventPtr.time, i, isPressed));
            }
        }

        // 창이 포커스를 잃으면 눌려 있던 키를 모두 뗀 것으로 처리해, 키가 눌린 채 고정되는 것을 막는다.
        private void OnFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                return;
            }

            double now = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < KeyBindings.Length; i++)
            {
                if (!_isKeyboardKeyDown[i])
                {
                    continue;
                }

                _isKeyboardKeyDown[i] = false;
                _pendingTransitions.Enqueue(new KeyStateTransition(now, i, false));
            }
        }

        // 틱 하나분의 입력 기록을 만든다. tickEndTime 이전에 일어난 사건만 이번 틱에 반영한다.
        public TickInput ConsumeTickInput(double tickEndTime)
        {
            while (_pendingTransitions.Count > 0 && _pendingTransitions.Peek().Timestamp < tickEndTime)
            {
                ApplyKeyStateTransition(_pendingTransitions.Dequeue());
            }

            TickInput tickInput = new TickInput
            {
                PressedKeyMask = _latchedKeyMask,
                LastPressedSlotNumber = (byte)_lastPressedSlotNumber,
                LastPressedHorizontal = _lastPressedHorizontal
            };

            // 계속 누르고 있는 키는 다음 틱에도 1로 시작하고,
            // "마지막으로 눌린 키" 기록은 새 틱에서 다시 비운다.
            _latchedKeyMask = _currentPressedKeyMask;
            _lastPressedSlotNumber = LogicalKeySlots.NoSlot;
            _lastPressedHorizontal = HorizontalDirection.None;

            return tickInput;
        }

        private void ApplyKeyStateTransition(KeyStateTransition transition)
        {
            KeyBinding binding = KeyBindings[transition.BindingIndex];

            if (transition.IsPressed)
            {
                _currentPressedKeyMask |= binding.LogicalKeyFlag;
                _latchedKeyMask |= binding.LogicalKeyFlag;

                if (binding.SlotNumber > LogicalKeySlots.NoSlot)
                {
                    _lastPressedSlotNumber = binding.SlotNumber;
                }

                if (binding.Horizontal != HorizontalDirection.None)
                {
                    _lastPressedHorizontal = binding.Horizontal;
                }
            }
            else
            {
                _currentPressedKeyMask &= ~binding.LogicalKeyFlag;
            }
        }
    }
}