using System;

namespace RuneBlade.Network
{
    /// <summary>
    /// UnreliableTransport가 한 방향에 적용할 불량 조건(D-008). 시간 단위는 시뮬레이션 틱이다(D-007, 60Hz).
    /// 순서 뒤바뀜은 별도 값이 없다. jitterTicks 때문에 나중에 보낸 메시지가 먼저 도착하는 경우로 생긴다.
    /// </summary>
    [Serializable]
    public struct NetworkConditions
    {
        /// <summary>모든 메시지에 더해지는 기본 지연(틱). 0이면 Send한 틱에 바로 도착한다.</summary>
        public int latencyTicks;

        /// <summary>메시지마다 0 ~ jitterTicks 사이에서 무작위로 더해지는 추가 지연(틱).</summary>
        public int jitterTicks;

        /// <summary>메시지를 버릴 확률(0 ~ 1).</summary>
        public float lossRate;

        // TODO(tuning): 기본값은 근거가 약한 제안값이다. 6틱 = 100ms, 지터 3틱 = 50ms, 유실 5%.
        public static NetworkConditions Default => new NetworkConditions
        {
            latencyTicks = 6,
            jitterTicks = 3,
            lossRate = 0.05f,
        };
    }
}
