using RuneBlade.Simulation;

namespace RuneBlade.Network
{
    /// <summary>
    /// 서버 → 클라이언트 상태 메시지. 클라이언트 보정(reconciliation)의 기준점이 된다.
    /// LastProcessedInputTick까지의 입력을 반영한 결과가 State이다. 클라이언트는 비교 없이 항상
    /// 그 틱의 예측 상태를 State로 덮고, 그 다음 틱부터의 입력을 다시 적용한다(MovementReconciler).
    ///
    /// TODO: State는 MovementSimulation 값을 그대로 담는다(D-016). 바이트 직렬화가 필요한 실제 전송으로
    ///       바꿀 때 plain struct(Capture/Restore)로 분리한다.
    /// </summary>
    public readonly struct SnapshotMessage
    {
        public readonly int LastProcessedInputTick;
        public readonly MovementSimulation State;

        public SnapshotMessage(int lastProcessedInputTick, MovementSimulation state)
        {
            LastProcessedInputTick = lastProcessedInputTick;
            State = state;
        }
    }
}
