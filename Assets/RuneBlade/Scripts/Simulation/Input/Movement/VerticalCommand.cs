namespace RuneBlade.Simulation
{
    /// <summary>
    /// 수직 입력(↑, ↓↓) 판정이 내놓는 동작. 스냅샷·해시에 들어가지 않는 일회성 값이다.
    /// 좌우 이동과 대시는 이 타입이 아니라 HorizontalInputResolver 쪽에서 다룬다.
    /// </summary>
    public enum VerticalCommand : byte
    {
        None = 0,
        Jump = 1,
        DoubleJump = 2,
        DropThrough = 3,
    }
}
