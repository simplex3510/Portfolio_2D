using System;
using UnityEngine;

using Game.Player.Attack;

namespace Game.Input.Attack
{
    /// <summary>
    /// 공격 입력 판정기.
    /// (1) PlayerInputReader의 raw 입력(버튼 Press/Release, Shift 상태)을 받아
    /// (2) Tap/Hold를 판정해 가공된 입력(AttackInput)을 만들고, AttackMapSO에서 대응하는 공격(AttackDataSO)을 찾아
    /// (3) PlayerAttackController에게 이벤트로 넘겨준다.
    /// "어떤 입력이 어떤 공격인가"까지만 결정하며, 공격을 실제로 어떻게 처리할지
    /// (예: Hold 공격을 언제 발동하고 언제 취소할지)는 PlayerAttackController가 결정한다.
    ///
    /// 두 마우스 버튼을 동시에 판정하지 않는다.
    /// 한 버튼을 판정하는 동안 다른 버튼의 press/release는 무시한다.
    /// </summary>
    [RequireComponent(typeof(PlayerInputReader))]
    public class PlayerAttackInputReader : MonoBehaviour
    {
        private struct JudgementState
        {
            public bool PressSignaled { get; private set; }
            public bool ReleaseSignaled { get; private set; }
            public bool IsTracking { get; private set; }
            public float PressTime { get; private set; }
            public float ReleaseTime { get; private set; }

            public void Reset()
            {
                PressSignaled = false;
                ReleaseSignaled = false;
                PressTime = 0f;
                ReleaseTime = 0f;
                IsTracking = false;
            }

            public void SetPressSignal(float time)
            {
                PressSignaled = true;
                PressTime = time;
            }

            public void SetReleaseSignal(float time)
            {
                ReleaseSignaled = true;
                ReleaseTime = time;
            }

            public void ConsumePressSignal()
            {
                if (PressSignaled)
                {
                    PressSignaled = false;
                    IsTracking = true;
                }
            }

            public void ConsumeReleaseSignal()
            {
                if (ReleaseSignaled)
                {
                    ReleaseSignaled = false;
                    IsTracking = false;
                }
            }

        }

        /// <summary>입력(AttackInput) → 공격(AttackDataSO) 할당표</summary>
        [SerializeField] private AttackMapSO _attackMap;

        // Tap/Hold를 구분하는 시간(초, unscaledTime 기준, press 기준). 모든 입력에 공통으로 적용된다.
        [Tooltip("press 후 이 시간 동안 유지하면 Hold, 그 전에 떼면 Tap으로 판정한다.")]
        [Min(0f)]
        [SerializeField] private float _holdDetectionThreshold = 0.25f;

        /// <summary>
        /// 입력이 확정되어 해당 공격이 요청되었을 때 발행된다.
        /// 발행 시점: (None, Pressed) 누른 즉시 / (Hold, Pressed) Hold 시간 도달 / (Tap 또는 Hold, Released) 뗄 때
        /// </summary>
        public event Action<AttackDataSO> AttackInputConfirmed;

        /// <summary>
        /// 누르는 동안 확정되어 유지되던 공격의 입력이 끝났을 때 발행된다.
        /// (버튼을 뗐거나, 같은 press에서 다른 입력으로 전환된 경우)
        /// 뗄 때 확정되는 입력(Released)은 확정 즉시 끝난 것이므로 발행하지 않는다.
        /// 이 이벤트는 "입력이 끝났다"는 사실만 알린다. 예: Hold 공격을 뗄 때 발동할지 취소할지는
        /// 이 이벤트를 받은 PlayerAttackController가 ExecutionThreshold로 판단한다.
        /// </summary>
        public event Action<AttackDataSO> AttackInputExpired;

        /// <summary>Raw Input을 제공하는 PlayerInputReader. RequireComponent로 같은 오브젝트에서 가져온다.</summary>
        private PlayerInputReader _inputReader;

        // 누르는 동안 확정되어 유지 중인 공격. 입력이 끝나면 AttackInputCanceled로 알린다.
        private AttackDataSO _activeAttack;

        private AttackInput _attackInput;
        private JudgementState _judgementState;

        private void Awake()
        {
            _inputReader = GetComponent<PlayerInputReader>();

            if (_inputReader == null || _attackMap == null)
            {
                Debug.LogError("No required references are assigned to PlayerAttackInputReader.", this);
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            if (_inputReader == null) return;

            _inputReader.MouseButtonPressed += HandleMouseButtonPressed;
            _inputReader.MouseButtonReleased += HandleMouseButtonReleased;
        }

        private void OnDisable()
        {
            if (_inputReader != null)
            {
                _inputReader.MouseButtonPressed -= HandleMouseButtonPressed;
                _inputReader.MouseButtonReleased -= HandleMouseButtonReleased;
            }

            // 비활성화 중에는 판정 상태를 남기지 않는다 (유지 중이던 공격은 취소 이벤트 없이 버린다)
            ResetJudgement();
        }

        private void Update()
        {
            TickButton();
        }

        // 입력 재구성 시작
        private void HandleMouseButtonPressed(MouseInputButton button)
        {
            // 이미 어떤 버튼을 판정 중이면(같은 프레임에 아직 처리되지 않은 press 포함) 새 press는 무시한다.
            // _attackInput.Button은 press 콜백에서 정해지고 판정이 끝날 때 None으로 돌아간다.
            if (_attackInput.Button != MouseInputButton.None)
            {
                Debug.LogWarning($"AttackInputReader: Button {button} pressed while tracking {_attackInput.Button}. Ignoring.");
                return;
            }

            // 해당 시점에서의 입력 재구성
            _attackInput = new AttackInput(
                /* 변경된 입력 */
                button: button,
                phase: MouseInputPhase.Pressed,
                shiftModifier: _inputReader.IsShiftHeld ? MouseInputModifier.Shift : MouseInputModifier.None,

                /* 미할당 */
                gesture: MouseInputGesture.None
            );

            _judgementState.SetPressSignal(Time.unscaledTime);
        }

        private void HandleMouseButtonReleased(MouseInputButton button)
        {
            // 판정 중인 버튼의 release만 처리한다.
            // press 없이 들어온 release와, 무시된 다른 버튼의 release는 여기서 걸러진다.
            if (button != _attackInput.Button)
            {
                Debug.LogWarning($"AttackInputReader: Button {button} released but it is not the tracked button ({_attackInput.Button}). Ignoring.");
                return;
            }

            // 해당 시점에서의 입력 재구성
            _attackInput = new AttackInput(
                /* 변경된 입력 */
                button: button,
                phase: MouseInputPhase.Released,

                /* 유지된 입력 */
                gesture: _attackInput.Gesture,
                shiftModifier: _attackInput.ShiftModifier
            );

            _judgementState.SetReleaseSignal(Time.unscaledTime);
        }

        // ---------- 판정 ----------

        private void TickButton()
        {
            if (_judgementState.PressSignaled)
            {
                BeginTracking();
                _judgementState.ConsumePressSignal();
            }

            // release를 Hold 시간 검사보다 먼저 처리한다.
            // 같은 프레임에 Hold 시간 도달과 release가 겹치면 Tap으로 판정된다(Tap 우대).
            if (_judgementState.ReleaseSignaled)
            {
                EndTracking();
                _judgementState.ConsumeReleaseSignal();
            }

            if (_judgementState.IsTracking == true)
            {
                CheckGesture();
            }

        }

        // press 즉시 판정을 시작한다.
        private void BeginTracking()
        {
            // press 즉시의 입력. 아직 Tap/Hold를 알 수 없으므로 제스처는 None이다.
            _attackInput = new AttackInput(
                /* 변경된 입력 */
                phase: MouseInputPhase.Pressed,
                gesture: MouseInputGesture.None,

                /* 유지된 입력 */
                button: _attackInput.Button,
                shiftModifier: _attackInput.ShiftModifier
            );

            // (None, Pressed): 누른 즉시 발동하는 입력 (예: Parry)
            if (_attackMap.TryGetAttackData(_attackInput, out AttackDataSO attackData))
            {
                _activeAttack = attackData;
                RaiseConfirmed(attackData);
            }
        }

        private void EndTracking()
        {
            // 누르는 동안 유지되던 공격이 있으면 입력이 끝났음을 알린다
            CancelActiveAttack();

            // Hold가 아닌 경우는 다음과 같다.
            // 1. None: 같은 프레임에 press와 release가 겹치면 Tap으로 판정된다(Tap 우대).
            // 2. Tap: Hold에 도달하지 않은 상태에서 release가 발생하면 Tap으로 판정된다.
            MouseInputGesture gesture = _attackInput.Gesture == MouseInputGesture.Hold
                ? MouseInputGesture.Hold
                : MouseInputGesture.Tap;

            _attackInput = new AttackInput(
                /* 변경된 입력 */
                gesture: gesture,
                phase: MouseInputPhase.Released,

                /* 유지된 입력 */
                button: _attackInput.Button,
                shiftModifier: _attackInput.ShiftModifier
            );

            // (Tap/Hold, Released): 뗄 때 확정되는 입력. 확정 즉시 끝나므로 유지 중인 공격으로 기록하지 않는다.
            if (_attackMap.TryGetAttackData(_attackInput, out AttackDataSO attackData))
            {
                RaiseConfirmed(attackData);
            }

            ResetJudgement();
        }

        // 입력 시간에 따라서 Tap -> Hold로 변하도록 AttackInput을 갱신한다.
        private void CheckGesture()
        {
            // 이미 Hold로 확정되었으면 더 검사하지 않는다
            if (_attackInput.Gesture == MouseInputGesture.Hold) return;

            if (Time.unscaledTime - _judgementState.PressTime < _holdDetectionThreshold)
            {
                // Hold 시간 전: Tap 후보. 확정은 release 때 한다.
                _attackInput = new AttackInput(
                    /* 변경된 입력 */
                    gesture: MouseInputGesture.Tap,

                    /* 유지된 입력 */
                    button: _attackInput.Button,
                    phase: _attackInput.Phase,
                    shiftModifier: _attackInput.ShiftModifier
                );

                return;
            }

            ConfirmHold();
        }

        // 누른 채 Hold 시간에 도달한 순간의 입력을 확정한다.
        private void ConfirmHold()
        {
            _attackInput = new AttackInput(
                /* 변경된 입력 */
                gesture: MouseInputGesture.Hold,
                phase: MouseInputPhase.Pressed,

                /* 유지된 입력 */
                button: _attackInput.Button,
                shiftModifier: _attackInput.ShiftModifier
            );

            // (Hold, Pressed): 누른 채 Hold 시간에 도달한 순간 (예: Hold 공격의 Windup 시작)
            // 할당된 공격이 없으면 유지 중이던 공격은 그대로 두고, Hold 확정만 기록한다.
            if (_attackMap.TryGetAttackData(_attackInput, out AttackDataSO attackData) == false)
                return;

            // 유지 중이던 공격(예: (None, Pressed))이 있으면 먼저 종료한 뒤 Hold의 공격으로 전환한다
            CancelActiveAttack();

            _activeAttack = attackData;
            RaiseConfirmed(attackData);
        }

        // ---------- 보조 ----------

        private void RaiseConfirmed(AttackDataSO attackData)
        {
            AttackInputConfirmed?.Invoke(attackData);
            LogAttackEvent("Confirmed", attackData);
        }

        private void CancelActiveAttack()
        {
            if (_activeAttack == null) return;

            AttackDataSO attackData = _activeAttack;
            _activeAttack = null;

            AttackInputExpired?.Invoke(attackData);
            LogAttackEvent("Canceled", attackData);
        }

        private void ResetJudgement()
        {
            _judgementState.Reset();
            _attackInput.Reset();
            _activeAttack = null;
        }

        // 에디터에서만 호출되는 디버그 로그 (빌드에서는 호출 자체가 제거된다)
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void LogAttackEvent(string eventName, AttackDataSO attackData)
        {
            Debug.Log($"AttackInputReader: {eventName} attackData={attackData.name}, input={_attackInput.Button}, gesture={_attackInput.Gesture}, phase={_attackInput.Phase}, shift={_attackInput.ShiftModifier}", this);
        }
    }
}