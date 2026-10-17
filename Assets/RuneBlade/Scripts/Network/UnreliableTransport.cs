using System;
using System.Collections.Generic;

namespace RuneBlade.Network
{
    /// <summary>
    /// InMemoryTransport를 상속해 지연·유실·순서 뒤바뀜을 일부러 얹는 테스트용 가짜 불량 전송(N3).
    /// 큐에 넣는 지점(EnqueueToServer/EnqueueToClient)만 override하므로 핸들과 수신 쪽은 부모 그대로이다.
    /// 사용하는 쪽은 변수 타입을 InMemoryTransport로 두고 구현만 갈아 끼우면 된다.
    ///
    /// 시간은 Tick() 호출 횟수이다. 틱 루프가 매 틱 한 번 호출해야 지연된 메시지가 도착한다.
    /// 난수는 seed로 고정되므로 같은 seed와 같은 호출 순서면 같은 불량이 재현된다.
    /// </summary>
    public sealed class UnreliableTransport : InMemoryTransport
    {
        private readonly Random _randomFactor;
        private readonly NetworkConditions _toServerConditions;
        private readonly NetworkConditions _toClientConditions;

        // 도착 틱 오름차순, 같은 틱이면 보낸 순서로 유지한다.
        private readonly List<(int deliverAtTick, int clientId, InputMessage message)> _pendingToServer = new();
        private readonly List<(int deliverAtTick, int clientId, SnapshotMessage message)> _pendingToClient = new();

        private int _currentTick;

        public UnreliableTransport(int clientCount, NetworkConditions conditions, int seed)
            : this(clientCount, conditions, conditions, seed)
        {
        }

        public UnreliableTransport(int clientCount, NetworkConditions serverCondition, NetworkConditions clientCondition, int seed)
            : base(clientCount)
        {
            Validate(serverCondition, nameof(serverCondition));
            Validate(clientCondition, nameof(clientCondition));

            _randomFactor = new Random(seed);
            _toServerConditions = serverCondition;
            _toClientConditions = clientCondition;
        }

        /// <summary>
        /// 틱을 하나 진행하고 도착 틱이 된 메시지를 수신 큐로 넘긴다.
        /// 틱 루프가 매 틱 한 번 호출한다.
        /// </summary>
        public void Tick()
        {
            _currentTick++;
            Flush(_pendingToServer, base.EnqueueToServer);
            Flush(_pendingToClient, base.EnqueueToClient);
        }

        protected override void EnqueueToServer(int clientId, InputMessage message)
        {
            Schedule(_pendingToServer, _toServerConditions, clientId, message, base.EnqueueToServer);
        }

        protected override void EnqueueToClient(int clientId, SnapshotMessage message)
        {
            Schedule(_pendingToClient, _toClientConditions, clientId, message, base.EnqueueToClient);
        }

        private void Schedule<T>(
            List<(int deliverAtTick, int clientId, T message)> pending,
            NetworkConditions conditions,
            int clientId,
            T message,
            Action<int, T> deliverNow)
        {
            // 유실 확률에 따라 메시지를 버린다. 버리면 deliverNow를 호출하지 않고 그냥 리턴한다.
            if (_randomFactor.NextDouble() < conditions.lossRate)
            {
                return;
            }

            // 레이턴시와 지터를 더해 딜레이를 계산한다.
            int delayTicks = conditions.latencyTicks + _randomFactor.Next(conditions.jitterTicks + 1);
            if (delayTicks == 0)
            {
                deliverNow(clientId, message);
                return;
            }

            // 딜레이로 인해 선행 메시지보다 후행 메시지가 일찍 도착하는 경우가 순서 뒤바뀜이다.
            // 도착 틱 순서가 유지되는 위치에 끼워 넣는다.
            int deliverAtTick = _currentTick + delayTicks;
            int insertIndex = pending.Count;
            while (insertIndex > 0 && pending[insertIndex - 1].deliverAtTick > deliverAtTick)
            {
                insertIndex--;
            }

            pending.Insert(insertIndex, (deliverAtTick, clientId, message));
        }

        private void Flush<T>(
            List<(int deliverAtTick, int clientId, T message)> pending, 
            Action<int, T> deliver)
        {
            int deliveredCount = 0;
            while (deliveredCount < pending.Count && pending[deliveredCount].deliverAtTick <= _currentTick)
            {
                deliver(pending[deliveredCount].clientId, pending[deliveredCount].message);
                deliveredCount++;
            }

            pending.RemoveRange(0, deliveredCount);
        }

        private static void Validate(NetworkConditions conditions, string parameterName)
        {
            if (conditions.latencyTicks < 0 || conditions.jitterTicks < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "지연 틱은 0 이상이어야 합니다.");
            }

            if (conditions.lossRate < 0f || conditions.lossRate > 1f)
            {
                throw new ArgumentOutOfRangeException(parameterName, "lossRate는 0 ~ 1이어야 합니다.");
            }
        }
    }
}
