using UnityEngine;

using RuneBlade.Simulation;

namespace RuneBlade.Game
{
    /// <summary>
    /// 플레이 모드 테스트용. 화살표 입력을 모아 60Hz로 PlayerMovement를 진행하고 위치를 transform에 반영한다.
    /// </summary>
    [RequireComponent(typeof(PlayerInputReader))]
    public class PlayerMovementController : MonoBehaviour
    {
        // 렌더 프레임이 느려도 시뮬레이션은 이 간격으로 따라잡되, 한 프레임에 진행하는 틱 수는 제한한다.
        private const float TickDurationSeconds = 1f / 60f;
        private const int MaxTicksPerFrame = 5;

        [Header("Movement Settings")]
        [SerializeField] private PlayerMovementSettingSO _movementSettingSO;

        private MovementSimulation _movement;
        private MovementSettings _settings;

        private PlayerInputReader _inputReader;
        private float _tickAccumulator;

        private void Awake()
        {
            _inputReader = GetComponent<PlayerInputReader>();

            if (_inputReader == null)
            {
                Debug.LogError("PlayerInputReader가 연결되지 않았습니다.", this);
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            if (_movementSettingSO == null)
            {
                Debug.LogError("PlayerMovementSettingSO가 연결되지 않았습니다.", this);
                enabled = false;
                return;
            }

            // 매치 중 값이 바뀌면 예측과 서버 결과가 어긋나므로 시작 시 복사해서 고정한다.
            _settings = _movementSettingSO.Settings;
            _movement = MovementSimulation.CreateOnGround(0f);
            ApplyToTransform();
        }

        private void Update()
        {
            _tickAccumulator += Time.deltaTime;

            int ticksThisFrame = 0;
            while (_tickAccumulator >= TickDurationSeconds && ticksThisFrame < MaxTicksPerFrame)
            {
                _tickAccumulator -= TickDurationSeconds;
                ticksThisFrame++;
                RunTick();
            }

            // 상한에 걸려 못 따라잡은 시간은 버린다(일시 정지·긴 프레임 뒤의 폭주 방지).
            if (ticksThisFrame == MaxTicksPerFrame)
            {
                _tickAccumulator = 0f;
            }
        }

        private void RunTick()
        {
            TickInput input = _inputReader.GetTickInput();

            _movement.Tick(input, _settings);
            ApplyToTransform();
        }

        private void ApplyToTransform()
        {
            transform.position = new Vector3(_movement.PositionX, _movement.PositionY, transform.position.z);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10f, 10f, 800f, 24f), _movement.ToString());
        }
    }
}
