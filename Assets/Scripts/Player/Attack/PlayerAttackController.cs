using UnityEngine;

using Game.Core.Events;
using Game.Input.Attack;

namespace Game.Player.Attack
{
    [RequireComponent(typeof(PlayerAttackInputReader))]
    public class PlayerAttackController : MonoBehaviour
    {
        // 공격 진행 단계 (입력 신호와는 별개)
        private enum AttackPhase
        {
            Idle,       // 대기
            Pending,    // 보류, Tap-Hold 판정 진행 중 (press ~ HoldDetectionThreshold)
            Windup,     // Hold 확정 + 선 딜레이 재생 중 (Hold 확정 ~ ExecutionThreshold)
            Executing,  // 공격 발동 (Executed ~ Ended)
        }

        [Header("References")]
        [SerializeField] private AttackMapSO _attackMap;

        [Header("Send Event Channels")]
        [SerializeField] private PlayerAttackEventChannelSO _attackChannel;

        private void Awake()
        {

        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }
    }
}