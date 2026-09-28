using System.Collections.Generic;
using Game.Core.Events;
using Game.Player.Anim;
using UnityEngine;

namespace Game.Sword
{
    public class SwordAnimatorController : MonoBehaviour
    {
        [System.Serializable]
        private struct StateTrigger
        {
            public PlayerAnimState state;
            public string triggerName;
        }

        [Header("Rgithubeceive Event Channels")]
        [SerializeField] private PlayerAnimStateEventChannelSO _playerAnimStateChannel;

        [Header("State to Trigger Mappings")]
        [SerializeField] private StateTrigger[] _mappings;

        [SerializeField] private Animator _animator;

        private readonly Dictionary<PlayerAnimState, int> _animStateHashDict = new();

        public void Awake()
        {
            if (_animator == null)
            {
                Debug.LogError("Sword Animator is not assigned in the inspector.", this);
                return;
            }

            if (_playerAnimStateChannel == null)
            {
                Debug.LogError("Player Anim State Channel is not assigned in the inspector.", this);
                return;
            }

            if (_mappings == null || _mappings.Length == 0)
            {
                Debug.LogError("No state to trigger mappings are assigned in the inspector.", this);
            }
            else
            {
                foreach (var m in _mappings)
                {
                    _animStateHashDict[m.state] = Animator.StringToHash(m.triggerName);
                }
            }
        }

        private void OnEnable()
        {
            if (_playerAnimStateChannel != null) _playerAnimStateChannel.OnRaised += SetTrigger;
        }

        private void OnDisable()
        {
            if (_playerAnimStateChannel != null) _playerAnimStateChannel.OnRaised -= SetTrigger;
        }

        private void SetTrigger(PlayerAnimState state)
        {
            if (!_animStateHashDict.TryGetValue(state, out int hash)) 
            {
                return;
            }

            _animator.ResetTrigger(hash);
            _animator.SetTrigger(hash);
        }
    }
}