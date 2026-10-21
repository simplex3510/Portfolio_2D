namespace RuneBlade.Client
{
    /// <summary>MovementReconciler가 스냅샷 하나를 처리한 결과.</summary>
    public enum ReconcileResult
    {
        /// <summary>쓸 수 없는 스냅샷이라 아무것도 바꾸지 않았다(이력에서 밀려난 틱, 이미 처리한 틱보다 오래된 틱).</summary>
        Ignored,

        /// <summary>서버 상태로 덮고 재적용했지만 화면 위치는 오차 범위 안이었다.</summary>
        Matched,

        /// <summary>서버 상태로 덮고 재적용한 결과, 화면 위치가 오차 범위를 넘어 바뀌었다.</summary>
        Corrected,
    }
}
