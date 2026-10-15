using UnityEngine;

using RuneBlade.Simulation;

namespace RuneBlade.Game
{
    /// <summary>MovementSettings를 Inspector에서 조정하는 에셋.</summary>
    [CreateAssetMenu(fileName = "PlayerMovementSetting", menuName = "RuneBlade SO/Player Movement Setting")]
    public class PlayerMovementSettingSO : ScriptableObject
    {
        // 필드 초기화 구문은 에셋을 새로 만들 때 기본값으로 적용된다.
        [SerializeField] private MovementSettings _settings = MovementSettings.CreateDefault();

        public MovementSettings Settings => _settings;
    }
}
