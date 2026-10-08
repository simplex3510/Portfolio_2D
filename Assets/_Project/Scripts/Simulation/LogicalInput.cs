using System;
using System.Diagnostics;

namespace RuneBlade.Simulation
{
    // 시뮬레이션이 받는 논리 입력. 물리 키 이름은 알지 못하고 슬롯 번호로만 구분한다.
    [Flags]
    public enum LogicalInput : ushort
    {
        None = 0,

        // 방향 (비트 0~3)
        Left = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,

        // 슬롯 (비트 4~10), 슬롯 번호 0~6
        Slot0 = 1 << 4,
        Slot1 = 1 << 5,
        Slot2 = 1 << 6,
        Slot3 = 1 << 7,
        Slot4 = 1 << 8,
        Slot5 = 1 << 9,
        Slot6 = 1 << 10,
    }

    // 슬롯 번호 ↔ 비트 플래그 변환
    public static class LogicalInputSlots
    {
        public const int Count = 7;

        private const int FirstSlotBit = 4;

        public static LogicalInput ToFlag(int slotIndex)
        {
            Debug.Assert(slotIndex >= 0 && slotIndex < Count, "슬롯 번호 범위 초과");
            return (LogicalInput)(1 << (FirstSlotBit + slotIndex));
        }
    }
}