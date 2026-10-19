# decisions-overview.md

구현 결정 사실과 근거만 간략히 기록한다.

## 클라이언트 이동 예측·보정

| 결정 | 근거 |
|---|---|
| 범위는 내 캐릭터의 이동 예측과 보정까지. 상대 보간과 서버 본체는 제외 | 1순위 중 이동 예측·보정 단위를 먼저 닫기 위함 |
| 틱마다 (입력, 예측 후 상태)를 `PredictionHistory`에 기록하고, 스냅샷의 `LastProcessedInputTick`과 비교해 `MovementReconciler`가 보정. 서버 상태로 덮은 뒤 이후 입력을 재적용 | 서버 상태가 정답이므로 클라이언트 float 오차를 보정으로 흡수 |
| 입력이 없는 틱도 이력에 남기고 전송. 최근 N틱을 중복 포함 | 서버가 틱 순서를 알아야 하며 전송은 신뢰성이 없다고 가정 |
| 불일치 판정은 위치만 epsilon 비교, 나머지 필드는 완전 일치 | 런타임 차이의 float 오차로 불필요한 보정이 일어나는 것을 줄임 |
| 임시 서버 대역 `LoopbackMovementServer`로 통합 검증, 별도 `RuneBlade.Server` asmdef(참조는 Simulation·Network뿐, Unity 비의존)에 둔다. 실제 서버가 생기면 대체 | 서버(S단계)가 아직 없고, 이 어셈블리가 이후 실제 서버 로직의 자리가 됨 |
| 서버보다 앞선 틱(리드 틱)은 이번에 다루지 않음. 서버 대역은 클라이언트 틱 번호 순서로 처리 | 서버 고정 틱과 입력 도착 마감 규칙은 S단계에서 결정 |
| 어셈블리를 Simulation(규칙), Network(전송), Client(클라이언트 로직, Unity 비의존), Game(Unity 어댑터와 화면)으로 나눈다. 장치 입력 읽기만 Game, 입력 묶음·이력·예측·보정은 Client | 예측과 보정을 Unity 없이 테스트하고, 서버가 Simulation과 Network만 참조하게 하기 위함 |

## 미정·보류

| 항목 | 현재 방침 |
|---|---|
| epsilon 값 | `TODO(tuning)`, 실측으로 결정 |
| 예측 이력 용량 | 임시 128틱, `TODO(tuning)` |
| 중복 전송 틱 수 N | `TODO(tuning)` |
| 보정 시 화면 처리 | 우선 즉시 스냅, 부드럽게 보정은 보류 |
