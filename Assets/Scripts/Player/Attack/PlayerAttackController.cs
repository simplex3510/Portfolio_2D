using UnityEngine;

using Game.Core.Events;
using Game.Player.Input;

namespace Game.Player.Attack
{
    [RequireComponent(typeof(PlayerInputReader))]
    public class PlayerAttackController : MonoBehaviour
    {
        private enum AttackInputState
        {
            Idle,       // 대기
            Pending,    // 보류, Tap-Hold 판정 진행 중 (press ~ HoldDetectionThreshold)
            Windup,     // Hold 확정 + 선 딜레이 재생 중 (HoldDetectionThreshold ~ ExecutionThreshold)
            Executing,  // 공격 발동 (Executed ~ Ended)
        }

        [Header("References")]
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private PlayerAttackEventChannelSO _eventChannel;
        [SerializeField] private AttackMapSO _attackMap;

        [Header("Timing")]
        [Tooltip("Tap/Hold를 구분하는 InputAction 판정 길이(시간) (전역, press 기준)")]
        [SerializeField] private float _holdDetectionThreshold = 0.15f;

        // 런타임 상태
        private AttackInputState _state = AttackInputState.Idle;
        private float _pressElapsedTime;
        private bool _modifierLatched;
        private MouseButtonType _pressedButton;
        private AttackDataSO _currentAttackData;

        private void Awake()
        {
            _inputReader = GetComponent<PlayerInputReader>();
            if (_inputReader == null || _eventChannel == null || _attackMap == null)
            {
                Debug.LogError("No required references are assigned to PlayerAttackController.", this);
                enabled = false;
                return;
            }

            _attackMap.InitializeAttackDictionary();
        }

        private void OnEnable()
        {
            _inputReader.OnLeftClickAttackPressed += HandleLeftClickAttackPressed;
            _inputReader.OnLeftClickAttackReleased += HandleLeftClickAttackReleased;

            _inputReader.OnAttackModifierPressed += HandleAttackModifierPressed;
        }

        private void OnDisable()
        {
            _inputReader.OnLeftClickAttackPressed -= HandleLeftClickAttackPressed;
            _inputReader.OnLeftClickAttackReleased -= HandleLeftClickAttackReleased;

            _inputReader.OnAttackModifierPressed -= HandleAttackModifierPressed;
        }

        private void Update()
        {
            switch (_state)
            {
                case AttackInputState.Pending:
                    TickPending();
                    break;
                case AttackInputState.Windup:
                    TickWindup();
                    break;
            }
        }

        #region Input Callbacks
        private void HandleLeftClickAttackPressed()
        {
            // 이미 판정/공격 진행 중이면 새 입력을 무시한다 (버퍼링/캔슬은 다루지 않음)
            if (_state != AttackInputState.Idle)
            {
                return;
            }

            _pressedButton = MouseButtonType.Left;
            _pressElapsedTime = 0f;
            _modifierLatched = _inputReader.IsAttackModifierHeld;
            _state = AttackInputState.Pending;
        }

        private void HandleLeftClickAttackReleased()
        {
            if (_state == AttackInputState.Pending)
            {
                ResolveTap();
            }
            else if (_state == AttackInputState.Windup)
            {
                CancelAttack();
            }
            // Executing 중 release는 무시 (발동판정값 이후이므로 진행 중인 공격 유지)
        }

        private void HandleAttackModifierPressed()
        {
            // 판정 창이 끝난 뒤의 Shift 입력은 결과에 영향을 주지 않는다
            if (_state == AttackInputState.Pending)
            {
                _modifierLatched = true;
            }
        }
        #endregion

        #region State Ticks
        private void TickPending()
        {
            _pressElapsedTime += Time.deltaTime;

            if (_pressElapsedTime >= _holdDetectionThreshold)
            {
                ResolveHold();
            }
        }

        private void TickWindup()
        {
            _pressElapsedTime += Time.deltaTime;

            if (_pressElapsedTime >= _currentAttackData.ExecutionThreshold)
            {
                ExecuteAttack();
            }
        }
        #endregion

        #region Resolution
        private void ResolveTap()
        {
            var key = new AttackInputKey(_pressedButton, MousePressType.Tap, _modifierLatched);
            var data = _attackMap.GetAttackData(key);

            if (data == null)
            {
                // 정의되지 않은 슬롯: 아무 일도 일어나지 않는다
                _state = AttackInputState.Idle;
                return;
            }

            _currentAttackData = data;
            _eventChannel.RaiseStarted(_currentAttackData);
            ExecuteAttack();
        }

        private void ResolveHold()
        {
            var key = new AttackInputKey(_pressedButton, MousePressType.Hold, _modifierLatched);
            var data = _attackMap.GetAttackData(key);

            if (data == null)
            {
                _state = AttackInputState.Idle;
                return;
            }

            _currentAttackData = data;
            _pressElapsedTime = 0f; // Windup 구간 경과 시간으로 재사용 (기준점: HoldDetectionThreshold 도달 시점)
            _eventChannel.RaiseStarted(_currentAttackData);
            _state = AttackInputState.Windup;
        }

        private void ExecuteAttack()
        {
            _eventChannel.RaiseExecuted(_currentAttackData);
            _state = AttackInputState.Executing;
        }

        private void CancelAttack()
        {
            _eventChannel.RaiseCanceled(_currentAttackData);
            ResetToIdle();
        }

        private void ResetToIdle()
        {
            _currentAttackData = null;
            _state = AttackInputState.Idle;
        }
        #endregion

        // 애니메이션(또는 다른 외부 시스템)이 공격 종료를 알릴 때 호출한다
        // 예: SwordAnimatorController가 애니메이션 이벤트로 이 메서드를 호출
        public void NotifyAttackEnded()
        {
            if (_state != AttackInputState.Executing)
            {
                return;
            }

            var finishedData = _currentAttackData;
            ResetToIdle();
            _eventChannel.RaiseEnded(finishedData);
        }
    }
}