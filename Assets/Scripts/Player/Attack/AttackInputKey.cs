using System;
using UnityEngine;

namespace Game.Player.Attack
{
    // 어떤 마우스 버튼을 사용했는지
    public enum MouseButtonType
    {
        Left,
        Right,
    }

    // HoldDetectionThreshold 결과, Tap인지 Hold인지
    public enum MousePressType
    {
        Tap,
        Hold,
    }

    // 공격 슬롯을 식별하는 키값
    // 어떤 공격인지는 모르며, 순수하게 입력 조합만 표현
    [Serializable]
    public struct AttackInputKey : IEquatable<AttackInputKey>
    {
        [SerializeField] private MouseButtonType _buttonType;
        [SerializeField] private MousePressType _pressType;
        [SerializeField] private bool _withModifier;

        public readonly MouseButtonType Button => _buttonType;
        public readonly MousePressType PressType => _pressType;
        public readonly bool WithModifier => _withModifier;

        public AttackInputKey(MouseButtonType button, MousePressType pressType, bool withModifier)
        {
            _buttonType = button;
            _pressType = pressType;
            _withModifier = withModifier;
        }

        // Boxing 없이 Equals를 호출하기 위해 IEquatable<T> 구현
#region IEquatable interface Implementation
        public bool Equals(AttackInputKey other)
        {
            return _buttonType == other._buttonType
                && _pressType == other._pressType
                && _withModifier == other._withModifier;
        }
#endregion

#region Object Class Overrides
        public override bool Equals(object obj)
        {
            return obj is AttackInputKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_buttonType, _pressType, _withModifier);
        }

        public override string ToString()
        {
            return $"{_buttonType}_{_pressType}{(_withModifier ? "_Modifier" : string.Empty)}";
        }
#endregion
    }
}