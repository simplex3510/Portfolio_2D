using System;

using NUnit.Framework;

using RuneBlade.Simulation;

namespace RuneBlade.Network.Tests
{
    public class InputMessageTests
    {
        private static TickInput PressOf(LogicalKey key)
        {
            var tickInput = new TickInput();
            Assert.IsTrue(tickInput.TryAdd(new KeyInputEvent(key, KeyTransition.Press)));
            return tickInput;
        }

        [Test]
        public void TryGetInput_InsideRange_ReturnsThatTicksInput()
        {
            var message = new InputMessage(
                10, new[] { PressOf(LogicalKey.Left), PressOf(LogicalKey.Right), PressOf(LogicalKey.Up) });

            Assert.IsTrue(message.TryGetTickInput(10, out TickInput first));
            Assert.IsTrue(message.TryGetTickInput(12, out TickInput last));
            Assert.That(first.RawEvents, Is.EqualTo(PressOf(LogicalKey.Left).RawEvents));
            Assert.That(last.RawEvents, Is.EqualTo(PressOf(LogicalKey.Up).RawEvents));
        }

        [Test]
        public void TryGetInput_OutsideRange_ReturnsFalse()
        {
            var message = new InputMessage(
                10, new[] { PressOf(LogicalKey.Left), PressOf(LogicalKey.Right), PressOf(LogicalKey.Up) });

            Assert.IsFalse(message.TryGetTickInput(9, out TickInput before));
            Assert.IsFalse(message.TryGetTickInput(13, out TickInput after));
            Assert.IsTrue(before.IsEmpty);
            Assert.IsTrue(after.IsEmpty);
        }

        [Test]
        public void Constructor_WithNoInputs_Throws()
        {
            Assert.Throws<ArgumentException>(() => new InputMessage(0, null));
            Assert.Throws<ArgumentException>(() => new InputMessage(0, new TickInput[0]));
        }
    }
}
