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
        private struct JudgementContext
        {
            // 판정이 확정되기 전까지는 계속 tracking 상태를 유지.
            // 판정은 JudgeAttackInput()에서 이루어지며, 판정이 확정되면 tracking 상태를 종료한다.
            public bool IsTracking { get; set; }
            public float PressTime { get; set; }
            public float ReleaseTime { get; set; }

            public void Reset()
            {
                PressTime = 0f;
                ReleaseTime = 0f;
                IsTracking = false;
            }
        }

        /// <summary>입력(AttackInput) → 공격(AttackDataSO) 할당표</summary>
        [SerializeField] private AttackMapSO _attackMap;

        // Tap/Hold를 구분하는 시간(초, unscaledTime 기준, press 기준). 모든 입력에 공통으로 적용된다.
        [Tooltip("press 후 이 시간 동안 유지하면 Hold, 그 전에 떼면 Tap으로 판정한다.")]
        [Min(0f)]
        [SerializeField] private float _holdConfirmedThreshold = 0.25f;

        /// <summary>
        /// 입력이 확정되어 해당 공격이 요청되었을 때 발행된다.
        /// 발행 시점: (None, Pressed) 누른 즉시 / (Hold, Pressed) Hold 시간 도달 / (Tap 또는 Hold, Released) 뗄 때
        /// </summary>
        public event Action<AttackDataSO> AttackInputConfirmed;

        /// <summary>
        /// 누르는 동안 확정되어 유지되던 공격의 입력이 끝났을 때 발행된다.
        /// (버튼을 뗐거나, 같은 press에서 다른 입력으로 전환된 경우)
        /// 뗄 때 확정되는 입력(Released)은 확정 즉시 끝난 것이므로 발행하지 않는다.
        /// 이 이벤트는 "입력이 끝났다"는 사실만 알린다.
        /// 예: Hold 공격을 뗄 때 발동할지 취소할지는 이 이벤트를 받은 PlayerAttackController가 ExecutionThreshold로 판단한다.
        /// </summary>
        public event Action<AttackDataSO> AttackInputExpired;

        /// <summary>Raw Input을 제공하는 PlayerInputReader. RequireComponent로 같은 오브젝트에서 가져온다.</summary>
        private PlayerInputReader _inputReader;

        // 누르는 동안 확정되어 유지 중인 공격.
        private AttackDataSO _attackData;
        private AttackInput _attackInput;
        private JudgementContext _judgementContext;

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

            ResetContext();
        }

        private void OnDisable()
        {
            if (_inputReader == null) return;

            _inputReader.MouseButtonPressed -= HandleMouseButtonPressed;
            _inputReader.MouseButtonReleased -= HandleMouseButtonReleased;
            
            // 비활성화 중에는 판정 상태를 남기지 않는다 (유지 중이던 공격은 취소 이벤트 없이 버린다)
            ResetContext();
        }

        private void Update()
        {
            TickTracking();
        }

        // 입력 재구성 시작
        private void HandleMouseButtonPressed(MouseInputButton button)
        {
            // 이미 입력을 tracking 중일 때,
            if (_judgementContext.IsTracking == true)
            {
                // 새로운 입력은 무시한다.
                return;
            }

            _attackInput = new AttackInput(
                /* 변경된 입력 */
                button: button,
                gesture: MouseInputGesture.None,
                phase: MouseInputPhase.Pressed,
                shiftModifier: _inputReader.IsShiftHeld ? MouseInputModifier.Shift : MouseInputModifier.None
            );

            _judgementContext.IsTracking = true;
            _judgementContext.PressTime = Time.unscaledTime;
        }

        private void HandleMouseButtonReleased(MouseInputButton button)
        {
            if (_judgementContext.IsTracking != true || _attackInput.Button != button)
            {
                return;
            }

            _attackInput = new AttackInput(
                /* 변경된 입력 */
                button: button,
                phase: MouseInputPhase.Released,

                /* 유지된 입력 */
                gesture: _attackInput.Gesture,
                shiftModifier: _attackInput.ShiftModifier
            );

            _judgementContext.ReleaseTime = Time.unscaledTime;
        }

        // ---------- 판정 ----------

        private void TickTracking()
        {
            if (_judgementContext.IsTracking == true)
            {
                JudgeGesture();
            }

        }

        // 최종적으로 gesture가 확정되는 시점.
        // 입력 시간에 따라서 Tap -> Hold로 변하도록 AttackInput을 갱신한다.
        private void JudgeGesture()
        {
            if (_attackInput.Phase == MouseInputPhase.Pressed)
            {
                // 이미 Hold로 판정된 입력은 더 이상 갱신하지 않는다.
                if (_attackInput.Gesture == MouseInputGesture.Hold) return;

                if (Time.unscaledTime - _judgementContext.PressTime >= _holdConfirmedThreshold)
                {
                    _attackInput = new AttackInput(
                        /* 변경된 입력 */
                        gesture: MouseInputGesture.Hold,

                        /* 유지된 입력 */
                        button: _attackInput.Button,
                        phase: _attackInput.Phase,
                        shiftModifier: _attackInput.ShiftModifier
                    );

                    // (Hold, Pressed) 발행 - Hold가 Press 되었으므로 Confirmed 이벤트 발행.
                    _attackMap.TryGetAttackData(_attackInput, out _attackData);
                    if (_attackData != null)
                    {
                        RaiseConfirmed(_attackData);
                    }
                }
                // else: 아직 Hold 시간에 도달하지 않았으므로, (None, Pressed) 상태를 유지한다.
            }
            else if (_attackInput.Phase == MouseInputPhase.Released)
            {
                if (_judgementContext.ReleaseTime - _judgementContext.PressTime < _holdConfirmedThreshold)
                {
                    _attackInput = new AttackInput(
                        /* 변경된 입력 */
                        gesture: MouseInputGesture.Tap,

                        /* 유지된 입력 */
                        button: _attackInput.Button,
                        phase: _attackInput.Phase,
                        shiftModifier: _attackInput.ShiftModifier
                    );

                    // (Tap, Pressed)는 발행될 수 없음.
                    // (Tap, Released) 발행 - Tap이 Release 되었으므로 Confirmed 이벤트 발행.
                    _attackMap.TryGetAttackData(_attackInput, out _attackData);
                    if (_attackData != null)
                    {
                        RaiseConfirmed(_attackData);
                    }
                }
                // else: Hold로 판정되었으므로, (Hold, Released) Expired 이벤트 발행 
                else
                {
                    // 논리적으로 Hold로 판정되었으나 아직 Hold로 갱신되지 않은 상태일 경우                    
                    if (_attackInput.Gesture != MouseInputGesture.Hold)
                    {
                        _attackInput = new AttackInput(
                            /* 변경된 입력 */
                            gesture: MouseInputGesture.Hold,
                            phase: MouseInputPhase.Pressed,

                            /* 유지된 입력 */
                            button: _attackInput.Button,
                            shiftModifier: _attackInput.ShiftModifier
                        );

                        // Hold가 확정된 (None, Pressed) 발행
                        // Confirmed 이벤트를 발행하지 못했으므로, Confirmed 이벤트를 먼저 발행한다.
                        _attackMap.TryGetAttackData(_attackInput, out _attackData);
                        if (_attackData != null)
                        {
                            RaiseConfirmed(_attackData);
                        }
                    }

                    _attackInput = new AttackInput(
                        /* 변경된 입력 */
                        gesture: MouseInputGesture.Hold,
                        phase: MouseInputPhase.Released,

                        /* 유지된 입력 */
                        button: _attackInput.Button,
                        shiftModifier: _attackInput.ShiftModifier
                    );

                    // (Hold, Released) 발행 - Hold가 Release 되었으므로 Expired 이벤트 발행.
                    _attackMap.TryGetAttackData(_attackInput, out _attackData);
                    if (_attackData != null)
                    {
                        RaiseExpired(_attackData);
                    }
                }

                // tracking을 종료하고, 판정 context를 초기화한다.
                ResetContext();
            }
        }

        private void RaiseConfirmed(AttackDataSO attackData)
        {
            if (attackData == null)
            {
                Debug.LogError("AttackInputReader: Confirmed attackData is null. This should not happen.", this);
                return;
            }
            AttackInputConfirmed?.Invoke(attackData);
            LogAttackEvent("Confirmed", attackData);
        }

        private void RaiseExpired(AttackDataSO attackData)
        {
            if (attackData == null)
            {
                Debug.LogError("AttackInputReader: Expired attackData is null. This should not happen.", this);
                return;
            }

            AttackInputExpired?.Invoke(attackData);
            LogAttackEvent("Expired", attackData);
        }

        private void ResetContext()
        {
            _judgementContext.Reset();
            _attackInput.Reset();
            _attackData = null;
        }

        // 에디터에서만 호출되는 디버그 로그 (빌드에서는 호출 자체가 제거된다)
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void LogAttackEvent(string eventName, AttackDataSO attackData)
        {
            Debug.Log($"AttackInputReader: {eventName} attackData={attackData.name}, input={_attackInput.Button}, gesture={_attackInput.Gesture}, phase={_attackInput.Phase}, shift={_attackInput.ShiftModifier}", this);
        }
    }
}