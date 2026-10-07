using UnityEngine;

namespace Game.Sword.Anim
{
    public class SwordAnimStateBroadcaster : StateMachineBehaviour
    {
        [Header("Notify State")]
        [SerializeField] private SwordAnimState _enterState = SwordAnimState.None;
        [SerializeField] private SwordAnimState _exitState = SwordAnimState.None;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.TryGetComponent(out SwordAnimatorController controller))
            {
                controller.NotifyStateEntered(_enterState);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.TryGetComponent(out SwordAnimatorController controller))
            {
                controller.NotifyStateExited(_exitState);
            }
        }
    }
}