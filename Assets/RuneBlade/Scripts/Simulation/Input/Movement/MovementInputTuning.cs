using System;

namespace RuneBlade.Simulation
{
    /// <summary>
    /// 이동 입력의 조정 수치 모음. 체감을 보며 조정할 값이라 Inspector에서 고칠 수 있어야 한다.
    ///
    /// Simulation은 UnityEngine을 참조하지 않으므로 [SerializeField]를 쓸 수 없다. 그래서 순수 C# [Serializable]
    /// struct에 public 비readonly 필드로 두고, Unity 쪽 ScriptableObject가 이 struct를 그대로 담는다.
    /// 시뮬레이션은 읽기만 하며, 매치 진행 중에 값이 바뀌면 결정성이 깨지므로 매치 시작 시 복사해서 고정한다.
    /// 시간 값은 전부 틱 단위 정수이다(60Hz, D-007). float를 쓰지 않는다.
    /// TODO: 양 클라이언트의 값 일치를 매치 시작 시 보장하는 방식은 미결(Open).
    /// </summary>
    [Serializable]
    public struct MovementInputTuning
    {
        /// <summary>점프 선입력을 유지하는 틱 수. 0이면 저장한 그 틱까지만 유지한다.</summary>
        public int JumpBufferTicks; // TODO(tuning)

        /// <summary>더블탭 창. 첫 방향키 Press 후 이 틱 수 이내의 같은 방향 Press까지 더블탭으로 인식한다.</summary>
        public int DoubleTapWindowTicks; // TODO(tuning)

        // Inspector에서 새로 만든 에셋의 기본값으로 쓴다(struct 필드 초기화 구문은 직렬화에 쓸 수 없다).
        public static MovementInputTuning CreateDefault()
        {
            return new MovementInputTuning
            {
                JumpBufferTicks = 6, // TODO(tuning) 60Hz 기준 0.1초. 체감을 보고 조정한다.
                DoubleTapWindowTicks = 15, // TODO(tuning) 60Hz 기준 0.25초. 체감을 보고 조정한다.
            };
        }
    }
}
