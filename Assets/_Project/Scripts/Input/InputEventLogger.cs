using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace RuneBlade.Input
{
    // 임시 검증용: 원시 키 이벤트의 선후 기록 가능 여부, 타임스탬프 시간축, IME 상태별 동작을 확인한다.
    // 검증이 끝나면 삭제한다.
    public class InputEventLogger : MonoBehaviour
    {
        // 기획에서 확정된 슬롯 7개 + 방향키 4개
        private static readonly Key[] TrackedKeys =
        {
            Key.Q, Key.W, Key.E, Key.R, Key.A, Key.S, Key.D,
            Key.LeftArrow, Key.RightArrow, Key.UpArrow, Key.DownArrow
        };

        private const int MaxScreenLines = 20;

        private readonly bool[] _wasPressed = new bool[TrackedKeys.Length];
        private readonly Queue<string> _screenLines = new Queue<string>();
        private int _eventCounter;

        private void OnEnable()
        {
            InputSystem.onEvent += OnInputEvent;

            if (Keyboard.current != null)
            {
                AddLog($"keyboardLayout = {Keyboard.current.keyboardLayout}");
            }
        }

        private void OnDisable()
        {
            InputSystem.onEvent -= OnInputEvent;
        }

        // 이벤트가 기기 상태에 반영되기 전에 호출되므로, 이벤트 자체에서 키 상태를 읽는다.
        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (device is not Keyboard keyboard)
            {
                return;
            }

            bool isStateEvent = eventPtr.IsA<StateEvent>();
            if (!isStateEvent && !eventPtr.IsA<DeltaStateEvent>())
            {
                return;
            }

            _eventCounter++;

            for (int i = 0; i < TrackedKeys.Length; i++)
            {
                KeyControl control = keyboard[TrackedKeys[i]];

                // 이 이벤트가 해당 키의 상태를 담고 있지 않으면 건너뛴다.
                if (!control.ReadValueFromEvent(eventPtr, out float value))
                {
                    continue;
                }

                // 이전 상태와 달라진 키만 전환(Down/Up)으로 기록한다.
                bool isPressed = control.IsValueConsideredPressed(value);
                if (isPressed == _wasPressed[i])
                {
                    continue;
                }
                _wasPressed[i] = isPressed;

                double eventTime = eventPtr.time;
                double nowTime = Time.realtimeSinceStartupAsDouble;

                AddLog($"#{_eventCounter} {(isStateEvent ? "State" : "Delta")} " +
                       $"f={Time.frameCount} tEvent={eventTime:F4} " +
                       $"diff={(nowTime - eventTime) * 1000.0:F1}ms " +
                       $"{TrackedKeys[i]} {(isPressed ? "Down" : "Up")}");
            }
        }

        private void AddLog(string line)
        {
            Debug.Log(line);

            _screenLines.Enqueue(line);
            while (_screenLines.Count > MaxScreenLines)
            {
                _screenLines.Dequeue();
            }
        }

        // 빌드에서는 콘솔을 볼 수 없으므로 화면에도 최근 로그를 표시한다.
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            foreach (string line in _screenLines)
            {
                GUILayout.Label(line);
            }
            GUILayout.EndVertical();
        }
    }
}