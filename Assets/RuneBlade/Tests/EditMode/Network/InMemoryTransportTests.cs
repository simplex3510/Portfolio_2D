using System;

using NUnit.Framework;

using RuneBlade.Simulation;

namespace RuneBlade.Network.Tests
{
    public class InMemoryTransportTests
    {
        // ---- 테스트 도우미 ----

        // 키 이벤트 하나짜리 입력. 키가 다르면 RawEvents도 달라서 어느 입력인지 구분할 수 있다.
        private static TickInput PressOf(LogicalKey key)
        {
            var tickInput = new TickInput();
            Assert.IsTrue(tickInput.TryAdd(new KeyInputEvent(key, KeyTransition.Press)));
            return tickInput;
        }

        private static InputMessage MessageOf(int firstTick, params TickInput[] tickInputs)
        {
            return new InputMessage(firstTick, tickInputs);
        }

        // ---- 클라이언트 → 서버 ----

        [Test]
        public void ClientToServer_DeliversWithClientIdAndSameValues()
        {
            var transport = new InMemoryTransport(2);
            IClientTransport client = transport.CreateClient();

            client.Send(MessageOf(10, PressOf(LogicalKey.Left), PressOf(LogicalKey.Right), PressOf(LogicalKey.Up)));

            Assert.IsTrue(transport.Server.TryReceive(out int clientId, out InputMessage received));
            Assert.That(clientId, Is.EqualTo(0));
            Assert.That(received.FirstTick, Is.EqualTo(10));
            Assert.That(received.LastTick, Is.EqualTo(12));
            Assert.That(received.tickInputs[0].RawEvents, Is.EqualTo(PressOf(LogicalKey.Left).RawEvents));
            Assert.That(received.tickInputs[2].RawEvents, Is.EqualTo(PressOf(LogicalKey.Up).RawEvents));
        }

        [Test]
        public void ServerReceive_WhenQueueIsEmpty_ReturnsFalse()
        {
            var transport = new InMemoryTransport(2);
            IClientTransport client = transport.CreateClient();
            client.Send(MessageOf(0, PressOf(LogicalKey.Left)));
            transport.Server.TryReceive(out _, out _);

            Assert.IsFalse(transport.Server.TryReceive(out _, out _));
        }

        [Test]
        public void ServerReceive_KeepsSendOrderAcrossClients()
        {
            var transport = new InMemoryTransport(2);
            IClientTransport clientA = transport.CreateClient();
            IClientTransport clientB = transport.CreateClient();

            clientA.Send(MessageOf(1, PressOf(LogicalKey.Left)));
            clientB.Send(MessageOf(2, PressOf(LogicalKey.Right)));

            Assert.IsTrue(transport.Server.TryReceive(out int firstClientId, out InputMessage first));
            Assert.IsTrue(transport.Server.TryReceive(out int secondClientId, out InputMessage second));
            Assert.That(firstClientId, Is.EqualTo(0));
            Assert.That(first.FirstTick, Is.EqualTo(1));
            Assert.That(secondClientId, Is.EqualTo(1));
            Assert.That(second.FirstTick, Is.EqualTo(2));
        }

        [Test]
        public void Send_CopiesInputs_SoLaterChangesToTheSenderArrayDoNotLeak()
        {
            var transport = new InMemoryTransport(2);
            IClientTransport client = transport.CreateClient();
            TickInput[] senderArray = { PressOf(LogicalKey.Left), PressOf(LogicalKey.Right) };

            client.Send(new InputMessage(5, senderArray));
            senderArray[0] = PressOf(LogicalKey.Up); // 보낸 뒤 보내는 쪽이 배열을 재사용한 상황

            transport.Server.TryReceive(out _, out InputMessage received);
            Assert.That(received.tickInputs[0].RawEvents, Is.EqualTo(PressOf(LogicalKey.Left).RawEvents));
        }

        // ---- 서버 → 클라이언트 ----

        [Test]
        public void ServerToClient_DeliversOnlyToTheTargetClient()
        {
            var transport = new InMemoryTransport(2);
            IClientTransport clientA = transport.CreateClient();
            IClientTransport clientB = transport.CreateClient();
            MovementSimulation state = MovementSimulation.CreateOnGround(1.5f);

            transport.Server.Send(0, new SnapshotMessage(7, state));

            Assert.IsTrue(clientA.TryReceive(out SnapshotMessage received));
            Assert.That(received.LastProcessedInputTick, Is.EqualTo(7));
            Assert.That(received.State.PositionX, Is.EqualTo(1.5f));
            Assert.IsFalse(clientB.TryReceive(out _));
        }

        // ---- 오류 ----

        [Test]
        public void ServerSend_ToUnknownClient_Throws()
        {
            var transport = new InMemoryTransport(2);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => transport.Server.Send(2, new SnapshotMessage(0, default)));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => transport.Server.Send(-1, new SnapshotMessage(0, default)));
        }

        [Test]
        public void CreateClient_BeyondCapacity_Throws()
        {
            var transport = new InMemoryTransport(2);
            transport.CreateClient();
            transport.CreateClient();

            Assert.Throws<InvalidOperationException>(() => transport.CreateClient());
        }

        [Test]
        public void Constructor_WithNonPositiveClientCount_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new InMemoryTransport(0));
        }
    }
}
