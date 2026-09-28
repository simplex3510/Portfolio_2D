using UnityEngine;

namespace Game.Player.Anim
{
    public class PlayerAnimStateBroadcaster : StateMachineBehaviour
    {
        [SerializeField] private PlayerAnimState _state;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.TryGetComponent(out PlayerAnimatorController controller))
            {
                controller.NotifyStateEntered(_state);
            }
        }
    }
}