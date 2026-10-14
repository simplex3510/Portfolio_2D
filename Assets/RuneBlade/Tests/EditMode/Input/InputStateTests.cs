using NUnit.Framework;

using RuneBlade.Simulation;

namespace RuneBlade.Simulation.Tests
{
    public class InputStateTests
    {
        // ---- 테스트 도우미 ----

        private static KeyInputEvent Press(LogicalKey key) => new KeyInputEvent(key, KeyTransition.Press);

        private static KeyInputEvent Release(LogicalKey key) => new KeyInputEvent(key, KeyTransition.Release);

        /// <summary>
        /// 한 틱 분량의 TickInput을 만든다. 실제 경로와 같게 TickInput을 거치므로
        /// "도착 순서가 TickInput에 보존되는지"도 함께 검증된다.
        /// </summary>
        private static TickInput Tick(params KeyInputEvent[] events)
        {
            var tickInput = new TickInput();
            foreach (KeyInputEvent inputEvent in events)
            {
                Assert.IsTrue(tickInput.TryAdd(inputEvent), "테스트 입력이 TickInput 용량을 넘었습니다.");
            }

            return tickInput;
        }

        /// <summary>TickInput의 이벤트를 도착 순서대로 state에 적용한다.</summary>
        private static void ApplyAll(ref InputState state, in TickInput tickInput)
        {
            for (int i = 0; i < tickInput.EventCount; i++)
            {
                state.TryApplyKeyEvent(tickInput.GetEventAt(i));
            }
        }

        // ---- 같은 틱 좌우 동시 입력: 도착 순서가 결과를 정한다 ----

        [Test]
        public void SameTick_LeftThenRight_RightWins()
        {
            var state = new InputState();

            ApplyAll(ref state, Tick(Press(LogicalKey.Left), Press(LogicalKey.Right)));

            Assert.That(state.Horizontal, Is.EqualTo(HorizontalDirection.Right));
            Assert.IsTrue(state.IsHeldKey(LogicalKey.Left));
            Assert.IsTrue(state.IsHeldKey(LogicalKey.Right));
        }

        [Test]
        public void SameTick_RightThenLeft_LeftWins()
        {
            var state = new InputState();

            ApplyAll(ref state, Tick(Press(LogicalKey.Right), Press(LogicalKey.Left)));

            Assert.That(state.Horizontal, Is.EqualTo(HorizontalDirection.Left));
        }

        [Test]
        public void SameTick_OrderIsPartOfTheRecord()
        {
            // 순서만 다른 두 입력은 서로 다른 값이어야 한다(리플레이·해시가 순서를 구분하는 근거).
            TickInput leftFirst = Tick(Press(LogicalKey.Left), Press(LogicalKey.Right));
            TickInput rightFirst = Tick(Press(LogicalKey.Right), Press(LogicalKey.Left));

            Assert.AreNotEqual(leftFirst, rightFirst);
        }

        // ---- 우선 방향 복귀 ----

        [Test]
        public void ReleasePreferred_ReturnsToStillHeldSide()
        {
            var state = new InputState();
            ApplyAll(ref state, Tick(Press(LogicalKey.Left), Press(LogicalKey.Right))); // 우선 = Right

            state.TryApplyKeyEvent(Release(LogicalKey.Right));

            Assert.That(state.Horizontal, Is.EqualTo(HorizontalDirection.Left));
        }

        [Test]
        public void ReleaseNonPreferred_KeepsPreferred()
        {
            var state = new InputState();
            ApplyAll(ref state, Tick(Press(LogicalKey.Left), Press(LogicalKey.Right))); // 우선 = Right

            state.TryApplyKeyEvent(Release(LogicalKey.Left));

            Assert.That(state.Horizontal, Is.EqualTo(HorizontalDirection.Right));
        }

        [Test]
        public void ReleaseLastHeld_BecomesNone()
        {
            var state = new InputState();
            state.TryApplyKeyEvent(Press(LogicalKey.Right));

            state.TryApplyKeyEvent(Release(LogicalKey.Right));

            Assert.That(state.Horizontal, Is.EqualTo(HorizontalDirection.None));
            Assert.That(state.HeldKeyMask, Is.EqualTo((ushort)0));
        }

        [Test]
        public void SameTick_PressAndReleaseOfSameKey_LeavesNothingHeld()
        {
            var state = new InputState();

            ApplyAll(ref state, Tick(Press(LogicalKey.Right), Release(LogicalKey.Right)));

            Assert.That(state, Is.EqualTo(new InputState()));
        }

        // ---- 모순 이벤트: 상태는 그대로, false 반환 ----

        [Test]
        public void DuplicatePress_ReturnsFalse_AndKeepsPreferred()
        {
            var state = new InputState();
            ApplyAll(ref state, Tick(Press(LogicalKey.Left), Press(LogicalKey.Right))); // 우선 = Right
            InputState before = state;

            bool changed = state.TryApplyKeyEvent(Press(LogicalKey.Left)); // 이미 눌린 키의 Press

            Assert.IsFalse(changed);
            Assert.That(state, Is.EqualTo(before)); // 우선 방향도 Left로 뒤집히지 않아야 한다
        }

        [Test]
        public void ReleaseOfKeyNotHeld_ReturnsFalse()
        {
            // 첫 라운드에서 시뮬레이션 시작 전에 눌려 있던 키를 떼는 경우와 같은 형태
            var state = new InputState();

            bool changed = state.TryApplyKeyEvent(Release(LogicalKey.Right));

            Assert.IsFalse(changed);
            Assert.That(state, Is.EqualTo(new InputState()));
        }

        [Test]
        public void KeyOutOfRange_ReturnsFalse()
        {
            var state = new InputState();
            KeyInputEvent corrupted = KeyInputEvent.CreateFromPacked(0x0F); // 키 값 15 = 논리키 범위 밖

            bool changed = state.TryApplyKeyEvent(corrupted);

            Assert.IsFalse(changed);
            Assert.That(state, Is.EqualTo(new InputState()));
        }

        // ---- 좌우 외의 키 ----

        [Test]
        public void NonHorizontalKeys_DoNotChangeHorizontal()
        {
            var state = new InputState();
            state.TryApplyKeyEvent(Press(LogicalKey.Right));

            state.TryApplyKeyEvent(Press(LogicalKey.Up));
            state.TryApplyKeyEvent(Press(LogicalKey.Slot1));

            Assert.That(state.Horizontal, Is.EqualTo(HorizontalDirection.Right));
            Assert.IsTrue(state.IsHeldKey(LogicalKey.Up));
            Assert.IsTrue(state.IsHeldKey(LogicalKey.Slot1));
        }

        [Test]
        public void AllElevenKeys_HaveIndependentBits()
        {
            var state = new InputState();

            for (int i = 0; i < LogicalKeyUtility.KeyCount; i++)
            {
                Assert.IsTrue(state.TryApplyKeyEvent(Press((LogicalKey)i)));
            }

            // 11개 키가 모두 눌린 상태 = 하위 11비트 전부 1
            Assert.That(state.HeldKeyMask, Is.EqualTo((ushort)0x07FF));
        }
    }
}