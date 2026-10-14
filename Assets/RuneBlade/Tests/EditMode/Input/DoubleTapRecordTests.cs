using System;
using NUnit.Framework;

namespace RuneBlade.Simulation.Tests
{
    public class DoubleTapRecordTests
    {
        // 테스트용 창 길이. 실제 수치는 [잠정]이며 프로토타입 후 확정된다.
        private const int Window = 3;

        private static void Advance(ref DoubleTapRecord record, int ticks)
        {
            for (int i = 0; i < ticks; i++)
            {
                record.AdvanceTick();
            }
        }

        // ---- 기본 인식 ----

        [Test]
        public void SameKeyWithinWindow_IsRecognized_AndClearsRecord()
        {
            var record = new DoubleTapRecord();

            Assert.IsFalse(record.RegisterPress(LogicalKey.Right, Window));
            Advance(ref record, 2);
            Assert.IsTrue(record.RegisterPress(LogicalKey.Right, Window));

            Assert.IsFalse(record.IsActive); // 인식되면 이력 초기화
        }

        [Test]
        public void SameTick_PressTwice_IsRecognized()
        {
            // 간격 0틱: 같은 틱의 [Press, Release, Press]에서 두 번째 Press
            var record = new DoubleTapRecord();

            record.RegisterPress(LogicalKey.Left, Window);

            Assert.IsTrue(record.RegisterPress(LogicalKey.Left, Window));
        }

        [Test]
        public void Boundary_PressAtWindowTicks_IsRecognized()
        {
            var record = new DoubleTapRecord();

            record.RegisterPress(LogicalKey.Right, Window);
            Advance(ref record, Window); // 간격 d = Window

            Assert.IsTrue(record.RegisterPress(LogicalKey.Right, Window));
        }

        [Test]
        public void Boundary_PressAfterWindow_IsNotRecognized_AndBecomesNewFirstPress()
        {
            var record = new DoubleTapRecord();

            record.RegisterPress(LogicalKey.Right, Window);
            Advance(ref record, Window + 1); // 간격 d = Window + 1 → 만료

            Assert.IsFalse(record.IsActive);
            Assert.IsFalse(record.RegisterPress(LogicalKey.Right, Window));
            Assert.IsTrue(record.IsActive); // 새 첫 Press로 기록됨
            Assert.That(record.RemainingTicks, Is.EqualTo(Window));
        }

        [Test]
        public void DownDown_IsRecognized()
        {
            var record = new DoubleTapRecord();

            record.RegisterPress(LogicalKey.Down, Window);

            Assert.IsTrue(record.RegisterPress(LogicalKey.Down, Window));
        }

        // ---- 다른 방향이 끼면 무효화 ----

        [Test]
        public void LeftRightLeft_IsNotRecognized()
        {
            var record = new DoubleTapRecord();

            Assert.IsFalse(record.RegisterPress(LogicalKey.Left, Window));
            Assert.IsFalse(record.RegisterPress(LogicalKey.Right, Window)); // 첫 Press가 →로 교체됨
            Assert.IsFalse(record.RegisterPress(LogicalKey.Left, Window));  // ←는 이미 무효화되어 있음

            Assert.That(record.Key, Is.EqualTo(LogicalKey.Left));
        }

        [Test]
        public void UpBetween_InvalidatesFirstPress()
        {
            var record = new DoubleTapRecord();

            record.RegisterPress(LogicalKey.Right, Window);
            record.RegisterPress(LogicalKey.Up, Window);

            Assert.IsFalse(record.RegisterPress(LogicalKey.Right, Window));
        }

        // ---- ↑는 인식 대상이 아님 ----

        [Test]
        public void UpUp_IsNeverRecognized_ButReplacesRecord()
        {
            var record = new DoubleTapRecord();

            Assert.IsFalse(record.RegisterPress(LogicalKey.Up, Window));
            Assert.IsFalse(record.RegisterPress(LogicalKey.Up, Window));

            Assert.That(record.Key, Is.EqualTo(LogicalKey.Up));
            Assert.That(record.RemainingTicks, Is.EqualTo(Window)); // 창이 새로 시작됨
        }

        // ---- 인식 후 초기화 ----

        [Test]
        public void AfterRecognition_ThirdPressStartsNewRecord()
        {
            var record = new DoubleTapRecord();
            record.RegisterPress(LogicalKey.Right, Window);
            record.RegisterPress(LogicalKey.Right, Window); // 인식, 초기화

            // 세 번째 Press는 인식이 아니라 새 첫 Press
            Assert.IsFalse(record.RegisterPress(LogicalKey.Right, Window));
            Assert.IsTrue(record.IsActive);
        }

        // ---- 시간 동결 ----

        [Test]
        public void WithoutAdvanceTick_WindowStaysFrozen()
        {
            // 히트스톱 중에는 호출하는 쪽이 AdvanceTick을 부르지 않는다
            var record = new DoubleTapRecord();
            record.RegisterPress(LogicalKey.Right, Window);

            // (정지 구간: AdvanceTick 호출 없음)

            Assert.That(record.RemainingTicks, Is.EqualTo(Window));
            Assert.IsTrue(record.RegisterPress(LogicalKey.Right, Window));
        }

        // ---- 방향키가 아닌 키, 초기화, 인자 검증 ----

        [Test]
        public void SlotKey_DoesNotAffectRecord()
        {
            var record = new DoubleTapRecord();
            record.RegisterPress(LogicalKey.Right, Window);
            DoubleTapRecord before = record;

            bool recognized = record.RegisterPress(LogicalKey.Slot1, Window);

            Assert.IsFalse(recognized);
            Assert.That(record, Is.EqualTo(before)); // 기존 첫 Press가 무효화되지 않음
        }

        [Test]
        public void Clear_ResetsToDefault()
        {
            var record = new DoubleTapRecord();
            record.RegisterPress(LogicalKey.Left, Window);

            record.Clear();

            Assert.That(record, Is.EqualTo(new DoubleTapRecord()));
        }

        [Test]
        public void WindowOutOfRange_Throws()
        {
            var record = new DoubleTapRecord();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => record.RegisterPress(LogicalKey.Right, DoubleTapRecord.MaxWindowTicks + 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => record.RegisterPress(LogicalKey.Right, -1));
        }
    }
}