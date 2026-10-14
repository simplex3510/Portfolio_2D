namespace RuneBlade.Simulation
{
    /// <summary>
    /// 이동 입력 판정에 필요한 외부 상태. 읽기 전용이며 호출하는 쪽(캐릭터·공격 상태)이 채운다.
    /// 입력 계층은 이 값의 출처를 모르고, 값을 바꾸지도 않는다.
    /// 필드 구성은 designs/101_처리구조.md, 102_이동입력.md에서 도출한 최소 집합이며
    /// 처리기 구현 중에 추가·삭제될 수 있다.
    /// </summary>
    public readonly struct MovementInputContext
    {
        public readonly bool IsGrounded;

        /// <summary>이동 잠금(공격 선딜·Active·후딜, Charge, Channel). 잠금 중에도 더블탭 인식은 계속된다.</summary>
        public readonly bool IsMovementLocked;

        /// <summary>현재 공격 상태가 정하는 대시 허용 값(접점 1).</summary>
        public readonly bool IsDashAllowed;

        // TODO: 쿨다운의 실제 구조가 정해지면 bool 대신 남은 틱 등으로 바꾼다.
        public readonly bool IsDashOnCooldown;

        /// <summary>대시 진행 중. 이 동안은 더블탭 이력에 누름을 반영하지 않는다.</summary>
        public readonly bool IsDashing;

        /// <summary>공중에서 이단 점프가 가능한지(아직 쓰지 않았는지).</summary>
        public readonly bool CanDoubleJump;

        public MovementInputContext(
            bool isGrounded,
            bool isMovementLocked,
            bool isDashAllowed,
            bool isDashOnCooldown,
            bool isDashing,
            bool canDoubleJump)
        {
            IsGrounded = isGrounded;
            IsMovementLocked = isMovementLocked;
            IsDashAllowed = isDashAllowed;
            IsDashOnCooldown = isDashOnCooldown;
            IsDashing = isDashing;
            CanDoubleJump = canDoubleJump;
        }
    }
}
