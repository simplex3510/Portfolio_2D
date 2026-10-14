using System;
using System.Text;

namespace RuneBlade.Simulation
{
    /// <summary>
    /// 한 틱 구간에서 한 플레이어에게 일어난 키 전이 이벤트 목록(도착 순서 그대로).
    /// 시뮬레이션이 받는 입력의 전부이며, 결정성의 경계선이다.
    /// 이 값이 같으면 시뮬레이션 결과도 같아야 한다.
    ///
    /// 저장 방식: ulong 1개에 이벤트 바이트를 순서대로 채운다(0번 이벤트가 최하위 바이트).
    /// 배열·힙 할당이 없고, 값 비교·해시·직렬화가 단순해진다.
    ///
    /// 담는 것은 키 전이의 순수 정보뿐이다. 눌림 상태·우선 방향 같은 파생값은
    /// 담지 않으며 시뮬레이션이 이벤트를 처리하며 보유한다.
    /// </summary>
    public struct TickInput : IEquatable<TickInput>
    {
        /// <summary>
        /// 한 틱에 담을 수 있는 이벤트 수 [잠정]. 현재 저장 방식(ulong 1개)에서는 8이 구조적 상한이다.
        /// 더 필요해지면 저장을 ulong 2개로 바꿔야 한다. (60Hz 한 틱 약 16.7ms 안에 키 전이 8회는
        /// 정상 플레이에서 거의 나오지 않는다.)
        /// </summary>
        public const int Capacity = 8;

        private const int BitsPerEvent = 8;

        // 이벤트 바이트들. 0번 이벤트 = 하위 8비트. 사용하지 않는 칸은 항상 0으로 유지한다
        // (같은 입력이면 같은 비트 패턴이 되어야 비교·해시가 안전하다).
        private ulong _events;
        private byte _eventCount;

        /// <summary>이 틱에 담긴 이벤트 수.</summary>
        public readonly int EventCount => _eventCount;

        public readonly bool IsEmpty => _eventCount == 0;

        public readonly bool IsFull => _eventCount >= Capacity;

        /// <summary>저장 원본. 리플레이·네트워크 직렬화용(Count와 함께 저장한다).</summary>
        public readonly ulong RawEvents => _events;

        /// <summary>
        /// 이벤트를 목록 끝에 추가한다. 가득 차 있으면 false를 돌려주고 아무것도 바꾸지 않는다.
        /// 넘친 이벤트를 다음 틱으로 이월하는 일은 이 구조체가 아니라 Input 어셈블리(수집 쪽)의 몫이다.
        /// </summary>
        public bool TryAdd(KeyInputEvent inputEvent)
        {
            if (_eventCount >= Capacity)
            {
                return false;
            }

            // count번째 칸(바이트)에 이벤트를 넣는다.
            _events |= (ulong)inputEvent.PackedEvent << (_eventCount * BitsPerEvent);
            _eventCount++;
            return true;
        }

        /// <summary>index번째 이벤트를 읽는다(0 <= index < Count). 범위를 벗어나면 프로그래밍 오류로 예외.</summary>
        public readonly KeyInputEvent GetEventAt(int index)
        {
            if ((uint)index >= _eventCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            byte packed = (byte)(_events >> (index * BitsPerEvent));
            return KeyInputEvent.CreateFromPacked(packed);
        }

        /// <summary>
        /// 저장 원본(RawEvents, Count)에서 복원한다. 리플레이 파일·네트워크 수신처럼
        /// 외부에서 온 값이므로 검증을 거친다. 검증 항목:
        ///  - 개수가 용량 이내
        ///  - 사용하지 않는 칸이 전부 0 (정규 형태)
        ///  - 각 이벤트가 유효(예약 비트 0, 키 범위 이내)
        /// 통과하지 못하면 false이며 result는 빈 입력이다. 거부 후 처리는 호출하는 쪽이 정한다.
        /// </summary>
        public static bool TryCreateFromRawEvents(ulong rawEvents, int eventCount, out TickInput result)
        {
            result = default;

            if (eventCount < 0 || eventCount > Capacity)
            {
                return false;
            }

            // 사용하는 칸을 뺀 나머지 비트가 0이어야 정규 형태이다.
            int usedBits = eventCount * BitsPerEvent;
            ulong unusedBits = usedBits >= 64 ? 0UL : rawEvents >> usedBits;
            if (unusedBits != 0UL)
            {
                return false;
            }

            TickInput candidate = default;
            candidate._events = rawEvents;
            candidate._eventCount = (byte)eventCount;

            for (int i = 0; i < eventCount; i++)
            {
                if (!candidate.GetEventAt(i).IsValid)
                {
                    return false;
                }
            }

            result = candidate;
            return true;
        }

        public readonly bool Equals(TickInput other)
        {
            return _events == other._events && _eventCount == other._eventCount;
        }

        public override readonly bool Equals(object obj) => obj is TickInput other && Equals(other);

        public override readonly int GetHashCode() => HashCode.Combine(_events, _eventCount);

        public static bool operator ==(TickInput left, TickInput right) => left.Equals(right);

        public static bool operator !=(TickInput left, TickInput right) => !left.Equals(right);

        /// <summary>디버그·테스트 로그용. 예: [Press Left, Press Right]</summary>
        public override readonly string ToString()
        {
            var builder = new StringBuilder("[");
            for (int i = 0; i < _eventCount; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(GetEventAt(i).ToString());
            }

            builder.Append(']');
            return builder.ToString();
        }
    }
}