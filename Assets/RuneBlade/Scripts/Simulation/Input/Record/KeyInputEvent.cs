namespace RuneBlade.Simulation
{
    /// <summary>키 전이의 종류. 값이 곧 기록 형식이므로 번호를 바꾸면 안 된다.</summary>
    public enum KeyTransition : byte
    {
        Press = 0,
        Release = 1,
    }

    /// <summary>
    /// 키 전이 이벤트 1개(누름 또는 뗌). 내부 표현은 1바이트이다.
    ///
    /// 비트 배치: [7..5 예약(항상 0)] [4 종류: 0=Press, 1=Release] [3..0 LogicalKey 값]
    ///
    /// 순수 정보만 담는다. 이 이벤트가 규칙상 유효한지(예: 이미 눌린 키의 Press)는
    /// 담지 않으며, 그 판정은 시뮬레이션이 눌림 상태와 대조해서 한다.
    /// 이름에 Key를 넣은 이유: Input 어셈블리에서 UnityEngine.InputSystem의 InputEvent와
    /// 이름이 겹쳐 using 충돌이 나는 것을 피하기 위해서이다.
    /// </summary>
    public readonly struct KeyInputEvent
    {
        private const int KeyMask = 0b_0000_1111;           // 하위 4비트: 논리 키값
        private const int KeyTransitionBit = 0b_0001_0000;  // 5번째 비트: 키 트렌지션 종류
        private const int ReservedMask = 0b_1110_0000;  // 상위 3비트: 예약

        /// <summary>1바이트로 포장된 원본 값. TickInput 저장과 직렬화에 그대로 쓴다.</summary>
        public readonly byte PackedEvent;

        public KeyInputEvent(LogicalKey key, KeyTransition transition)
        {
            int rawEvent = (int)key & KeyMask;
            if (transition == KeyTransition.Release)
            {
                rawEvent |= KeyTransitionBit;
            }

            PackedEvent = (byte)rawEvent;
        }

        // 이미 포장된 바이트(TickInput 저장분, 리플레이, 네트워크 수신)에서 복원할 때 쓴다.
        private KeyInputEvent(byte packedEvent)
        {
            PackedEvent = packedEvent;
        }

        /// <summary>포장된 바이트에서 KeyInputEvent를 생성한다.</summary>
        public static KeyInputEvent CreateFromPacked(byte packedEvent)
        {
            return new KeyInputEvent(packedEvent);
        }

        public LogicalKey Key => (LogicalKey)(PackedEvent & KeyMask);

        public KeyTransition Transition =>
            (PackedEvent & KeyTransitionBit) != 0 ? KeyTransition.Release : KeyTransition.Press;

        public bool IsPress => (PackedEvent & KeyTransitionBit) == 0;

        public bool IsRelease => (PackedEvent & KeyTransitionBit) != 0;

        /// <summary>
        /// 예약 비트가 0이고 키 값이 실제 논리키 범위일 때만 유효하다.
        /// 외부에서 들어온 기록(리플레이 파일, 네트워크)이 손상됐을 때 걸러내는 용도이며,
        /// 유효하지 않은 이벤트를 어떻게 다룰지는 호출하는 쪽이 정한다.
        /// </summary>
        public bool IsValid =>
            (PackedEvent & ReservedMask) == 0 && LogicalKeyUtility.IsValid(Key);

        public override string ToString() => $"({Key}, {Transition})";
    }
}