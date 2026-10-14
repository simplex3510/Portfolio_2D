using System;

namespace RuneBlade.Simulation
{
    /// <summary>
    /// 좌우 방향의 현재 우선 방향. "나중에 누른 쪽 우선" 규칙의 결과값이다.
    /// 값은 스냅샷·해시에 들어가므로 번호를 바꾸면 안 된다.
    /// </summary>
    public enum HorizontalDirection : byte
    {
        None = 0,
        Left = 1,
        Right = 2,
    }

    /// <summary>
    /// 한 플레이어의 키 눌림 상태와 좌우 우선 방향. 둘 다 TickInput이 아니라
    /// 시뮬레이션이 이벤트를 처리하며 보유하는 파생 상태이며, 스냅샷·해시에 포함된다.
    ///
    /// 라운드가 바뀌어도 초기화하지 않는다(104 라운드 경계 규칙: 눌림과 우선 방향은 유지).
    /// 매치 시작 시에는 default(전부 해제)에서 출발한다.
    /// 정지·피격 경직·사망·연출 구간에서도 갱신은 계속한다.
    /// 막는 것은 갱신이 아니라 그 이벤트가 일으킬 동작이며, 그 판정은 이 구조체의 일이 아니다.
    /// </summary>
    public struct InputState : IEquatable<InputState>
    {
        // 논리키 11종의 눌림 비트. 비트 번호 = (int)LogicalKey.
        private ushort _heldKeyMask;

        // 좌우 우선 방향. 항상 "현재 눌려 있는 쪽"을 가리키도록 유지한다(아래 Apply 참조).
        private HorizontalDirection _horizontal;

        /// <summary>눌림 마스크 원본. 해시·스냅샷용.</summary>
        public readonly ushort HeldKeyMask => _heldKeyMask;

        /// <summary>
        /// 현재 좌우 이동 의도. 양쪽이 눌려 있으면 나중에 누른 쪽, 한쪽만 눌려 있으면 그쪽, 없으면 None.
        /// 이동은 이벤트가 아니라 이 값에서 매 틱 도출한다.
        /// </summary>
        public readonly HorizontalDirection Horizontal => _horizontal;

        public readonly bool IsHeldKey(LogicalKey key)
        {
            if (!LogicalKeyUtility.IsValid(key))
            {
                return false;
            }

            return (_heldKeyMask & (1 << (int)key)) != 0;
        }

        /// <summary>
        /// 이벤트 하나를 눌림 상태에 반영한다(같은 틱 규칙 1단계).
        ///
        /// true  = 상태가 실제로 바뀜(정상 이벤트). 호출자는 이어서 실행 판정(3·4단계)을 진행한다.
        /// false = 현재 눌림 상태와 모순되는 이벤트이며 상태는 그대로이다. 호출자는 그 이벤트가 일으킬
        ///         동작을 없던 것으로 한다. 모순의 경우:
        ///         - 이미 눌려 있는 키의 Press
        ///         - 눌려 있지 않은 키의 Release
        ///         - 논리키 범위 밖의 키
        /// </summary>
        public bool TryApplyKeyEvent(KeyInputEvent keyInputEvent)
        {
            LogicalKey eventKey = keyInputEvent.Key;
            if (!LogicalKeyUtility.IsValid(eventKey))
            {
                return false;
            }

            int eventKeyBit = 1 << (int)eventKey;

            // 이벤트가 발생한 키가 현재 눌려 있는지 확인
            bool isHeldKey = (_heldKeyMask & eventKeyBit) != 0;

            // Press 이벤트 처리
            if (keyInputEvent.IsPress)
            {
                if (isHeldKey)
                {
                    // 이미 눌려 있는 키를 또 누른 경우: 모순
                    return false;
                }

                // Press 이벤트가 발생한 키를 눌림 상태에 반영.
                _heldKeyMask |= (ushort)eventKeyBit;

                if (eventKey == LogicalKey.Left)
                {
                    _horizontal = HorizontalDirection.Left;
                }
                else if (eventKey == LogicalKey.Right)
                {
                    _horizontal = HorizontalDirection.Right;
                }

                return true;
            }
            // Release 이벤트 처리
            else
            {
                if (!isHeldKey)
                {
                    // 눌려 있지 않은 키를 뗀 경우: 모순
                    return false;
                }

                // Release 이벤트가 발생한 키를 눌림 상태에서 제거.
                _heldKeyMask &= (ushort)~eventKeyBit;

                // Release키가 왼쪽방향키이고, 현재 입력 상태가 왼쪽방향이면
                if (eventKey == LogicalKey.Left && _horizontal == HorizontalDirection.Left)
                {
                    // 오른쪽 방향키가 눌려있으면 오른쪽으로 방향 전환
                    _horizontal = IsHeldKey(LogicalKey.Right) ? HorizontalDirection.Right : HorizontalDirection.None;
                }
                else if (eventKey == LogicalKey.Right && _horizontal == HorizontalDirection.Right)
                {
                    _horizontal = IsHeldKey(LogicalKey.Left) ? HorizontalDirection.Left : HorizontalDirection.None;
                }

                return true;
            }
        }

        public readonly bool Equals(InputState other)
        {
            return _heldKeyMask == other._heldKeyMask && _horizontal == other._horizontal;
        }

        public override readonly bool Equals(object obj) => obj is InputState other && Equals(other);

        public override readonly int GetHashCode() => HashCode.Combine(_heldKeyMask, (byte)_horizontal);

        public static bool operator ==(InputState left, InputState right) => left.Equals(right);

        public static bool operator !=(InputState left, InputState right) => !left.Equals(right);

        public override readonly string ToString() => $"Held=0x{_heldKeyMask:X3}, Horizontal={_horizontal}";
    }
}
