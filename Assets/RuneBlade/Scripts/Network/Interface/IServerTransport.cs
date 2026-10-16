namespace RuneBlade.Network
{
    /// <summary>
    /// 서버 쪽 전송 계층(D-014). IClientTransport의 반대 방향이며 같은 전제(비신뢰 전송, 폴링 수신)를 따른다.
    /// clientId는 서버가 접속한 클라이언트를 구분하는 번호이다.
    /// TODO: 1:1 룸이므로 0과 1 두 자리를 가정한다. 자리 배정과 범위는 S2에서 확정한다.
    /// </summary>
    public interface IServerTransport
    {
        void Send(int clientId, SnapshotMessage message);

        /// <summary>도착한 입력 하나를 보낸 클라이언트 번호와 함께 꺼낸다. 없으면 false이다.</summary>
        bool TryReceive(out int clientId, out InputMessage message);
    }
}
