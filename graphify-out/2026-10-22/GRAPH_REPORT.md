# Graph Report - Portfolio_2D  (2026-10-22)

## Corpus Check
- 52 files · ~43,737 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 178 file(s) not represented in the graph (top: .meta 117, .asset 35, .anim 10)

## Summary
- 939 nodes · 1651 edges · 56 communities (48 shown, 8 thin omitted)
- Extraction: 87% EXTRACTED · 13% INFERRED · 0% AMBIGUOUS · INFERRED: 212 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `5e0b0448`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- manifest.json
- dependencies
- com.unity.modules.umbra
- com.unity.modules.subsystems
- MovementSimulation
- Test
- @PlayerInputActions
- packages-lock.json
- com.unity.test-framework
- com.unity.render-pipelines.core
- RuneBlade.Simulation
- InMemoryTransport
- com.unity.modules.physics
- com.unity.modules.imgui
- com.unity.2d.tilemap
- dependencies
- 서버 권위 멀티플레이 환경 구현 방향성 참고 문서
- TickInput
- com.unity.ext.nunit
- com.unity.modules.imageconversion
- com.unity.modules.animation
- dependencies
- dependencies
- com.unity.burst
- com.unity.modules.physics2d
- com.unity.2d.common
- com.unity.collab-proxy
- PredictionHistory
- com.unity.modules.assetbundle
- com.unity.mathematics
- dependencies
- RuneBlade(가칭) 입력 시스템 기획 — 이동 입력
- RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력
- com.unity.modules.accessibility
- RuneBlade(가칭) 입력 시스템 기획 — 개요
- com.unity.modules.ui
- com.unity.modules.terrain
- com.unity.modules.ai
- com.unity.modules.tetgen
- progress-overview.md
- com.unity.2d.sprite
- game-design-overview.md
- com.unity.modules.wind
- com.unity.modules.jsonserialize
- com.unity.modules.uielements
- com.unity.modules.unityanalytics
- com.unity.modules.unitywebrequest
- com.unity.modules.audio
- dependencies
- com.unity.modules.vectorgraphics

## God Nodes (most connected - your core abstractions)
1. `MovementSimulation` - 31 edges
2. `PredictionHistory` - 30 edges
3. `LogicalKey` - 30 edges
4. `@PlayerInputActions` - 29 edges
5. `TickInput` - 29 edges
6. `KeyInputEvent` - 26 edges
7. `DoubleTapTracker` - 24 edges
8. `RuneBlade.Simulation` - 23 edges
9. `InMemoryTransport` - 21 edges
10. `MovementSettings` - 20 edges

## Surprising Connections (you probably didn't know these)
- `클라이언트 이동 예측·보정` --references--> `MovementSettings`  [INFERRED]
  .docs/implement/decisions/decisions-overview.md → Assets/RuneBlade/Scripts/Simulation/Input/Movement/MovementSettings.cs
- `클라이언트 이동 예측·보정` --references--> `MovementReconciler`  [INFERRED]
  .docs/implement/decisions/decisions-overview.md → Assets/RuneBlade/Scripts/Client/MovementReconciler.cs
- `클라이언트 이동 예측·보정` --references--> `PredictionHistory`  [INFERRED]
  .docs/implement/decisions/decisions-overview.md → Assets/RuneBlade/Scripts/Client/PredictionHistory.cs
- `PredictionHistory` --references--> `TickInput`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Client/PredictionHistory.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/TickInput.cs
- `PredictionHistory` --references--> `MovementSimulation`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Client/PredictionHistory.cs → Assets/RuneBlade/Scripts/Simulation/Player/MovementSimulation.cs

## Import Cycles
- None detected.

## Communities (56 total, 8 thin omitted)

### Community 0 - "manifest.json"
Cohesion: 0.04
Nodes (55): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+47 more)

### Community 1 - "dependencies"
Cohesion: 0.04
Nodes (56): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+48 more)

### Community 2 - "com.unity.modules.umbra"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 3 - "com.unity.modules.subsystems"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 4 - "MovementSimulation"
Cohesion: 0.09
Nodes (19): JumpInputBuffer, IsStored, RemainingTicks, MovementSettings, DashSpeedPerTick, GravityPerTick, JumpVelocityPerTick, MoveSpeedPerTick (+11 more)

### Community 5 - "Test"
Cohesion: 0.09
Nodes (22): LogicalKey, Down, Left, Right, Slot1, Slot2, Slot3, Slot4 (+14 more)

### Community 6 - "@PlayerInputActions"
Cohesion: 0.07
Nodes (13): PlayerActions, @DropDown, enabled, @JumpUp, @LeftMove, @RightMove, @PlayerInputActions, asset (+5 more)

### Community 7 - "packages-lock.json"
Cohesion: 0.05
Nodes (40): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+32 more)

### Community 8 - "com.unity.test-framework"
Cohesion: 0.10
Nodes (22): dependencies, dependencies, depth, source, url, version, dependencies, depth (+14 more)

### Community 9 - "com.unity.render-pipelines.core"
Cohesion: 0.06
Nodes (37): dependencies, depth, source, version, dependencies, depth, source, url (+29 more)

### Community 10 - "RuneBlade.Simulation"
Cohesion: 0.07
Nodes (15): ReconcileResult, Corrected, Ignored, Matched, PlayerMovementSettingSO, Settings, Welcome2DScript, RuneBlade.Client (+7 more)

### Community 11 - "InMemoryTransport"
Cohesion: 0.11
Nodes (14): ClientHandle, InMemoryTransport, Server, ServerHandle, InputMessage, LastTick, IClientTransport, IServerTransport (+6 more)

### Community 12 - "com.unity.modules.physics"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 13 - "com.unity.modules.imgui"
Cohesion: 0.06
Nodes (33): dependencies, depth, source, version, dependencies, depth, source, version (+25 more)

### Community 14 - "com.unity.2d.tilemap"
Cohesion: 0.14
Nodes (15): dependencies, depth, dependencies, depth, source, url, version, source (+7 more)

### Community 15 - "dependencies"
Cohesion: 0.20
Nodes (11): depth, source, version, dependencies, depth, source, url, version (+3 more)

### Community 16 - "서버 권위 멀티플레이 환경 구현 방향성 참고 문서"
Cohesion: 0.15
Nodes (12): 1. 배경과 목적, 1순위 포함, 2. 범위, 2순위 포함, 3. 서버와 클라이언트의 역할 개요, 4. 원칙, 5. 방향성 이탈 점검 기준, 6. 결정 사항과 미정 사항 (+4 more)

### Community 17 - "TickInput"
Cohesion: 0.05
Nodes (20): LocalInputCollector, IPlayerActions, PlayerInputReader, PlayerMovementController, KeyInputEvent, IsPress, IsRelease, IsValid (+12 more)

### Community 18 - "com.unity.ext.nunit"
Cohesion: 0.17
Nodes (12): dependencies, depth, source, version, dependencies, depth, source, url (+4 more)

### Community 19 - "com.unity.modules.imageconversion"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 20 - "com.unity.modules.animation"
Cohesion: 0.11
Nodes (20): dependencies, depth, source, version, dependencies, depth, source, version (+12 more)

### Community 21 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.animation

### Community 22 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.aseprite

### Community 23 - "com.unity.burst"
Cohesion: 0.20
Nodes (10): dependencies, dependencies, depth, source, version, depth, source, version (+2 more)

### Community 24 - "com.unity.modules.physics2d"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, version, dependencies, depth, source, version (+3 more)

### Community 25 - "com.unity.2d.common"
Cohesion: 0.18
Nodes (11): depth, source, url, version, dependencies, depth, source, url (+3 more)

### Community 26 - "com.unity.collab-proxy"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.collab-proxy

### Community 27 - "PredictionHistory"
Cohesion: 0.15
Nodes (8): MovementReconciler, PredictionHistory, Capacity, NewestTick, MovementReconcilerTests, PredictionHistoryTests, 미정·보류, 클라이언트 이동 예측·보정

### Community 28 - "com.unity.modules.assetbundle"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 29 - "com.unity.mathematics"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 30 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, version, dependencies, com.unity.modules.hierarchycore

### Community 31 - "RuneBlade(가칭) 입력 시스템 기획 — 이동 입력"
Cohesion: 0.29
Nodes (6): RuneBlade(가칭) 입력 시스템 기획 — 이동 입력, 더블탭 이력 규칙, 이동 입력 규칙, 이동 입력: 방향키, 이동 종류, 일반 점프 선입력 버퍼

### Community 32 - "RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력"
Cohesion: 0.29
Nodes (6): RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력, 공격 입력 시 제약, 스킬 선입력 버퍼, 입력 제스처, 입력과 키 슬롯, 키 슬롯과 스킬 할당

### Community 33 - "com.unity.modules.accessibility"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.accessibility

### Community 34 - "RuneBlade(가칭) 입력 시스템 기획 — 개요"
Cohesion: 0.40
Nodes (4): RuneBlade(가칭) 입력 시스템 기획 — 개요, 문서 구성, 작성 원칙, 전제

### Community 35 - "com.unity.modules.ui"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, version, dependencies, depth, source, version (+3 more)

### Community 36 - "com.unity.modules.terrain"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 37 - "com.unity.modules.ai"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.ai

### Community 39 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 42 - "com.unity.2d.sprite"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 46 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

### Community 47 - "com.unity.modules.jsonserialize"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 49 - "com.unity.modules.uielements"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, url, version, depth, source, version (+2 more)

### Community 57 - "com.unity.modules.unityanalytics"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unityanalytics

### Community 58 - "com.unity.modules.unitywebrequest"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 60 - "com.unity.modules.audio"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 62 - "dependencies"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestwww

### Community 63 - "com.unity.modules.vectorgraphics"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.vectorgraphics

## Knowledge Gaps
- **157 isolated node(s):** `Capacity`, `NewestTick`, `Ignored`, `Matched`, `Corrected` (+152 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 546 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **8 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.modules.umbra`, `com.unity.modules.subsystems`, `packages-lock.json`, `com.unity.test-framework`, `com.unity.render-pipelines.core`, `com.unity.modules.physics`, `com.unity.modules.imgui`, `com.unity.2d.tilemap`, `com.unity.ext.nunit`, `com.unity.modules.imageconversion`, `com.unity.modules.animation`, `dependencies`, `dependencies`, `com.unity.burst`, `com.unity.modules.physics2d`, `com.unity.2d.common`, `com.unity.collab-proxy`, `com.unity.modules.assetbundle`, `com.unity.mathematics`, `dependencies`, `com.unity.modules.accessibility`, `com.unity.modules.ui`, `com.unity.modules.terrain`, `com.unity.modules.ai`, `com.unity.modules.tetgen`, `com.unity.2d.sprite`, `com.unity.modules.wind`, `com.unity.modules.jsonserialize`, `com.unity.modules.uielements`, `com.unity.modules.unityanalytics`, `com.unity.modules.unitywebrequest`, `com.unity.modules.audio`, `dependencies`, `com.unity.modules.vectorgraphics`?**
  _High betweenness centrality (0.191) - this node is a cross-community bridge._
- **Are the 17 inferred relationships involving `PredictionHistory` (e.g. with `.AfterCorrection_HistoryHoldsReplayedStates()` and `.HiddenStateOnlyDifference_IsAdopted_AndAffectsReplay()`) actually correct?**
  _`PredictionHistory` has 17 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Capacity`, `NewestTick`, `Ignored` to the rest of the system?**
  _157 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `@PlayerInputActions` connect `@PlayerInputActions` to `TickInput`, `RuneBlade.Simulation`, `InMemoryTransport`?**
  _High betweenness centrality (0.022) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `MovementSimulation` connect `MovementSimulation` to `InMemoryTransport`, `TickInput`, `PredictionHistory`, `Test`?**
  _High betweenness centrality (0.022) - this node is a cross-community bridge._