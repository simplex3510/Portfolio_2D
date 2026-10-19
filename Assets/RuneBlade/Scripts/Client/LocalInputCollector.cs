using System.Collections.Generic;

using RuneBlade.Simulation;

namespace RuneBlade.Client
{
    /// <summary>
    /// 프레임 중 발생한 키 이벤트를 모아 두었다가 틱마다 TickInput 하나로 묶어 내준다.
    /// Unity에 의존하지 않는 순수 C#이며, 이벤트 순서는 들어온 순서 그대로 보존된다.
    /// </summary>
    public sealed class LocalInputCollector
    {
        private readonly Queue<KeyInputEvent> _pendingInputEvents = new();

        public void AddKeyInput(KeyInputEvent inputEvent)
        {
            _pendingInputEvents.Enqueue(inputEvent);
        }

        /// <summary>
        /// 이번 틱의 입력을 만든다. 한 틱에 담는 이벤트는 TickInput.Capacity까지이며,
        /// 넘친 이벤트는 큐에 남아 다음 틱으로 이월된다. 이벤트가 없으면 빈 입력을 돌려준다.
        /// </summary>
        public TickInput GetTickInput()
        {
            TickInput input = default;
            while (_pendingInputEvents.Count > 0 && !input.IsFull)
            {
                input.TryAdd(_pendingInputEvents.Dequeue());
            }

            return input;
        }
    }
}
