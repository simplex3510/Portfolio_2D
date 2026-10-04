using UnityEngine;

using Game.Core.Events;
using Game.Input.Attack;

namespace Game.Player.Attack
{
    [RequireComponent(typeof(PlayerAttackInputReader))]
    public class PlayerAttackController : MonoBehaviour
    {
        // [Header("References")]

        private PlayerAttackInputReader _attackInputReader;

        private Animator _animator;

        private void Awake()
        {
            _attackInputReader = GetComponent<PlayerAttackInputReader>();
        }

        private void OnEnable()
        {
            _attackInputReader.AttackInputConfirmed += HandleAttackInputConfirmed;
        }

        private void OnDisable()
        {
            _attackInputReader.AttackInputConfirmed -= HandleAttackInputConfirmed;
        }

        private void HandleAttackInputConfirmed(AttackDataSO attackData)
        {
            
        }
    }
}