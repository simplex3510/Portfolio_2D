using System.Collections.Generic;
using UnityEngine;
using RuneBlade.Simulation;

namespace RuneBlade.Input
{
    // 임시 확인용: RawKeySampler가 만든 InputFrame이 의도대로인지 눈으로 확인한다. 확인 후 삭제한다.
    public class RawKeySamplerTester : MonoBehaviour
    {
        // 낮게(예: 10) 설정하면 한 틱이 길어져서 "한 틱 안에 눌렀다 뗀 탭"을 손으로 재현하기 쉽다.
        [SerializeField] private int _ticksPerSecond = 60;

        private const int MaxScreenLines = 20;

        private readonly Queue<string> _screenLines = new Queue<string>();
        private RawKeySampler _sampler;
        private double _tickDuration;
        private double _nextTickEndTime;
        private int _tickCount;
        private InputFrame _previousFrame;

        private void OnEnable()
        {
            _sampler = new RawKeySampler();
            _tickDuration = 1.0 / _ticksPerSecond;
            _nextTickEndTime = Time.realtimeSinceStartupAsDouble + _tickDuration;
            _tickCount = 0;
            _previousFrame = default;
        }

        private void OnDisable()
        {
            _sampler.Dispose();
        }

        private void Update()
        {
            double now = Time.realtimeSinceStartupAsDouble;

            // 프레임이 밀려 처리하지 못한 틱이 있으면 한 번에 여러 틱을 처리한다.
            while (now >= _nextTickEndTime)
            {
                InputFrame frame = _sampler.ConsumeFrame(_nextTickEndTime);
                _nextTickEndTime += _tickDuration;
                _tickCount++;

                // 직전 틱과 달라졌을 때만 기록해 화면이 도배되지 않게 한다.
                if (!IsSame(frame, _previousFrame))
                {
                    LogFrame(frame);
                }

                _previousFrame = frame;
            }
        }

        private static bool IsSame(InputFrame a, InputFrame b)
        {
            return a.PressedMask == b.PressedMask
                && a.LastPressedSlotCode == b.LastPressedSlotCode
                && a.LastPressedHorizontal == b.LastPressedHorizontal;
        }

        private void LogFrame(InputFrame frame)
        {
            string lastSlot = frame.TryGetLastPressedSlot(out int slotIndex) ? slotIndex.ToString() : "-";
            string line = $"tick {_tickCount}: mask=[{frame.PressedMask}] lastSlot={lastSlot} lastH={frame.LastPressedHorizontal}";

            Debug.Log(line);

            _screenLines.Enqueue(line);
            while (_screenLines.Count > MaxScreenLines)
            {
                _screenLines.Dequeue();
            }
        }

        private void OnGUI()
        {
            GUILayout.Label($"ticks/sec = {_ticksPerSecond}");
            foreach (string line in _screenLines)
            {
                GUILayout.Label(line);
            }
        }
    }
}