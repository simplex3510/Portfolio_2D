namespace RuneBlade.Simulation
{
    /// <summary>
    /// 수직 입력(↑ 점프·이단 점프, ↓↓ 하강) 판정. 상태는 직접 갖지 않고, 점프 버퍼만 인자로 받아 갱신한다.
    /// 실행하지 못한 입력은 다른 동작으로 대체하지 않는다(폴백 없음). 막힌 점프는 저장 또는 소실, 막힌 하강은 폐기한다.
    /// </summary>
    public static class VerticalInputResolver
    {
        /// <summary>
        /// 방향키 Press 하나를 수직 동작으로 판정한다. ↑, ↓ 외의 키는 None이다.
        /// isDoubleTapRecognized는 같은 Press가 DoubleTapTracker에서 더블탭으로 인식됐는지이다(↓에만 쓴다).
        /// </summary>
        public static VerticalCommand Resolve(
            LogicalKey key,
            bool isDoubleTapRecognized,
            in MovementInputContext context,
            in MovementInputTuning tuning,
            ref JumpInputBuffer jumpBuffer)
        {
            if (key == LogicalKey.Up)
            {
                return ResolveJump(context, tuning, ref jumpBuffer);
            }

            if (key == LogicalKey.Down)
            {
                return ResolveDropThrough(isDoubleTapRecognized, context);
            }

            return VerticalCommand.None;
        }

        private static VerticalCommand ResolveJump(
            in MovementInputContext context,
            in MovementInputTuning tuning,
            ref JumpInputBuffer jumpBuffer)
        {
            // 이동 잠금 중이거나 대시 중인 점프는 저장하지 않고 소실한다(착지용 버퍼에도 넣지 않는다).
            if (context.IsMovementLocked || context.IsDashing)
            {
                return VerticalCommand.None;
            }

            if (context.IsGrounded)
            {
                return VerticalCommand.Jump;
            }

            if (context.CanDoubleJump)
            {
                return VerticalCommand.DoubleJump;
            }

            // 공중이고 이단 점프도 불가하면 착지 시 소비할 선입력으로 저장한다.
            jumpBuffer.Store(tuning.JumpBufferTicks);
            return VerticalCommand.None;
        }

        private static VerticalCommand ResolveDropThrough(bool isDoubleTapRecognized, in MovementInputContext context)
        {
            if (!isDoubleTapRecognized)
            {
                return VerticalCommand.None;
            }

            // TODO: 하강은 서 있는 바닥이 통과 가능한 플랫폼일 때만 가능하다. 플랫폼 정보가 생기면 조건에 추가한다.
            if (context.IsMovementLocked || !context.IsGrounded)
            {
                return VerticalCommand.None;
            }

            return VerticalCommand.DropThrough;
        }
    }
}
