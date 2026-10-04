using System;
using UnityEngine;

namespace Game.Input.Attack
{
    public enum AttackInputState
    {
        None,
        Windup,     // 공격 발동 전 준비 상태 (선 딜레이)
        Execute,    // 공격 발동 중 상태
        Recovery    // 공격 발동 후 복귀 상태 (후 딜레이)
    }

    /// <summary>
    /// 가공된 마우스 공격 입력 하나를 나타내는 값 타입.
    /// PlayerAttackInputReader가 판정 결과로 만들어 내보내는 값이자,
    /// AttackInputSO가 "자신이 어떤 입력인지"를 정의하는 값이다.
    /// 네 값이 모두 같으면 같은 입력으로 취급한다.
    /// </summary>
    [Serializable]
    public struct AttackInput : IEquatable<AttackInput>
    {
        [SerializeField] private MouseInputButton _button;

        [SerializeField] private MouseInputGesture _gesture;

        // 버튼을 누르는 순간 Shift가 눌려 있었는지 (press 시점에 한 번만 샘플링)
        [SerializeField] private MouseInputModifier _shiftModifier;

        private MouseInputPhase _phase;


        public MouseInputButton Button { get { return _button; } }
        public MouseInputGesture Gesture { get {return _gesture; } }
        public MouseInputModifier ShiftModifier { get { return _shiftModifier; } }
        public MouseInputPhase Phase { get {return _phase; } }
        // public AttackInputState State { get { return _state; } }
        
        public AttackInput(
            MouseInputButton button = MouseInputButton.None,
            MouseInputGesture gesture = MouseInputGesture.None,
            MouseInputPhase phase = MouseInputPhase.None,
            MouseInputModifier shiftModifier = MouseInputModifier.None
            // AttackInputState state = AttackInputState.None
        )
        {
            _button = button;
            _gesture = gesture;
            _phase = phase;
            _shiftModifier = shiftModifier;
            // _state = state;
        }

        public void Reset()
        {
            _button = MouseInputButton.None;
            _gesture = MouseInputGesture.None;
            _phase = MouseInputPhase.None;
            _shiftModifier = MouseInputModifier.None;
            // _state = AttackInputState.None;
        }

        // 같은 입력인지 비교하는 요소는 총 3가지다.
        // 1. 어떤 버튼을 눌렀는가 (Left, Right)
        // 2. 어떤 제스처를 취했는가 (Tap, Hold)
        // 3. Shift를 누른 상태에서 입력했는가 (Shift, None)
        public readonly bool Equals(AttackInput other)
        {
            return _button == other._button
                && _gesture == other._gesture
                && _shiftModifier == other._shiftModifier;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(_button, _gesture, _shiftModifier);
        }

        /// <summary>표시용 이름 (예: "LC Hold Pressed + Shift"). 에디터/로그 용도라 매번 문자열을 생성한다.</summary>
        public override readonly string ToString()
        {
            string button = _button == MouseInputButton.LeftButton ? "LC" : _button == MouseInputButton.RightButton ? "RC" : "None";
            string shift = _shiftModifier == MouseInputModifier.Shift ? "Shift" : "None";
            return $"{button} {_gesture} {shift}";
        }
    }
}