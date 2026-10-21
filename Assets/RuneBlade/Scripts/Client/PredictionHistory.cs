using System;

using RuneBlade.Simulation;

namespace RuneBlade.Client
{
    /// <summary>
    /// 클라이언트가 예측한 틱별 기록. 틱 번호마다 "그 틱에 넣은 입력"과 "그 틱을 진행한 뒤의 예측 상태"를 보관한다.
    /// 보정(reconciliation) 때 서버 스냅샷 틱의 예측 상태와 비교하고, 그다음 틱부터의 입력을 다시 적용하는 데 쓴다.
    ///
    /// 고정 크기 링 버퍼이며, 용량을 넘기면 가장 오래된 틱부터 덮어쓴다. 밀려난 틱은 조회되지 않는다.
    /// 틱은 연속으로만 기록할 수 있다(건너뛰면 보정 때 입력을 재적용할 수 없으므로 프로그래밍 오류로 본다).
    /// </summary>
    public sealed class PredictionHistory
    {
        // TODO(tuning): 임시값. 약 2초 분량(60Hz). 서버 응답이 이보다 늦으면 보정 기준 틱이 이력에서 밀려난다.
        public const int DefaultCapacity = 128;

        private readonly TickInput[] _inputs;
        private readonly MovementSimulation[] _states;

        private bool _hasRecorded;
        private int _firstTick;
        private int _newestTick;

        public PredictionHistory(int capacity = DefaultCapacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _inputs = new TickInput[capacity];
            _states = new MovementSimulation[capacity];
        }

        public int Capacity => _inputs.Length;

        /// <summary>가장 최근에 기록한 틱 번호. 기록이 하나도 없으면 읽을 수 없다.</summary>
        public int NewestTick
        {
            get
            {
                if (!_hasRecorded)
                {
                    throw new InvalidOperationException("기록된 틱이 없습니다.");
                }

                return _newestTick;
            }
        }

        /// <summary>한 틱의 입력과 진행 후 상태를 기록한다. 첫 기록 외에는 직전 틱 + 1이어야 한다.</summary>
        public void Record(int tick, in TickInput input, in MovementSimulation stateAfterTick)
        {
            if (!_hasRecorded)
            {
                _hasRecorded = true;
                _firstTick = tick;
            }
            else if (tick != _newestTick + 1)
            {
                throw new ArgumentException($"틱은 연속으로 기록해야 합니다. 직전={_newestTick}, 요청={tick}", nameof(tick));
            }

            int slot = SlotOf(tick);
            _inputs[slot] = input;
            _states[slot] = stateAfterTick;
            _newestTick = tick;
        }

        /// <summary>
        /// 이미 기록된 틱의 상태만 교체한다(보정 중 재적용한 결과를 다시 쓰는 용도).
        /// 기록 범위 밖이면 프로그래밍 오류로 보고 예외를 던진다.
        /// </summary>
        public void ReplaceState(int tick, in MovementSimulation state)
        {
            if (!Contains(tick))
            {
                throw new ArgumentOutOfRangeException(nameof(tick), $"기록 범위 밖의 틱입니다: {tick}");
            }

            _states[SlotOf(tick)] = state;
        }

        /// <summary>해당 틱의 입력을 읽는다. 기록된 적 없거나 이력에서 밀려났으면 false이다.</summary>
        public bool TryGetInput(int tick, out TickInput input)
        {
            if (!Contains(tick))
            {
                input = default;
                return false;
            }

            input = _inputs[SlotOf(tick)];
            return true;
        }

        /// <summary>해당 틱을 진행한 뒤의 예측 상태를 읽는다. 기록된 적 없거나 이력에서 밀려났으면 false이다.</summary>
        public bool TryGetState(int tick, out MovementSimulation state)
        {
            if (!Contains(tick))
            {
                state = default;
                return false;
            }

            state = _states[SlotOf(tick)];
            return true;
        }

        // 기록된 범위: 최근 Capacity개 틱 중 실제로 기록한 것.
        private bool Contains(int tick)
        {
            // 1. 기록된 적이 있고,
            // 2. tick이 기록된 범위 안에 있고,
            // 3. tick이 용량 내에 존재해야 한다.
            return _hasRecorded
                && tick >= _firstTick
                && tick <= _newestTick
                && tick > _newestTick - _inputs.Length;
        }

        // 틱이 음수여도 슬롯이 음수가 되지 않게 한다.
        private int SlotOf(int tick)
        {
            return ((tick % _inputs.Length) + _inputs.Length) % _inputs.Length;
        }
    }
}
