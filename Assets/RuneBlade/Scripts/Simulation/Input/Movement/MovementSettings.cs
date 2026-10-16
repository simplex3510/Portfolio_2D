using System;

namespace RuneBlade.Simulation
{
    /// <summary>
    /// 이동의 조정 수치 모음(입력 판정 시간 + 물리 값). 체감을 보며 조정할 값이라 Inspector에서 고칠 수 있어야 한다.
    ///
    /// Simulation은 UnityEngine을 참조하지 않으므로 [SerializeField]를 쓸 수 없다. 그래서 순수 C# [Serializable]
    /// struct에 public 비readonly 필드로 두고, Unity 쪽 ScriptableObject가 이 struct를 그대로 담는다(D-008).
    /// 시뮬레이션은 읽기만 하며, 매치 진행 중에 값이 바뀌면 예측과 서버 결과가 어긋나므로 매치 시작 시 복사해서 고정한다.
    /// 시간은 틱 정수, 거리·속도·가속도는 월드 유닛 기준 float(유닛/초, 유닛/초²)로 저장하고,
    /// 시뮬레이션에서 쓰는 틱당 값은 아래 ...PerTick 속성이 만든다(D-012).
    /// TODO: 서버와 클라이언트가 같은 값을 쓰게 하는 방식은 미결(Open).
    /// </summary>
    [Serializable]
    public struct MovementSettings
    {
        // shortcut: 틱 속도(D-007)를 여기서 상수로 둔다. 두 번째 사용처가 생기면 공용 위치로 옮긴다.
        private const int TicksPerSecond = 60;

        // ---- 입력 판정 (틱) ----

        /// <summary>점프 선입력을 유지하는 틱 수. 0이면 저장한 그 틱까지만 유지한다.</summary>
        public int JumpBufferTicks; // TODO(tuning)

        /// <summary>더블탭 창. 첫 방향키 Press 후 이 틱 수 이내의 같은 방향 Press까지 더블탭으로 인식한다.</summary>
        public int DoubleTapWindowTicks; // TODO(tuning)

        // ---- 이동 물리 (거리 = 월드 유닛, 시간 = 초) ----

        /// <summary>좌우 이동 속도. 유닛/초.</summary>
        public float MoveSpeed; // TODO(tuning)

        /// <summary>점프 시작 시 위쪽 속도(이단 점프도 같은 값). 유닛/초.</summary>
        public float JumpVelocity; // TODO(tuning)

        /// <summary>중력 가속도(아래 방향 크기). 유닛/초².</summary>
        public float Gravity; // TODO(tuning)

        /// <summary>대시 중 좌우 속도. 유닛/초.</summary>
        public float DashSpeed; // TODO(tuning)

        /// <summary>대시가 지속되는 틱 수.</summary>
        public int DashDurationTicks; // TODO(tuning)

        /// <summary>대시가 끝난 뒤 다시 쓸 수 있을 때까지의 틱 수.</summary>
        public int DashCooldownTicks; // TODO(tuning)

        /// <summary>틱당 좌우 이동 거리(월드 유닛).</summary>
        public readonly float MoveSpeedPerTick => MoveSpeed / TicksPerSecond;

        /// <summary>점프 시작 시 틱당 위쪽 이동 거리(월드 유닛).</summary>
        public readonly float JumpVelocityPerTick => JumpVelocity / TicksPerSecond;

        /// <summary>틱마다 아래 방향 속도에 더하는 값(월드 유닛/틱²).</summary>
        public readonly float GravityPerTick => Gravity / (TicksPerSecond * TicksPerSecond);

        /// <summary>대시 중 틱당 좌우 이동 거리(월드 유닛).</summary>
        public readonly float DashSpeedPerTick => DashSpeed / TicksPerSecond;

        // Inspector에서 새로 만든 에셋의 기본값으로 쓴다(struct 필드 초기화 구문은 직렬화에 쓸 수 없다).
        // 아래 값은 모두 시험하지 않은 제안값이며, 월드 유닛 크기(캐릭터 키 약 2유닛)를 가정했다.
        public static MovementSettings CreateDefault()
        {
            return new MovementSettings
            {
                JumpBufferTicks = 6,        // TODO(tuning) 0.1초
                DoubleTapWindowTicks = 15,  // TODO(tuning) 0.25초
                MoveSpeed = 6f,             // TODO(tuning) 6유닛/초
                JumpVelocity = 12f,         // TODO(tuning) 12유닛/초. 중력 30과 함께 점프 높이 약 2.4유닛
                Gravity = 30f,              // TODO(tuning) 30유닛/초²
                DashSpeed = 14f,            // TODO(tuning) 14유닛/초. 12틱 지속 시 약 2.8유닛 이동
                DashDurationTicks = 12,     // TODO(tuning) 0.2초
                DashCooldownTicks = 30,     // TODO(tuning) 0.5초
            };
        }
    }
}
