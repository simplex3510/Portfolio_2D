using UnityEngine;

namespace Game.Player.Attack
{
    // 개별 공격의 콘텐츠 데이터 (smash / thrust / spin 등)
    // 이 데이터를 늘리는 것이 "공격 추가" 작업의 전부가 되도록 유지한다
    [CreateAssetMenu(menuName = "ScriptableObject/Attack/Attack Data", fileName = "New Attack Data")]
    public class AttackDataSO : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _attackName;

        [Header("Animation Parameter")]
        [Tooltip("Animator에서 이 공격을 재생하기 위한 트리거 파라미터 이름")]
        [SerializeField] private string _animationTrigger;

        [Header("Timing (Only for Hold)")]
        [Tooltip("입력이 확정되어 공격이 요청된 시점(AttackInputCompleted 수신)부터, 이 시간이 지나면 공격이 발동한다.\n" +
                 "이 시간이 지나기 전에 입력이 취소(AttackInputCanceled)되면 공격은 취소된다.\n" +
                 "0이면 확정 즉시 발동한다. (Tap 공격은 0을 사용한다)")]
        [SerializeField] private float _executionThreshold;

        public string AttackName => _attackName;
        public string AnimationTrigger => _animationTrigger;
        public float ExecutionThreshold => _executionThreshold;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_executionThreshold < 0f)
            {
                Debug.LogWarning($"[{name}] ExecutionThreshold has to be a non-negative value.", this);
            }
        }
#endif
    }
}
