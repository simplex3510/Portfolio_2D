using System;
using System.Collections.Generic;

using RuneBlade.Simulation;

namespace RuneBlade.Network
{
    /// <summary>
    /// 같은 프로세스 안에서 서버와 클라이언트가 메시지를 주고받는 가짜 전송(D-014).
    /// 이 객체는 큐를 소유하고, 서버용·클라이언트용 핸들을 따로 내어준다.
    /// 핸들이 서로 다른 인터페이스만 구현하므로 클라이언트가 서버 쪽 메서드를 부르는 실수는 컴파일 단계에서 막힌다.
    ///
    /// 메시지는 Send 즉시 상대 큐에 들어가고 TryReceive로 꺼낼 때까지 남는다(지연·유실 없음, 보낸 순서 그대로).
    /// 지연·유실·순서 뒤바뀜은 N3의 UnreliableTransport가 상속해서 EnqueueToServer/EnqueueToClient를 override해 얹는다.
    /// 스레드 안전하지 않다. 서버와 클라이언트를 같은 스레드의 틱 루프에서 돌린다는 전제이다(D-015).
    /// </summary>
    public partial class InMemoryTransport
    {
        // 클라이언트 → 서버 큐 하나(어느 클라이언트가 보냈는지는 항목에 함께 담는다).
        private readonly Queue<(int clientId, InputMessage message)> _toServerQueue = new();

        // 서버 → 클라이언트 큐는 클라이언트 번호별로 하나씩이다.
        private readonly Queue<SnapshotMessage>[] _toClientQueues;

        private int _createdClientCount;

        public InMemoryTransport(int clientCount)
        {
            if (clientCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(clientCount));
            }

            _toClientQueues = new Queue<SnapshotMessage>[clientCount];
            for (int i = 0; i < clientCount; i++)
            {
                _toClientQueues[i] = new Queue<SnapshotMessage>();
            }

            Server = new ServerHandle(this);
        }

        public IServerTransport Server { get; }

        /// <summary>클라이언트 핸들을 만든다. 호출 순서대로 0, 1, ... 번호가 배정되며 생성자의 clientCount를 넘으면 예외이다.</summary>
        public IClientTransport CreateClient()
        {
            if (_createdClientCount >= _toClientQueues.Length)
            {
                throw new InvalidOperationException("준비된 클라이언트 자리를 모두 사용했습니다.");
            }

            return new ClientHandle(this, _createdClientCount++);
        }

        /// <summary>클라이언트가 보낸 입력을 서버 수신 큐에 넣는다. 불량 전송이 override해 유실·지연을 끼워 넣는 지점이다.</summary>
        protected virtual void EnqueueToServer(int clientId, InputMessage message)
        {
            _toServerQueue.Enqueue((clientId, message));
        }

        /// <summary>서버가 보낸 스냅샷을 해당 클라이언트 수신 큐에 넣는다. clientId 범위는 호출 전에 검사된다.</summary>
        protected virtual void EnqueueToClient(int clientId, SnapshotMessage message)
        {
            _toClientQueues[clientId].Enqueue(message);
        }

        private static bool TryDequeue<T>(Queue<T> queue, out T message)
        {
            if (queue.Count == 0)
            {
                message = default;
                return false;
            }

            message = queue.Dequeue();
            return true;
        }
    }

    /// <summary>
    /// InMeomoryTransport의 private nested class들을 partial class로 분리해,
    /// InMemoryTransport.cs와 UnreliableTransport.cs에서 공유한다.
    /// </summary>
    public partial class InMemoryTransport
    {

        private sealed class ClientHandle : IClientTransport
        {
            private readonly InMemoryTransport _owner;
            private readonly int _clientId;

            public ClientHandle(InMemoryTransport owner, int clientId)
            {
                _owner = owner;
                _clientId = clientId;
            }

            public void Send(InputMessage message)
            {
                // 실제 전송은 직렬화로 바이트를 거치며 복사된다. 보내는 쪽이 배열을 재사용해도 수신 값이 바뀌지 않게 같은 효과를 낸다.
                TickInput[] copiedTickInputs = (TickInput[])message.tickInputs.Clone();
                _owner.EnqueueToServer(_clientId, new InputMessage(message.FirstTick, copiedTickInputs));
            }

            public bool TryReceive(out SnapshotMessage message)
            {
                return InMemoryTransport.TryDequeue(_owner._toClientQueues[_clientId], out message);
            }
        }

        private sealed class ServerHandle : IServerTransport
        {
            private readonly InMemoryTransport _owner;

            public ServerHandle(InMemoryTransport owner)
            {
                _owner = owner;
            }

            public void Send(int clientId, SnapshotMessage message)
            {
                // 없는 클라이언트 번호는 서버 코드의 오류이므로 예외로 알린다.
                if ((uint)clientId >= (uint)_owner._toClientQueues.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(clientId));
                }

                _owner.EnqueueToClient(clientId, message);
            }

            public bool TryReceive(out int clientId, out InputMessage message)
            {
                if (_owner._toServerQueue.Count == 0)
                {
                    clientId = default;
                    message = default;
                    return false;
                }

                (clientId, message) = _owner._toServerQueue.Dequeue();
                return true;
            }
        }


    }
}
