namespace RuneBlade.Simulation
{
    /// <summary>
    /// 플레이어 한 명의 이동 상태와 규칙. 위치·속도·지상/대시 상태와 이동 입력 이력을 함께 소유하고,
    /// 한 틱 입력(TickInput)을 받아 스스로 갱신한다. 서버 스냅샷·클라이언트 보정의 대상이 되는 상태이다.
    ///
    /// 조정 수치(MovementSettings)는 매치 시작 시 고정된 값을 밖에서 넘겨받는다(D-008).
    /// 좌표계는 월드 유닛 float이며 +X가 오른쪽, +Y가 위쪽이다(D-012).
    ///
    /// TODO: 공격 상태가 생기면 대시 허용(접점 1)과 이동 잠금(접점 3)을 공격 도메인에서 받는다.
    ///       지금은 공격이 없으므로 대시는 항상 허용되고 이동은 잠기지 않는다.
    /// TODO: 바닥은 Y=0 하나뿐이다. 플랫폼과 하강(↓↓)은 플랫폼 정보가 생기면 구현한다.
    /// TODO: 바라보는 방향(Facing)은 공격·렌더가 필요해질 때 추가한다.
    /// </summary>
    public struct PlayerMovement
    {
        // 임시 바닥 높이. 플랫폼 정보가 생기면 대체된다.
        private const float GroundY = 0f;

        private float _positionX;
        private float _positionY;
        private float _velocityY;

        private bool _isGrounded;
        private bool _hasUsedDoubleJump; // 공중에서 이단 점프를 이미 썼는지(착지하면 초기화)

        private HorizontalDirection _dashDirection; // 대시 중이 아닐 때는 항상 None
        private int _dashRemainingTicks;            // 0보다 크면 대시 진행 중
        private int _dashCooldownRemainingTicks;    // 0보다 크면 대시 불가

        // 이동 입력 상태. 눌림 상태는 공격 도메인과 공유하게 될 값이라 공격이 생기면 이 구조체 밖으로 올릴 수 있다.
        private InputState _inputState;
        private DoubleTapTracker _doubleTapTracker;
        private JumpInputBuffer _jumpBuffer;

        public readonly float PositionX => _positionX;

        public readonly float PositionY => _positionY;

        public readonly float VelocityY => _velocityY;

        public readonly bool IsGrounded => _isGrounded;

        public readonly bool IsDashing => _dashRemainingTicks > 0;

        public readonly HorizontalDirection DashDirection => _dashDirection;

        /// <summary>바닥 위에 선 상태로 시작한다. default는 공중 상태이므로 시작 시에는 이 메서드를 쓴다.</summary>
        public static PlayerMovement CreateOnGround(float startPositionX)
        {
            PlayerMovement movement = default;
            movement._positionX = startPositionX;
            movement._positionY = GroundY;
            movement._isGrounded = true;
            return movement;
        }

        /// <summary>
        /// 한 틱을 진행한다. 순서: (1) 시간 경과(더블탭 창, 점프 버퍼) (2) 이벤트를 도착 순서대로 처리
        /// (3) 이번 틱의 이동·중력 적용. 같은 틱 안에서 앞 이벤트의 결과는 뒤 이벤트 판정에 즉시 반영된다.
        /// </summary>
        public void Tick(in TickInput input, in MovementSettings settings)
        {
            _doubleTapTracker.CountDownTick();
            _jumpBuffer.CountDownTick();

            for (int i = 0; i < input.EventCount; i++)
            {
                KeyInputEvent keyInputEvent = input.GetEventAt(i);

                // 눌림 상태와 모순되는 이벤트(이미 눌린 키의 Press 등)는 그 이벤트가 일으킬 동작을 없던 것으로 한다.
                if (!_inputState.TryApplyKeyEvent(keyInputEvent))
                {
                    continue;
                }

                ProcessKeyEvent(keyInputEvent, settings);
            }

            StepHorizontal(settings);
            StepVertical(settings);
        }

        // 방향키 Press만 이동 규칙을 일으킨다. 좌우 이동 자체는 이벤트가 아니라 InputState.Horizontal에서 도출한다.
        private void ProcessKeyEvent(KeyInputEvent keyInputEvent, in MovementSettings settings)
        {
            LogicalKey key = keyInputEvent.Key;
            if (!keyInputEvent.IsPress || !LogicalKeyUtility.IsDirection(key))
            {
                return;
            }

            // 대시 진행 중의 누름은 더블탭 이력을 새로 만들지도, 기존 이력을 무효화하지도 않는다.
            bool isDoubleTapRecognized = false;
            if (!IsDashing)
            {
                isDoubleTapRecognized = _doubleTapTracker.RegisterPress(key, settings.DoubleTapWindowTicks);
            }

            switch (key)
            {
                case LogicalKey.Left:
                case LogicalKey.Right:
                    if (isDoubleTapRecognized)
                    {
                        TryStartDash(key, settings);
                    }
                    break;

                case LogicalKey.Up:
                    ProcessJumpPress(settings);
                    break;

                case LogicalKey.Down:
                    // TODO: ↓↓ 하강(isDoubleTapRecognized). 플랫폼 정보가 생기면 구현한다.
                    break;
            }
        }

        private void TryStartDash(LogicalKey key, in MovementSettings settings)
        {
            // 공중 대시는 없다. 쿨다운 중이면 실행하지 않고 폐기한다(다른 동작으로 대체하지 않음).
            if (!_isGrounded || _dashCooldownRemainingTicks > 0)
            {
                return;
            }

            _dashDirection = key == LogicalKey.Left ? HorizontalDirection.Left : HorizontalDirection.Right;
            _dashRemainingTicks = settings.DashDurationTicks;
        }

        private void ProcessJumpPress(in MovementSettings settings)
        {
            // 대시 중의 점프는 저장하지 않고 소실한다(점프 선입력 버퍼에도 넣지 않는다).
            if (IsDashing)
            {
                return;
            }

            if (_isGrounded)
            {
                StartJump(settings);
            }
            else if (!_hasUsedDoubleJump)
            {
                _velocityY = settings.JumpVelocityPerTick;
                _hasUsedDoubleJump = true;
            }
            else
            {
                // 공중이고 이단 점프도 썼다면 착지 시 소비할 선입력으로 저장한다.
                _jumpBuffer.Store(settings.JumpBufferTicks);
            }
        }

        private void StartJump(in MovementSettings settings)
        {
            _velocityY = settings.JumpVelocityPerTick;
            _isGrounded = false;
        }

        private void StepHorizontal(in MovementSettings settings)
        {
            // 쿨다운은 틱 끝에서 줄인다. 대시가 끝난 틱에는 아래에서 새로 설정하므로 쿨다운 틱 수만큼 막힌다.
            if (_dashCooldownRemainingTicks > 0)
            {
                _dashCooldownRemainingTicks--;
            }

            if (IsDashing)
            {
                // 대시 중에는 방향 전환 없이 시작 방향으로만 이동한다.
                _positionX += settings.DashSpeedPerTick * ToSign(_dashDirection);
                _dashRemainingTicks--;

                if (_dashRemainingTicks == 0)
                {
                    _dashDirection = HorizontalDirection.None;
                    _dashCooldownRemainingTicks = settings.DashCooldownTicks;
                }

                return;
            }

            // TODO: 이동 잠금 중에는 이동하지 않는다.
            _positionX += settings.MoveSpeedPerTick * ToSign(_inputState.Horizontal);
        }

        private void StepVertical(in MovementSettings settings)
        {
            if (_isGrounded)
            {
                return;
            }

            // 속도를 먼저 갱신한 뒤 위치를 옮긴다.
            _velocityY -= settings.GravityPerTick;
            _positionY += _velocityY;

            if (_positionY <= GroundY)
            {
                Land(settings);
            }
        }

        private void Land(in MovementSettings settings)
        {
            _positionY = GroundY;
            _velocityY = 0f;
            _isGrounded = true;
            _hasUsedDoubleJump = false;

            // 공중에서 눌러 둔 점프가 있으면 착지하자마자 점프한다.
            if (_jumpBuffer.TryConsume())
            {
                StartJump(settings);
            }
        }

        private static int ToSign(HorizontalDirection direction)
        {
            switch (direction)
            {
                case HorizontalDirection.Left:
                    return -1;
                case HorizontalDirection.Right:
                    return 1;
                default:
                    return 0;
            }
        }

        public override readonly string ToString()
        {
            return $"Pos=({_positionX}, {_positionY}), VelY={_velocityY}, Grounded={_isGrounded}, "
                + $"Dash={_dashDirection}({_dashRemainingTicks}), Cooldown={_dashCooldownRemainingTicks}";
        }
    }
}
