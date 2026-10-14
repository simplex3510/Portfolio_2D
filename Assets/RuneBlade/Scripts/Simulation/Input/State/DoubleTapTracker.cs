using System;

namespace RuneBlade.Simulation
{
    /// <summary>
    /// 더블탭 이력. 방향키 Press 이벤트를 도착 순서대로 받아, 같은 방향이 창 안에서 두 번 눌렸는지 판정한다.
    ///
    /// 유효한 첫 Press는 항상 "마지막 방향 Press 하나"이므로(designs/102_이동입력.md), 방향별 이력 3개가 아니라
    /// 기록 1개(키 + 남은 틱)로 구현한다. 다른 방향 Press가 끼면 기록이 새 키로 교체되어
    /// 기존 첫 Press가 자연히 무효화된다(← → ← 는 대시가 아님).
    ///
    /// 창은 틱 번호 차이가 아니라 "남은 틱 카운트다운"으로 센다. 시간이 멈추는 구간에서는 호출하는 쪽이
    /// CountDownTick을 부르지 않는 것만으로 창이 동결된다.
    /// Clear로 초기화한다. 스냅샷·해시에 포함된다.
    /// TODO: 초기화·동결이 필요한 구간(라운드 경계, 히트스톱 등)은 기획 재작성 후 확정한다.
    /// </summary>
    public struct DoubleTapTracker : IEquatable<DoubleTapTracker>
    {
        // 창 길이는 byte에 담기는 범위까지만 허용한다. 실제 값은 [잠정] 수치이며 호출하는 쪽이 넘긴다.
        public const int MaxWindowTicks = byte.MaxValue;

        private bool _isActive;
        private LogicalKey _key;      // _isActive가 true일 때만 의미가 있다
        private byte _remainingTicks; // 남은 창. 0이어도 이번 틱까지는 유효(CountDownTick 참조)

        public readonly bool IsActive => _isActive;

        public readonly LogicalKey Key => _key;

        public readonly int RemainingTicks => _remainingTicks;

        /// <summary>
        /// 방향키 Press 하나를 이력에 반영하고, 더블탭이 인식되면 true를 돌려준다.
        ///
        /// 인식 조건: 기록이 있고, 같은 키이고, 창이 남아 있고, 키가 ←, →, ↓ 중 하나일 때.
        ///  - 인식되면 기록을 비운다(실행 여부와 무관. 막힌 인식이 다음 더블탭의 첫 Press로 재사용되지 않게).
        ///  - 인식되지 않으면 이 Press를 새 첫 Press로 기록한다(↑ 포함. ↑↑는 이단 점프이며 인식 대상이 아님).
        ///
        /// 방향키가 아닌 키는 이력에 영향을 주지 않고 false를 돌려준다.
        /// "이력 미반영 상태"(현재 확정된 것은 대시 진행 중)에서는 호출하는 쪽이 이 메서드를 부르지 않는다.
        /// </summary>
        public bool RegisterPress(LogicalKey key, int windowTicks)
        {
            if (!LogicalKeyUtility.IsDirection(key))
            {
                return false;
            }

            if ((uint)windowTicks > MaxWindowTicks)
            {
                throw new ArgumentOutOfRangeException(nameof(windowTicks));
            }

            bool isRecognizable = key != LogicalKey.Up;
            if (_isActive && _key == key && isRecognizable)
            {
                this = default; // 인식 완료: 이력 초기화
                return true;
            }

            // 인식 아님: 이 Press가 새 첫 Press가 된다(기존 기록은 교체되어 무효화).
            _isActive = true;
            _key = key;
            _remainingTicks = (byte)windowTicks;
            return false;
        }

        /// <summary>
        /// 시간이 흐르는 틱의 시작에 한 번 호출한다(시간이 멈추는 구간에서는 호출하지 않는다).
        /// 남은 틱이 0이면 기록이 만료되고, 아니면 1 줄어든다.
        /// 따라서 첫 Press 이후 d틱 뒤의 Press까지 인식된다(d ≤ windowTicks).
        /// </summary>
        public void CountDownTick()
        {
            if (!_isActive)
            {
                return;
            }

            if (_remainingTicks == 0)
            {
                this = default;
                return;
            }

            _remainingTicks--;
        }

        /// <summary>이력을 비운다.</summary>
        public void Clear()
        {
            this = default;
        }

        public readonly bool Equals(DoubleTapTracker other)
        {
            return _isActive == other._isActive
                && _key == other._key
                && _remainingTicks == other._remainingTicks;
        }

        public override readonly bool Equals(object obj) => obj is DoubleTapTracker other && Equals(other);

        public override readonly int GetHashCode() => HashCode.Combine(_isActive, (byte)_key, _remainingTicks);

        public static bool operator ==(DoubleTapTracker left, DoubleTapTracker right) => left.Equals(right);

        public static bool operator !=(DoubleTapTracker left, DoubleTapTracker right) => !left.Equals(right);

        public override readonly string ToString()
        {
            return _isActive ? $"Active({_key}, remaining={_remainingTicks})" : "Inactive";
        }
    }
}