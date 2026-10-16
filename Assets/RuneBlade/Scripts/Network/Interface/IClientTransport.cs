namespace RuneBlade.Network
{
    /// <summary>
    /// 클라이언트 쪽 전송 계층(D-014). 메모리 전송과 실제 소켓 전송을 바꿔 끼울 수 있게 하는 경계이다.
    /// 모든 메시지는 신뢰성·순서가 보장되지 않는 전송으로 가정한다
    /// (입력은 중복 전송, 스냅샷은 최신이 이전을 덮는 구조로 유실을 감당한다).
    /// 수신은 이벤트가 아니라 폴링이다. 틱 루프가 원하는 시점에 TryReceive를 false가 나올 때까지 호출해 비운다.
    /// TODO: 신뢰성이 필요한 메시지(접속, 룸 진행)는 S2/S6에서 필요해질 때 추가한다.
    /// </summary>
    public interface IClientTransport
    {
        void Send(InputMessage message);

        /// <summary>도착한 스냅샷 하나를 꺼낸다. 없으면 false이다.</summary>
        bool TryReceive(out SnapshotMessage message);
    }
}
