using System;

namespace RuneBlade.Simulation
{
    /// <summary>
    /// 점프 선입력 버퍼. 공중에서 이단 점프가 불가할 때 누른 ↑ 하나를 저장해 두고, 착지 시 소비한다.
    /// 저장은 항상 1칸이다. 이미 저장된 상태에서 다시 저장하면 유지 시간만 갱신한다.
    ///
    /// 유지 시간은 DoubleTapTracker와 같은 방식의 "남은 틱 카운트다운"이다.
    /// 시간이 멈추는 구간에서는 호출하는 쪽이 CountDownTick을 부르지 않는 것만으로 동결된다.
    /// 슬롯 입력 버퍼와는 별개이다. 스냅샷·해시에 포함된다.
    /// TODO: 초기화·동결이 필요한 구간(라운드 경계, 히트스톱 등)은 기획 재작성 후 확정한다.
    /// </summary>
    public struct JumpInputBuffer : IEquatable<JumpInputBuffer>
    {
        // 유지 시간은 byte에 담기는 범위까지만 허용한다. 실제 값은 MovementSettings.JumpBufferTicks이다.
        public const int MaxBufferTicks = byte.MaxValue;

        private bool _isStored;
        private byte _remainingTicks; // 0이어도 이번 틱까지는 유효(CountDownTick 참조)

        public readonly bool IsStored => _isStored;

        public readonly int RemainingTicks => _remainingTicks;

        /// <summary>점프 입력을 저장한다. 이미 저장되어 있으면 유지 시간을 새로 시작한다.</summary>
        public void Store(int bufferTicks)
        {
            if ((uint)bufferTicks > MaxBufferTicks)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferTicks));
            }

            _isStored = true;
            _remainingTicks = (byte)bufferTicks;
        }

        /// <summary>
        /// 저장된 점프가 있으면 비우고 true를 돌려준다(착지 시 호출). 없으면 false이다.
        /// </summary>
        public bool TryConsume()
        {
            if (!_isStored)
            {
                return false;
            }

            this = default;
            return true;
        }

        /// <summary>
        /// 시간이 흐르는 틱의 시작에 한 번 호출한다(시간이 멈추는 구간에서는 호출하지 않는다).
        /// 남은 틱이 0이면 만료되고, 아니면 1 줄어든다.
        /// 따라서 저장 후 d틱 뒤의 착지까지 소비된다(d ≤ bufferTicks).
        /// </summary>
        public void CountDownTick()
        {
            if (!_isStored)
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

        /// <summary>저장된 점프를 버린다.</summary>
        public void Clear()
        {
            this = default;
        }

        public readonly bool Equals(JumpInputBuffer other)
        {
            return _isStored == other._isStored && _remainingTicks == other._remainingTicks;
        }

        public override readonly bool Equals(object obj) => obj is JumpInputBuffer other && Equals(other);

        public override readonly int GetHashCode() => HashCode.Combine(_isStored, _remainingTicks);

        public static bool operator ==(JumpInputBuffer left, JumpInputBuffer right) => left.Equals(right);

        public static bool operator !=(JumpInputBuffer left, JumpInputBuffer right) => !left.Equals(right);

        public override readonly string ToString()
        {
            return _isStored ? $"Stored(remaining={_remainingTicks})" : "Empty";
        }
    }
}
