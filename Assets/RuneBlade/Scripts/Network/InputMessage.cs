using System;

using RuneBlade.Simulation;

namespace RuneBlade.Network
{
    /// <summary>
    /// 클라이언트 → 서버 입력 메시지. 연속된 틱의 입력을 한 번에 담는다.
    /// Inputs[i]는 FirstTick + i번째 틱의 입력이며, 마지막 원소가 가장 최신 틱이다.
    /// 앞쪽 원소는 이전 메시지가 유실됐을 때를 대비한 중복 전송이다(몇 틱을 담을지는 보내는 쪽이 정한다).
    /// 같은 틱이 여러 메시지에 실려 올 수 있으므로 받는 쪽이 틱 번호로 중복을 걸러야 한다.
    ///
    /// shortcut: Inputs 배열을 복사하지 않고 받은 참조를 그대로 쓴다(보내는 쪽이 이후 배열을 고치면 안 된다).
    ///           직렬화로 바이트를 거치는 실제 전송으로 바꿀 때 함께 정리한다.
    /// </summary>
    public readonly struct InputMessage
    {
        public readonly int FirstTick;
        public readonly TickInput[] tickInputs;

        public InputMessage(int firstTick, TickInput[] tickInputs)
        {
            if (tickInputs == null || tickInputs.Length == 0)
            {
                throw new ArgumentException("입력이 하나 이상 있어야 합니다.", nameof(tickInputs));
            }

            FirstTick = firstTick;
            this.tickInputs = tickInputs;
        }

        /// <summary>이 메시지가 담은 가장 최신 틱 번호.</summary>
        public int LastTick => FirstTick + tickInputs.Length - 1;

        /// <summary>틱 번호로 입력을 읽는다. 이 메시지가 담은 범위(FirstTick ~ LastTick) 밖이면 false이다.</summary>
        public bool TryGetTickInput(int tickNumber, out TickInput tickInput)
        {
            if (tickNumber < FirstTick || tickNumber > LastTick)
            {
                tickInput = default;
                return false;
            }

            tickInput = tickInputs[tickNumber - FirstTick];
            return true;
        }
    }
}
