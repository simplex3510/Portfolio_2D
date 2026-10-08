namespace RuneBlade.Simulation
{
    // 좌우 방향 중 마지막으로 눌린 쪽
    public enum HorizontalDirection : byte
    {
        None = 0,
        Left = 1,
        Right = 2
    }

    // 한 틱 구간의 입력 기록. 복사 가능한 순수 데이터이며 롤백 복원과 네트워크 전송의 단위이다.
    public struct InputFrame
    {
        // 해당 틱 구간 중 한 번이라도 눌린 입력은 1 (OR 래치 반영)
        public LogicalInput PressedMask;

        // 틱 구간에서 마지막으로 눌린 슬롯. 0이면 없음, 1~7이면 슬롯 번호 + 1.
        // default 값이 곧 "없음"이 되도록 한 인코딩이며, 7개 슬롯 + 없음 = 8가지라 3비트로 충분하다.
        public byte LastPressedSlotCode;

        // 틱 구간에서 마지막으로 눌린 좌우 방향 (2비트 분량)
        public HorizontalDirection LastPressedHorizontal;

        public bool TryGetLastPressedSlot(out int slotIndex)
        {
            slotIndex = LastPressedSlotCode - 1;
            return LastPressedSlotCode != 0;
        }

        public void SetLastPressedSlot(int slotIndex)
        {
            LastPressedSlotCode = (byte)(slotIndex + 1);
        }
    }
}