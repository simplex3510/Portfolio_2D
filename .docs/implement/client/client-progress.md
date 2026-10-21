# client-progress.md

클라이언트의 구현 개발의 진행 상황을 다루는 문서다.
결정 사항과 근거는 `.docs/implement/decisions/decisions-overview.md`에 있다.

## 범위

내 캐릭터의 이동 예측과 보정까지. 상대 보간과 서버 본체는 제외한다.

## 어셈블리 구조

| 어셈블리 | Unity | 역할 |
|---|---|---|
| Simulation | 없음 | 규칙. 입력과 상태로 다음 상태를 계산 |
| Network | 없음 | 메시지 형식과 전송 통로 |
| Client | 없음 | 클라이언트 로직(틱 구동, 이력, 예측, 보정) |
| Game | 있음 | 장치 입력 읽기, 화면 반영 |
| Server | 없음 | 임시 서버 대역. asmdef 생성됨, 참조는 아직 비어 있음(Simulation, Network 연결 필요) |

## 단계 진행

| 단계 | 내용 | 상태 |
|---|---|---|
| 1 | 입력 수집기 분리 (`LocalInputCollector`, `PlayerInputReader`) | 완료 |
| 2 | 예측 이력 (`PredictionHistory`) | 완료 |
| 3 | 보정기 (`MovementReconciler`, `ReconcileResult`) | 완료 |
| 4 | 클라이언트 세션 (`ClientMovementSession`) + `MovementSettings.Validate()` | 다음 |
| 5 | 임시 서버 대역 (`LoopbackMovementServer`, `RuneBlade.Server`) | 대기 |
| 6 | 통합 EditMode 테스트 (`UnreliableTransport` + 세션 + 서버 대역) | 대기 |
| 7 | 컨트롤러를 View로 축소, `Game→Network` 참조, HUD | 대기 |

## 현재 상태

- `PlayerMovementController`는 아직 로컬 전용이다(자체 틱 구동과 `MovementSimulation` 소유). 단계 4 이후 세션으로 옮긴다.
- 단계 3까지의 EditMode 테스트는 모두 통과했다.

## 단계 4 착수 시 정할 것 (권장안, 미확정)

| 항목 | 권장 |
|---|---|
| 세션의 입력 수집 | 세션이 `LocalInputCollector`를 받아 쓰고 `PlayerInputReader`가 같은 수집기에 넣는다 |
| 틱 구동 | `Advance(float deltaSeconds)`. `TickDurationSeconds`, `MaxTicksPerFrame`을 컨트롤러에서 Client로 이동 |
| 중복 전송 틱 수 N | 임시 4~8, `TODO(tuning)` |
| 틱 번호 시작값 | 0 |
| 스냅샷 수신 | 큐를 모두 비우고 가장 큰 `LastProcessedInputTick` 하나만 보정기에 전달 |
| 노출할 값 | 현재 `MovementSimulation`, 마지막 보정 결과, 누적 보정 횟수 |

## 남은 확인·보류

- 튜닝 대상: 보정 오차(0.001), 이력 용량(128틱), 중복 전송 틱 수 N
- 보정 시 화면 처리: 즉시 스냅(부드럽게 보정은 보류)
- 단계 5 직전: `RuneBlade.Server` asmdef 설정 확인(사용자 확인 필요)
- 단계 7: `RuneBlade.Game`에 `RuneBlade.Network` 참조 추가, 씬 컨트롤러의 Inspector 연결 확인(사용자 작업)
- 늦게 도착하거나 건너뛴 입력 틱의 처리 정책은 서버 단계에서 결정
- 예약 비트 이벤트가 `Tick()`을 조용히 통과하는 것의 의도 확인 필요
