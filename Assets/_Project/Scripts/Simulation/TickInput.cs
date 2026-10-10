namespace RuneBlade.Simulation
{
    // 한 틱 구간의 입력 기록. 복사 가능한 순수 데이터이며 롤백 복원과 네트워크 전송의 단위이다.
    public struct TickInput
    {
        // 해당 틱 구간 중 한 번이라도 눌린 입력은 1 (OR 래치 반영)
        public LogicalKeyFlags PressedKeyMask;

        // TODO: Press 에지가 있는 키만 "마지막 키" 후보로 삼는다 (논의 후 반영)
        // 틱 구간에서 Press 에지가 있던 슬롯 중 마지막으로 눌린 슬롯 번호.
        // 0 = 없음, 1~7 = 슬롯 번호 (3비트 분량). default 값이 곧 "없음"이다.
        public byte LastPressedSlotNumber;

        // 틱 구간에서 Press 에지가 있던 좌우 키 중 마지막으로 눌린 방향 (2비트 분량)
        public HorizontalDirection LastPressedHorizontal;

        public bool TryGetLastPressedSlot(out int slotNumber)
        {
            slotNumber = LastPressedSlotNumber;
            return LastPressedSlotNumber != LogicalKeySlots.NoSlot;
        }
    }
}