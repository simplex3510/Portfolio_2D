using System;
using System.Diagnostics;

namespace RuneBlade.Simulation
{
    // 시뮬레이션이 받는 논리 입력. 물리 키 이름은 알지 못하고 슬롯 번호로만 구분한다.
    [Flags]
    public enum LogicalKeyFlags : ushort
    {
        None = 0,

        // 방향 (비트 0~3)
        Left = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,

        // 슬롯 (비트 4~10), 슬롯 번호 1~7
        KeySlot1 = 1 << 4,
        KeySlot2 = 1 << 5,
        KeySlot3 = 1 << 6,
        KeySlot4 = 1 << 7,
        KeySlot5 = 1 << 8,
        KeySlot6 = 1 << 9,
        KeySlot7 = 1 << 10,
    }

    // 슬롯 번호 ↔ 비트 플래그 변환
    public static class LogicalKeySlots
    {
        public const int NoSlot = 0;
        public const int Count = 7;
        public const int FirstSlotNumber = 1;

        private const int FirstSlotBit = 4;

        public static LogicalKeyFlags ConvertToFlag(int slotNumber)
        {
            Debug.Assert(slotNumber >= FirstSlotNumber && slotNumber < FirstSlotNumber + Count, "슬롯 번호 범위 초과");
            int bitOffset = slotNumber - FirstSlotNumber;   // 0~6
            return (LogicalKeyFlags)(1 << (FirstSlotBit + bitOffset));
        }
    }
}