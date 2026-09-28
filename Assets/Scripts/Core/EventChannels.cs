using System;
using Game.Player.Anim;
using UnityEngine;

namespace Game.Core.Events
{
    /// <summary>
    /// 값이 없는 신호(예: 점프 시작)를 전달하는 이벤트 채널.
    /// Raise()를 호출하는 쪽과 OnRaised를 구독하는 쪽이 서로의 타입을 몰라도 됩니다.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Channels/Void Event Channel", fileName = "New Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public event Action OnRaised;

        public void Raise()
        {
            OnRaised?.Invoke();
        }
    }

    /// <summary>
    /// 연속값을 전달하는 이벤트 채널
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Channels/Float Event Channel", fileName = "New Float Event Channel")]
    public class FloatEventChannelSO : ScriptableObject
    {
        public event Action<float> OnRaised;

        public void Raise(float value) => OnRaised?.Invoke(value);
    }

    /// <summary>
    /// bool 값을 전달하는 이벤트 채널(예: 접지 상태, 이동 상태 변화).
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Channels/Bool Event Channel", fileName = "New Bool Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public event Action<bool> OnRaised;

        public void Raise(bool value) => OnRaised?.Invoke(value);
    }

    /// <summary>
    /// PlayerAnimState를 전달하는 이벤트 채널(예: 플레이어 애니메이션 상태 변화).
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Channels/Event Player Anim State Channel")]
    public class PlayerAnimStateEventChannelSO : ScriptableObject
    {
        public event Action<PlayerAnimState> OnRaised;
        public void Raise(PlayerAnimState state) => OnRaised?.Invoke(state);
    }
}