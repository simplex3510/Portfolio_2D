using NUnit.Framework;

using RuneBlade.Network;
using RuneBlade.Simulation;

namespace RuneBlade.Client.Tests
{
    public class MovementReconcilerTests
    {
        private static readonly MovementSettings Settings = MovementSettings.CreateDefault();

        // ---- 테스트 도우미 ----

        private static TickInput InputOf(params KeyInputEvent[] events)
        {
            var tickInput = new TickInput();
            foreach (KeyInputEvent inputEvent in events)
            {
                Assert.IsTrue(tickInput.TryAdd(inputEvent));
            }

            return tickInput;
        }

        private static KeyInputEvent Press(LogicalKey key) => new KeyInputEvent(key, KeyTransition.Press);

        private static KeyInputEvent Release(LogicalKey key) => new KeyInputEvent(key, KeyTransition.Release);

        // 클라이언트처럼 입력을 틱 0부터 순서대로 예측하며 이력에 기록한다.
        private static MovementSimulation Predict(PredictionHistory history, TickInput[] inputs)
        {
            MovementSimulation current = MovementSimulation.CreateOnGround(0f);
            for (int tick = 0; tick < inputs.Length; tick++)
            {
                current.Tick(inputs[tick], Settings);
                history.Record(tick, inputs[tick], current);
            }

            return current;
        }

        // 서버가 받은 입력으로 계산한 틱 시점의 상태.
        private static SnapshotMessage ServerSnapshot(TickInput[] serverInputs, int lastProcessedTick)
        {
            MovementSimulation server = MovementSimulation.CreateOnGround(0f);
            for (int tick = 0; tick <= lastProcessedTick; tick++)
            {
                server.Tick(serverInputs[tick], Settings);
            }

            return new SnapshotMessage(lastProcessedTick, server);
        }

        private static TickInput[] EmptyInputs(int count) => new TickInput[count];

        // ---- 일치 ----

        [Test]
        public void SameInputsOnBothSides_IsMatched_AndCurrentIsUnchanged()
        {
            TickInput[] inputs = EmptyInputs(10);
            inputs[2] = InputOf(Press(LogicalKey.Right));
            var history = new PredictionHistory();
            MovementSimulation current = Predict(history, inputs);
            float positionBefore = current.PositionX;

            var reconciler = new MovementReconciler();
            ReconcileResult result = reconciler.Reconcile(ServerSnapshot(inputs, 4), history, ref current, Settings);

            Assert.That(result, Is.EqualTo(ReconcileResult.Matched));
            Assert.That(current.PositionX, Is.EqualTo(positionBefore).Within(MovementReconciler.DefaultPositionTolerance));
        }

        // ---- 보정 ----

        // 서버가 틱 2의 Right 누름을 받지 못했고, 스냅샷 이후(틱 6)의 점프 입력은 재적용되어야 한다.
        [Test]
        public void LostInput_IsCorrected_AndLaterInputsAreReplayedOnServerState()
        {
            TickInput[] clientInputs = EmptyInputs(10);
            clientInputs[2] = InputOf(Press(LogicalKey.Right));
            clientInputs[6] = InputOf(Press(LogicalKey.Up));
            TickInput[] serverInputs = (TickInput[])clientInputs.Clone();
            serverInputs[2] = default;

            var history = new PredictionHistory();
            MovementSimulation current = Predict(history, clientInputs);
            Assert.That(current.PositionX, Is.GreaterThan(0.1f));

            var reconciler = new MovementReconciler();
            ReconcileResult result = reconciler.Reconcile(ServerSnapshot(serverInputs, 4), history, ref current, Settings);

            Assert.That(result, Is.EqualTo(ReconcileResult.Corrected));
            Assert.That(current.PositionX, Is.EqualTo(0f).Within(MovementReconciler.DefaultPositionTolerance));
            Assert.That(current.PositionY, Is.GreaterThan(0f), "스냅샷 이후 점프 입력이 재적용되어야 한다.");
        }

        [Test]
        public void AfterCorrection_HistoryHoldsReplayedStates()
        {
            TickInput[] clientInputs = EmptyInputs(10);
            clientInputs[2] = InputOf(Press(LogicalKey.Right));
            TickInput[] serverInputs = EmptyInputs(10);

            var history = new PredictionHistory();
            MovementSimulation current = Predict(history, clientInputs);

            new MovementReconciler().Reconcile(ServerSnapshot(serverInputs, 4), history, ref current, Settings);

            Assert.IsTrue(history.TryGetState(4, out MovementSimulation atSnapshotTick));
            Assert.That(atSnapshotTick.PositionX, Is.EqualTo(0f));
            Assert.IsTrue(history.TryGetState(9, out MovementSimulation atNewestTick));
            Assert.That(atNewestTick.PositionX, Is.EqualTo(current.PositionX));
        }

        // 위치는 같지만 서버에만 더블탭 이력이 있다. 서버 상태를 채택하므로 이후 입력이 대시로 이어진다.
        [Test]
        public void HiddenStateOnlyDifference_IsAdopted_AndAffectsReplay()
        {
            TickInput[] clientInputs = EmptyInputs(10);
            clientInputs[5] = InputOf(Press(LogicalKey.Left));
            TickInput[] serverInputs = (TickInput[])clientInputs.Clone();
            serverInputs[2] = InputOf(Press(LogicalKey.Left), Release(LogicalKey.Left));

            var history = new PredictionHistory();
            MovementSimulation current = Predict(history, clientInputs);
            Assert.IsFalse(current.IsDashing);

            ReconcileResult result = new MovementReconciler().Reconcile(
                ServerSnapshot(serverInputs, 4), history, ref current, Settings);

            Assert.That(result, Is.EqualTo(ReconcileResult.Corrected));
            Assert.IsTrue(current.IsDashing);
        }

        // ---- 무시 ----

        [Test]
        public void SnapshotOlderThanHistory_IsIgnored_AndCurrentIsUnchanged()
        {
            var history = new PredictionHistory(4);
            MovementSimulation current = Predict(history, EmptyInputs(10)); // 틱 6~9만 남는다.
            MovementSimulation before = current;

            ReconcileResult result = new MovementReconciler().Reconcile(
                new SnapshotMessage(2, MovementSimulation.CreateOnGround(5f)), history, ref current, Settings);

            Assert.That(result, Is.EqualTo(ReconcileResult.Ignored));
            Assert.That(current.PositionX, Is.EqualTo(before.PositionX));
        }

        [Test]
        public void SnapshotAheadOfHistory_IsIgnored()
        {
            var history = new PredictionHistory();
            MovementSimulation current = Predict(history, EmptyInputs(5));

            ReconcileResult result = new MovementReconciler().Reconcile(
                new SnapshotMessage(20, MovementSimulation.CreateOnGround(5f)), history, ref current, Settings);

            Assert.That(result, Is.EqualTo(ReconcileResult.Ignored));
        }

        [Test]
        public void OlderSnapshotArrivingAfterNewerOne_IsIgnored()
        {
            TickInput[] inputs = EmptyInputs(10);
            var history = new PredictionHistory();
            MovementSimulation current = Predict(history, inputs);
            var reconciler = new MovementReconciler();

            reconciler.Reconcile(ServerSnapshot(inputs, 6), history, ref current, Settings);
            ReconcileResult result = reconciler.Reconcile(ServerSnapshot(inputs, 3), history, ref current, Settings);

            Assert.That(result, Is.EqualTo(ReconcileResult.Ignored));
        }

        [Test]
        public void SameTickSnapshotTwice_SecondIsIgnored()
        {
            TickInput[] inputs = EmptyInputs(10);
            var history = new PredictionHistory();
            MovementSimulation current = Predict(history, inputs);
            var reconciler = new MovementReconciler();

            reconciler.Reconcile(ServerSnapshot(inputs, 4), history, ref current, Settings);
            ReconcileResult result = reconciler.Reconcile(ServerSnapshot(inputs, 4), history, ref current, Settings);

            Assert.That(result, Is.EqualTo(ReconcileResult.Ignored));
        }

        [Test]
        public void Constructor_WithNegativeTolerance_Throws()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new MovementReconciler(-1f));
        }
    }
}
