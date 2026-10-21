using NUnit.Framework;

namespace RuneBlade.Simulation.Tests
{
    /// <summary>
    /// 입력 내용만으로는 Tick()이 예외를 던지거나 상태를 망가뜨리지 않음을 고정한다.
    /// 서버가 외부 입력을 재검증하더라도, 시뮬레이션 자체가 이상한 이벤트에 안전하다는 보장이다.
    /// </summary>
    public class MovementSimulationTickRobustnessTests
    {
        private static readonly MovementSettings Settings = MovementSettings.CreateDefault();

        private static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

        [Test]
        public void Tick_WithAnyPackedEventByte_DoesNotThrowAndStaysFinite()
        {
            MovementSimulation simulation = MovementSimulation.CreateOnGround(0f);

            for (int packed = 0; packed <= byte.MaxValue; packed++)
            {
                var input = new TickInput();
                Assert.IsTrue(input.TryAdd(KeyInputEvent.CreateFromPacked((byte)packed)));

                Assert.DoesNotThrow(() => simulation.Tick(input, Settings), $"packed={packed}");
            }

            Assert.IsTrue(IsFinite(simulation.PositionX));
            Assert.IsTrue(IsFinite(simulation.PositionY));
            Assert.IsTrue(IsFinite(simulation.VelocityY));
        }

        [Test]
        public void Tick_WithFullTickOfSameGarbageByte_DoesNotThrow()
        {
            for (int packed = 0; packed <= byte.MaxValue; packed++)
            {
                MovementSimulation simulation = MovementSimulation.CreateOnGround(0f);
                var input = new TickInput();
                while (!input.IsFull)
                {
                    input.TryAdd(KeyInputEvent.CreateFromPacked((byte)packed));
                }

                Assert.DoesNotThrow(() => simulation.Tick(input, Settings), $"packed={packed}");
            }
        }
    }
}
