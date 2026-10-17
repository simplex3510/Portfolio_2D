using System;
using System.Collections.Generic;

using NUnit.Framework;

using RuneBlade.Simulation;

namespace RuneBlade.Network.Tests
{
    public class UnreliableTransportTests
    {
        private static NetworkConditions Conditions(int latencyTicks, int jitterTicks = 0, float lossRate = 0f)
        {
            return new NetworkConditions { latencyTicks = latencyTicks, jitterTicks = jitterTicks, lossRate = lossRate };
        }

        private static InputMessage MessageOf(int firstTick)
        {
            var tickInput = new TickInput();
            Assert.IsTrue(tickInput.TryAdd(new KeyInputEvent(LogicalKey.Left, KeyTransition.Press)));
            return new InputMessage(firstTick, new[] { tickInput });
        }

        private static SnapshotMessage SnapshotOf(int lastProcessedInputTick)
        {
            return new SnapshotMessage(lastProcessedInputTick, default);
        }

        // 틱마다 메시지를 하나씩 보내고 Tick()을 돌리며, 서버에 도착한 FirstTick을 도착 순서대로 모은다.
        private static List<int> DeliveredFirstTicks(NetworkConditions conditions, int seed)
        {
            var transport = new UnreliableTransport(1, conditions, seed);
            IClientTransport client = transport.CreateClient();
            var delivered = new List<int>();

            for (int tick = 0; tick < 40; tick++)
            {
                client.Send(MessageOf(tick));
                transport.Tick();
                while (transport.Server.TryReceive(out _, out InputMessage message))
                {
                    delivered.Add(message.FirstTick);
                }
            }

            return delivered;
        }

        [Test]
        public void NoConditions_DeliversImmediately()
        {
            var transport = new UnreliableTransport(1, Conditions(0), 1);
            IClientTransport client = transport.CreateClient();

            client.Send(MessageOf(5));
            transport.Server.Send(0, SnapshotOf(7));

            Assert.IsTrue(transport.Server.TryReceive(out int clientId, out InputMessage input));
            Assert.That(clientId, Is.EqualTo(0));
            Assert.That(input.FirstTick, Is.EqualTo(5));
            Assert.IsTrue(client.TryReceive(out SnapshotMessage snapshot));
            Assert.That(snapshot.LastProcessedInputTick, Is.EqualTo(7));
        }

        [Test]
        public void Latency_DeliversOnlyAfterEnoughTicks()
        {
            var transport = new UnreliableTransport(1, Conditions(3), 1);
            IClientTransport client = transport.CreateClient();

            client.Send(MessageOf(0));
            transport.Tick();
            transport.Tick();
            Assert.IsFalse(transport.Server.TryReceive(out _, out _));

            transport.Tick();
            Assert.IsTrue(transport.Server.TryReceive(out _, out _));
        }

        [Test]
        public void Latency_AppliesToServerToClientDirection()
        {
            var transport = new UnreliableTransport(1, Conditions(2), 1);
            IClientTransport client = transport.CreateClient();

            transport.Server.Send(0, SnapshotOf(1));
            transport.Tick();
            Assert.IsFalse(client.TryReceive(out _));

            transport.Tick();
            Assert.IsTrue(client.TryReceive(out _));
        }

        [Test]
        public void FullLoss_DropsEverything()
        {
            var transport = new UnreliableTransport(1, Conditions(0, 0, 1f), 1);
            IClientTransport client = transport.CreateClient();

            client.Send(MessageOf(0));
            transport.Server.Send(0, SnapshotOf(0));
            transport.Tick();

            Assert.IsFalse(transport.Server.TryReceive(out _, out _));
            Assert.IsFalse(client.TryReceive(out _));
        }

        [Test]
        public void Jitter_ReordersMessages()
        {
            // 지연 1~6틱, 매 틱 한 개씩 40개를 보내면 도착 틱 역전이 생기지 않을 확률은 사실상 없다.
            List<int> delivered = DeliveredFirstTicks(Conditions(1, 5), 1);

            Assert.That(delivered, Is.Not.Ordered);
        }

        [Test]
        public void NoJitter_KeepsSendOrder()
        {
            List<int> delivered = DeliveredFirstTicks(Conditions(3), 1);

            Assert.That(delivered, Is.Ordered);
        }

        [Test]
        public void SameSeed_ReproducesSameDeliveries()
        {
            NetworkConditions conditions = Conditions(2, 4, 0.3f);

            Assert.That(DeliveredFirstTicks(conditions, 42), Is.EqualTo(DeliveredFirstTicks(conditions, 42)));
        }

        [Test]
        public void InvalidConditions_Throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new UnreliableTransport(1, Conditions(-1), 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new UnreliableTransport(1, Conditions(0, 0, 1.5f), 1));
        }
    }
}
