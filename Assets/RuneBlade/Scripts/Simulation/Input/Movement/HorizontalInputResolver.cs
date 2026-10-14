namespace RuneBlade.Simulation
{
    /// <summary>
    /// 좌우 입력의 이벤트 판정(대시). 좌우 이동 자체는 이벤트가 아니라 InputState.Horizontal에서 매 틱 도출하므로
    /// 여기서 다루지 않는다. 상태는 갖지 않으며, 실행하지 못한 입력은 폐기한다(폴백 없음).
    /// </summary>
    public static class HorizontalInputResolver
    {
        /// <summary>
        /// 방향키 Press 하나가 대시를 일으키는지 판정하고, 일으키면 대시 방향(Left 또는 Right)을 돌려준다.
        /// 대시가 아니면 None이다.
        /// isDoubleTapRecognized는 같은 Press가 DoubleTapTracker에서 더블탭으로 인식됐는지이다.
        /// 이동 잠금은 대시를 막지 않는다. 대시가 잠금 행동을 캔슬하며, 허용 여부는 IsDashAllowed가 정한다.
        /// </summary>
        public static HorizontalDirection ResolveDash(
            LogicalKey key,
            bool isDoubleTapRecognized,
            in MovementInputContext context)
        {
            if (!isDoubleTapRecognized)
            {
                return HorizontalDirection.None;
            }

            // ↓↓도 더블탭으로 인식되지만 대시가 아니다(하강은 VerticalInputResolver 담당).
            HorizontalDirection direction = ToDirection(key);
            if (direction == HorizontalDirection.None)
            {
                return HorizontalDirection.None;
            }

            // 대시 진행 중에는 호출자가 더블탭 이력을 갱신하지 않으므로 보통 여기까지 오지 않는다. 방어용 확인이다.
            if (context.IsDashing)
            {
                return HorizontalDirection.None;
            }

            // 공중 대시는 없다. 쿨다운 중이거나 현재 공격 상태가 허용하지 않아도 실행하지 않는다.
            if (!context.IsGrounded || !context.IsDashAllowed || context.IsDashOnCooldown)
            {
                return HorizontalDirection.None;
            }

            return direction;
        }

        private static HorizontalDirection ToDirection(LogicalKey key)
        {
            if (key == LogicalKey.Left)
            {
                return HorizontalDirection.Left;
            }

            if (key == LogicalKey.Right)
            {
                return HorizontalDirection.Right;
            }

            return HorizontalDirection.None;
        }
    }
}
