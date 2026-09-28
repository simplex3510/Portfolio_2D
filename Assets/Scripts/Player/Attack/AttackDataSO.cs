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

        [Header("Animation")]
        [Tooltip("Animator에서 이 공격을 재생하기 위한 트리거 파라미터 이름")]
        [SerializeField] private string _animationTrigger;

        [Header("Timing")]
        [Tooltip("press 시점 기준, 이 시간에 도달하면 공격이 확정 발동된다. " +
                 "Tap 공격은 0에 가까운 값(또는 0)을 사용한다.")]
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