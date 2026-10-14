namespace RuneBlade.Simulation
{
    /// <summary>
    /// 이동 도메인의 입력 처리 앞단. 키 이벤트 하나를 받아 더블탭 이력을 갱신하고,
    /// 좌우는 HorizontalInputResolver(대시), 상하는 VerticalInputResolver(점프·하강)로 보낸다.
    ///
    /// InputState에 이벤트를 반영하는 일은 여기서 하지 않는다. 눌림 상태는 이동·공격 도메인이 공유하므로
    /// 호출자(틱 루프)가 이벤트마다 한 번만 반영하고, 정상 이벤트만 이 처리기로 넘긴다.
    /// 좌우 이동 자체는 이벤트가 아니라 InputState.Horizontal에서 도출하므로 다루지 않는다.
    /// </summary>
    public static class MovementInputProcessor
    {
        /// <summary>
        /// 이벤트 하나를 처리한다. 이동 도메인과 무관한 이벤트(Release, 방향키가 아닌 키)는 둘 다 None으로 끝난다.
        /// 시간이 흐르는 틱의 시작에 doubleTapTracker·jumpBuffer의 CountDownTick을 부르는 일은 호출자의 몫이다.
        /// </summary>
        public static void Process(
            KeyInputEvent keyInputEvent,
            in MovementInputContext context,
            in MovementInputTuning tuning,
            ref DoubleTapTracker doubleTapTracker,
            ref JumpInputBuffer jumpBuffer,
            out VerticalCommand verticalCommand,
            out HorizontalDirection dashDirection)
        {
            verticalCommand = VerticalCommand.None;
            dashDirection = HorizontalDirection.None;

            LogicalKey key = keyInputEvent.Key;
            if (!keyInputEvent.IsPress || !LogicalKeyUtility.IsDirection(key))
            {
                return;
            }

            // 대시 진행 중의 누름은 더블탭 이력을 새로 만들지도, 기존 이력을 무효화하지도 않는다.
            bool isDoubleTapRecognized = false;
            if (!context.IsDashing)
            {
                isDoubleTapRecognized = doubleTapTracker.RegisterPress(key, tuning.DoubleTapWindowTicks);
            }

            if (key == LogicalKey.Left || key == LogicalKey.Right)
            {
                dashDirection = HorizontalInputResolver.ResolveDash(key, isDoubleTapRecognized, context);
            }
            else
            {
                verticalCommand = VerticalInputResolver.Resolve(key, isDoubleTapRecognized, context, tuning, ref jumpBuffer);
            }
        }
    }
}
