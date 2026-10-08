using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using RuneBlade.Simulation;

namespace RuneBlade.Input
{
    // 키보드 이벤트를 받아 틱마다 InputFrame 한 장을 만들어 주는 "접수 창구".
    public class RawKeySampler : IDisposable
    {
        // 물리 키 하나가 어떤 논리 입력에 대응되는지 적어 둔 항목
        private readonly struct KeyBinding
        {
            public readonly Key Key;
            public readonly LogicalInput LogicalFlag;
            public readonly int SlotIndex;                  // 슬롯 키가 아니면 -1
            public readonly HorizontalDirection Horizontal; // 좌우 키가 아니면 None

            public KeyBinding(Key key, LogicalInput logicalFlag, int slotIndex, HorizontalDirection horizontal)
            {
                Key = key;
                LogicalFlag = logicalFlag;
                SlotIndex = slotIndex;
                Horizontal = horizontal;
            }
        }

        // 키가 눌리거나 떼어진 사건 하나를 적은 쪽지. 대기줄에서 틱이 가져가기를 기다린다.
        private readonly struct KeyTransition
        {
            public readonly double Time;
            public readonly int BindingIndex;
            public readonly bool IsPressed;

            public KeyTransition(double time, int bindingIndex, bool isPressed)
            {
                Time = time;
                BindingIndex = bindingIndex;
                IsPressed = isPressed;
            }
        }

        // 물리 키 → 논리 입력 대응표 (슬롯 Q W E R A S D = 0~6번, 방향은 화살표 키)
        private static readonly KeyBinding[] Bindings =
        {
            SlotBinding(Key.Q, 0),
            SlotBinding(Key.W, 1),
            SlotBinding(Key.E, 2),
            SlotBinding(Key.R, 3),
            SlotBinding(Key.A, 4),
            SlotBinding(Key.S, 5),
            SlotBinding(Key.D, 6),
            new KeyBinding(Key.LeftArrow, LogicalInput.Left, -1, HorizontalDirection.Left),
            new KeyBinding(Key.RightArrow, LogicalInput.Right, -1, HorizontalDirection.Right),
            new KeyBinding(Key.UpArrow, LogicalInput.Up, -1, HorizontalDirection.None),
            new KeyBinding(Key.DownArrow, LogicalInput.Down, -1, HorizontalDirection.None),
        };

        // 이벤트 쪽에서 쓰는 값: 각 키가 지금 눌려 있다고 알고 있는지 (전환 감지용)
        private readonly bool[] _isKeyDown = new bool[Bindings.Length];

        // 아직 어느 틱에도 반영되지 않은 사건들. 도착한 순서 그대로 쌓인다.
        private readonly Queue<KeyTransition> _pendingTransitions = new Queue<KeyTransition>();

        // 틱 쪽에서 쓰는 값들
        private LogicalInput _currentMask;   // 지금 눌려 있는 키
        private LogicalInput _latchedMask;   // 이번 틱 구간에서 한 번이라도 눌린 키 (OR 래치)
        private int _lastSlotIndex = -1;     // 이번 틱 구간에서 마지막으로 눌린 슬롯 (-1 = 없음)
        private HorizontalDirection _lastHorizontal = HorizontalDirection.None;

        public RawKeySampler()
        {
            InputSystem.onEvent += OnInputEvent;
            Application.focusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            InputSystem.onEvent -= OnInputEvent;
            Application.focusChanged -= OnFocusChanged;
        }

        private static KeyBinding SlotBinding(Key key, int slotIndex)
        {
            return new KeyBinding(key, LogicalInputSlots.ToFlag(slotIndex), slotIndex, HorizontalDirection.None);
        }

        // 키보드 이벤트가 올 때마다 호출된다. 눌림/뗌이 바뀐 키만 골라 대기줄에 세운다.
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

            for (int i = 0; i < Bindings.Length; i++)
            {
                KeyControl control = keyboard[Bindings[i].Key];

                // 이 이벤트에 해당 키 정보가 없으면 건너뛴다.
                if (!control.ReadValueFromEvent(eventPtr, out float value))
                {
                    continue;
                }

                // 이미 알고 있는 상태와 같으면 변화가 아니므로 무시한다.
                bool isPressed = control.IsValueConsideredPressed(value);
                if (isPressed == _isKeyDown[i])
                {
                    continue;
                }

                _isKeyDown[i] = isPressed;
                _pendingTransitions.Enqueue(new KeyTransition(eventPtr.time, i, isPressed));
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
            for (int i = 0; i < Bindings.Length; i++)
            {
                if (!_isKeyDown[i])
                {
                    continue;
                }

                _isKeyDown[i] = false;
                _pendingTransitions.Enqueue(new KeyTransition(now, i, false));
            }
        }

        // 틱 하나분의 입력 기록을 만든다. tickEndTime 이전에 일어난 사건만 이번 틱에 반영한다.
        public InputFrame ConsumeFrame(double tickEndTime)
        {
            while (_pendingTransitions.Count > 0 && _pendingTransitions.Peek().Time < tickEndTime)
            {
                ApplyTransition(_pendingTransitions.Dequeue());
            }

            InputFrame frame = new InputFrame
            {
                PressedMask = _latchedMask,
                LastPressedHorizontal = _lastHorizontal
            };

            if (_lastSlotIndex >= 0)
            {
                frame.SetLastPressedSlot(_lastSlotIndex);
            }

            // 다음 틱 준비: 계속 누르고 있는 키는 다음 틱에도 1로 시작하고,
            // "마지막으로 눌린 키" 기록은 새 틱에서 다시 비운다.
            _latchedMask = _currentMask;
            _lastSlotIndex = -1;
            _lastHorizontal = HorizontalDirection.None;

            return frame;
        }

        private void ApplyTransition(KeyTransition transition)
        {
            KeyBinding binding = Bindings[transition.BindingIndex];

            if (transition.IsPressed)
            {
                _currentMask |= binding.LogicalFlag;
                _latchedMask |= binding.LogicalFlag;

                // 나중에 도착한 눌림이 "마지막으로 눌린 키"가 된다.
                if (binding.SlotIndex >= 0)
                {
                    _lastSlotIndex = binding.SlotIndex;
                }

                if (binding.Horizontal != HorizontalDirection.None)
                {
                    _lastHorizontal = binding.Horizontal;
                }
            }
            else
            {
                // 뗀 것은 "현재 눌림"에서만 지우고, 래치에는 남겨 짧은 탭이 사라지지 않게 한다.
                _currentMask &= ~binding.LogicalFlag;
            }
        }
    }
}