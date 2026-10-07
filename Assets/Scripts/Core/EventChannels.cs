using System;
using Game.Player.Anim;
using Game.Player.Attack;
using UnityEngine;

namespace Game.Core.Events
{
    /// <summary>
    /// 값이 없는 신호(예: 점프 시작)를 전달하는 이벤트 채널.
    /// Raise()를 호출하는 쪽과 OnRaised를 구독하는 쪽이 서로의 타입을 몰라도 됩니다.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/EventsChannels/Void Event Channel", fileName = "New Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public event Action OnRaised;

        public void Raise() => OnRaised?.Invoke();
    }

    /// <summary>
    /// 연속값을 전달하는 이벤트 채널
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/EventsChannels/Float Event Channel", fileName = "New Float Event Channel")]
    public class FloatEventChannelSO : ScriptableObject
    {
        public event Action<float> OnRaised;

        public void Raise(float value) => OnRaised?.Invoke(value);
    }

    [CreateAssetMenu(menuName = "ScriptableObject/EventsChannels/Int Event Channel", fileName = "New Int Event Channel")]
    public class IntEventChannelSO : ScriptableObject
    {
        public event Action<int> OnRaised;

        public void Raise(int value) => OnRaised?.Invoke(value);
    }

    /// <summary>
    /// bool 값을 전달하는 이벤트 채널(예: 접지 상태, 이동 상태 변화).
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObject/EventsChannels/Bool Event Channel", fileName = "New Bool Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public event Action<bool> OnRaised;

        public void Raise(bool value) => OnRaised?.Invoke(value);
    }

    // 플레이어 공격 상태 변화를 전파하는 이벤트 채널
    // 발행자(PlayerAttackController)는 구독자(SwordAnimatorController, VFX 등)를 직접 알지 못한다
    [CreateAssetMenu(menuName = "ScriptableObject/EventsChannels/Player Attack Event Channel", fileName = "New Player Attack Event Channel")]
    public class PlayerAttackEventChannelSO : ScriptableObject
    {
        // 공격 확정 시점 (Tap: 즉시 / Hold: HoldDetectionThreshold 도달 시, Windup 시작)
        public event Action<AttackDataSO> Started;

        // 공격 발동 시점 (Tap: Started 직후 / Hold: ExecutionThreshold 도달 시)
        public event Action<AttackDataSO> Executed;

        // 공격 정상 종료
        public event Action Ended;

        // Hold 공격이 Windup 중 release되어 취소됨
        public event Action<AttackDataSO> Canceled;

        public void RaiseStarted(AttackDataSO data) => Started?.Invoke(data);
        public void RaiseExecuted(AttackDataSO data) => Executed?.Invoke(data);
        public void RaiseCanceled(AttackDataSO data) => Canceled?.Invoke(data);
        public void RaiseEnded() => Ended?.Invoke();
    }
}