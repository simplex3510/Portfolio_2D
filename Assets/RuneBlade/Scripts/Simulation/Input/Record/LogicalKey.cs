namespace RuneBlade.Simulation
{
    /// <summary>
    /// 시뮬레이션이 인식하는 논리키 11종(방향 4 + 슬롯 7).
    /// 물리 키(Q, W, 화살표 등)와의 매핑은 Input 어셈블리가 담당하고,
    /// Simulation은 이 값만 안다.
    /// 값은 TickInput 이벤트 1바이트의 하위 4비트에 그대로 들어가므로(0~15 범위) 번호를 바꾸면 안 된다.
    /// </summary>
    public enum LogicalKey : byte
    {
        // 방향키 (화살표 고정, 사용자 정의 불가)
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,

        // 스킬 슬롯 (슬롯 번호는 1부터. 0은 "없음"으로 예약)
        Slot1 = 4,
        Slot2 = 5,
        Slot3 = 6,
        Slot4 = 7,
        Slot5 = 8,
        Slot6 = 9,
        Slot7 = 10
    }

    public static class LogicalKeyUtility
    {
        /// <summary>논리키 종류 수. 눌림 마스크 비트 수와 같다.</summary>
        public const int KeyCount = 11;

        /// <summary>이벤트 1바이트의 키 필드(4비트)가 담을 수 있는 최댓값.</summary>
        public const int KeyFieldMax = 15;

        private const int FirstSlotValue = (int)LogicalKey.Slot1;

        /// <summary>디코딩된 값이 실제 논리키 범위인지 확인한다(손상된 기록 방어).</summary>
        public static bool IsValid(LogicalKey key)
        {
            return (int)key < KeyCount;
        }

        public static bool IsDirection(LogicalKey key)
        {
            return key <= LogicalKey.Down;
        }

        public static bool IsSlot(LogicalKey key)
        {
            return key >= LogicalKey.Slot1 && (int)key < KeyCount;
        }

        /// <summary>
        /// 슬롯 키를 슬롯 번호(1~7)로 변환한다. 슬롯 키가 아니면 0("없음")을 돌려준다.
        /// </summary>
        public static int ToSlotNumber(LogicalKey key)
        {
            if (!IsSlot(key))
            {
                return 0;
            }

            return (int)key - FirstSlotValue + 1;
        }
    }
}