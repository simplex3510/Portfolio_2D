using System;

using NUnit.Framework;

using RuneBlade.Simulation;

namespace RuneBlade.Client.Tests
{
    public class PredictionHistoryTests
    {
        // ---- 테스트 도우미 ----

        // 키가 다르면 RawEvents도 달라서 어느 입력인지 구분할 수 있다.
        private static TickInput PressOf(LogicalKey key)
        {
            var tickInput = new TickInput();
            Assert.IsTrue(tickInput.TryAdd(new KeyInputEvent(key, KeyTransition.Press)));
            return tickInput;
        }

        // 위치 X로 어느 틱의 상태인지 구분한다.
        private static MovementSimulation StateAt(float positionX)
        {
            return MovementSimulation.CreateOnGround(positionX);
        }

        // ---- 기록과 조회 ----

        [Test]
        public void Record_ThenGet_ReturnsSameInputAndState()
        {
            var history = new PredictionHistory(8);

            history.Record(5, PressOf(LogicalKey.Left), StateAt(1f));
            history.Record(6, PressOf(LogicalKey.Up), StateAt(2f));

            Assert.IsTrue(history.TryGetInput(5, out TickInput input5));
            Assert.That(input5.RawEvents, Is.EqualTo(PressOf(LogicalKey.Left).RawEvents));
            Assert.IsTrue(history.TryGetState(6, out MovementSimulation state6));
            Assert.That(state6.PositionX, Is.EqualTo(2f));
            Assert.That(history.NewestTick, Is.EqualTo(6));
        }

        [Test]
        public void Get_BeforeFirstOrAfterNewest_ReturnsFalse()
        {
            var history = new PredictionHistory(8);
            history.Record(5, default, StateAt(1f));

            Assert.IsFalse(history.TryGetState(4, out _));
            Assert.IsFalse(history.TryGetState(6, out _));
            Assert.IsFalse(history.TryGetInput(4, out _));
        }

        [Test]
        public void Get_WhenNothingRecorded_ReturnsFalse()
        {
            var history = new PredictionHistory(8);

            Assert.IsFalse(history.TryGetState(0, out _));
            Assert.Throws<InvalidOperationException>(() => _ = history.NewestTick);
        }

        // ---- 연속성 ----

        [Test]
        public void Record_WithSkippedOrRepeatedTick_Throws()
        {
            var history = new PredictionHistory(8);
            history.Record(5, default, StateAt(1f));

            Assert.Throws<ArgumentException>(() => history.Record(7, default, StateAt(2f)));
            Assert.Throws<ArgumentException>(() => history.Record(5, default, StateAt(2f)));
        }

        // ---- 용량 ----

        [Test]
        public void Record_BeyondCapacity_DropsOldestTicksOnly()
        {
            var history = new PredictionHistory(4);

            for (int tick = 0; tick < 6; tick++)
            {
                history.Record(tick, default, StateAt(tick));
            }

            // 용량 4: 틱 2~5만 남는다.
            Assert.IsFalse(history.TryGetState(0, out _));
            Assert.IsFalse(history.TryGetState(1, out _));
            Assert.IsTrue(history.TryGetState(2, out MovementSimulation oldest));
            Assert.That(oldest.PositionX, Is.EqualTo(2f));
            Assert.IsTrue(history.TryGetState(5, out MovementSimulation newest));
            Assert.That(newest.PositionX, Is.EqualTo(5f));
        }

        [Test]
        public void Constructor_WithNonPositiveCapacity_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PredictionHistory(0));
        }
    }
}
