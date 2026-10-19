# Graph Report - Portfolio_2D  (2026-10-20)

## Corpus Check
- 48 files · ~42,408 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 174 file(s) not represented in the graph (top: .meta 113, .asset 35, .anim 10)

## Summary
- 906 nodes · 1483 edges · 56 communities (47 shown, 9 thin omitted)
- Extraction: 89% EXTRACTED · 11% INFERRED · 0% AMBIGUOUS · INFERRED: 163 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `27410e91`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- manifest.json
- dependencies
- JumpInputBuffer
- com.unity.modules.jsonserialize
- com.unity.burst
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
- com.unity.modules.animation
- 서버 권위 멀티플레이 환경 구현 방향성 참고 문서
- KeyInputEvent
- MovementSimulation
- com.unity.modules.imageconversion
- TickInput
- dependencies
- dependencies
- com.unity.2d.sprite
- com.unity.mathematics
- com.unity.2d.common
- com.unity.collab-proxy
- PredictionHistory
- com.unity.modules.assetbundle
- com.unity.modules.particlesystem
- HorizontalDirection
- RuneBlade(가칭) 입력 시스템 기획 — 이동 입력
- RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력
- com.unity.modules.ai
- RuneBlade(가칭) 입력 시스템 기획 — 개요
- com.unity.modules.unitywebrequest
- dependencies
- com.unity.profiling.core
- com.unity.modules.unitywebrequestassetbundle
- com.unity.modules.tetgen
- progress-overview.md
- com.unity.modules.unitywebrequestaudio
- com.unity.modules.unitywebrequesttexture
- game-design-overview.md
- dependencies
- com.unity.modules.wind
- com.unity.modules.uielements
- com.unity.modules.audio
- com.unity.modules.video
- decisions-overview.md

## God Nodes (most connected - your core abstractions)
1. `@PlayerInputActions` - 29 edges
2. `LogicalKey` - 28 edges
3. `MovementSimulation` - 28 edges
4. `KeyInputEvent` - 24 edges
5. `DoubleTapTracker` - 24 edges
6. `TickInput` - 23 edges
7. `InMemoryTransport` - 21 edges
8. `RuneBlade.Simulation` - 21 edges
9. `InputState` - 20 edges
10. `UnreliableTransport` - 19 edges

## Surprising Connections (you probably didn't know these)
- `클라이언트 이동 예측·보정` --references--> `PredictionHistory`  [INFERRED]
  .docs/implement/decisions/decisions-overview.md → Assets/RuneBlade/Scripts/Client/PredictionHistory.cs
- `PredictionHistory` --references--> `TickInput`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Client/PredictionHistory.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/TickInput.cs
- `PredictionHistory` --references--> `MovementSimulation`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Client/PredictionHistory.cs → Assets/RuneBlade/Scripts/Simulation/Player/MovementSimulation.cs
- `PlayerInputReader` --references--> `LogicalKey`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerInputReader.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/LogicalKey.cs
- `PlayerMovementController` --references--> `PlayerMovementSettingSO`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Game/Player/ScriptableObject/PlayerMovementSettingSO.cs

## Import Cycles
- None detected.

## Communities (56 total, 9 thin omitted)

### Community 0 - "manifest.json"
Cohesion: 0.04
Nodes (55): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+47 more)

### Community 1 - "dependencies"
Cohesion: 0.04
Nodes (56): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+48 more)

### Community 2 - "JumpInputBuffer"
Cohesion: 0.22
Nodes (3): JumpInputBuffer, IsStored, RemainingTicks

### Community 3 - "com.unity.modules.jsonserialize"
Cohesion: 0.05
Nodes (45): dependencies, depth, source, version, dependencies, depth, source, version (+37 more)

### Community 4 - "com.unity.burst"
Cohesion: 0.20
Nodes (11): dependencies, dependencies, depth, source, version, depth, source, version (+3 more)

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
Cohesion: 0.06
Nodes (34): dependencies, dependencies, depth, source, version, dependencies, depth, source (+26 more)

### Community 9 - "com.unity.render-pipelines.core"
Cohesion: 0.07
Nodes (30): dependencies, depth, source, version, depth, source, version, dependencies (+22 more)

### Community 10 - "RuneBlade.Simulation"
Cohesion: 0.08
Nodes (11): PlayerMovementSettingSO, Settings, Welcome2DScript, RuneBlade.Client, RuneBlade.Client.Tests, RuneBlade.Game, RuneBlade.Simulation, Unity.U2D.Welcome (+3 more)

### Community 11 - "InMemoryTransport"
Cohesion: 0.10
Nodes (14): ClientHandle, InMemoryTransport, Server, ServerHandle, InputMessage, LastTick, IClientTransport, IServerTransport (+6 more)

### Community 12 - "com.unity.modules.physics"
Cohesion: 0.04
Nodes (47): dependencies, depth, source, version, dependencies, depth, source, version (+39 more)

### Community 13 - "com.unity.modules.imgui"
Cohesion: 0.06
Nodes (33): dependencies, depth, source, version, dependencies, depth, source, version (+25 more)

### Community 14 - "com.unity.2d.tilemap"
Cohesion: 0.13
Nodes (16): dependencies, depth, dependencies, depth, source, url, version, source (+8 more)

### Community 15 - "com.unity.modules.animation"
Cohesion: 0.14
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 16 - "서버 권위 멀티플레이 환경 구현 방향성 참고 문서"
Cohesion: 0.15
Nodes (12): 1. 배경과 목적, 1순위 포함, 2. 범위, 2순위 포함, 3. 서버와 클라이언트의 역할 개요, 4. 원칙, 5. 방향성 이탈 점검 기준, 6. 결정 사항과 미정 사항 (+4 more)

### Community 17 - "KeyInputEvent"
Cohesion: 0.08
Nodes (13): LocalInputCollector, IPlayerActions, PlayerInputReader, PlayerMovementController, KeyInputEvent, IsPress, IsRelease, IsValid (+5 more)

### Community 18 - "MovementSimulation"
Cohesion: 0.15
Nodes (12): MovementSettings, DashSpeedPerTick, GravityPerTick, JumpVelocityPerTick, MoveSpeedPerTick, MovementSimulation, DashDirection, IsDashing (+4 more)

### Community 19 - "com.unity.modules.imageconversion"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 20 - "TickInput"
Cohesion: 0.18
Nodes (6): TickInput, EventCount, IsEmpty, IsFull, RawEvents, InputMessageTests

### Community 21 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.animation

### Community 22 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.aseprite

### Community 23 - "com.unity.2d.sprite"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 24 - "com.unity.mathematics"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 25 - "com.unity.2d.common"
Cohesion: 0.18
Nodes (11): depth, source, url, version, dependencies, depth, source, url (+3 more)

### Community 26 - "com.unity.collab-proxy"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.collab-proxy

### Community 27 - "PredictionHistory"
Cohesion: 0.27
Nodes (4): PredictionHistory, Capacity, NewestTick, PredictionHistoryTests

### Community 28 - "com.unity.modules.assetbundle"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.assetbundle

### Community 29 - "com.unity.modules.particlesystem"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.particlesystem

### Community 30 - "HorizontalDirection"
Cohesion: 0.33
Nodes (4): HorizontalDirection, Left, None, Right

### Community 31 - "RuneBlade(가칭) 입력 시스템 기획 — 이동 입력"
Cohesion: 0.29
Nodes (6): RuneBlade(가칭) 입력 시스템 기획 — 이동 입력, 더블탭 이력 규칙, 이동 입력 규칙, 이동 입력: 방향키, 이동 종류, 일반 점프 선입력 버퍼

### Community 32 - "RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력"
Cohesion: 0.29
Nodes (6): RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력, 공격 입력 시 제약, 스킬 선입력 버퍼, 입력 제스처, 입력과 키 슬롯, 키 슬롯과 스킬 할당

### Community 33 - "com.unity.modules.ai"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.ai

### Community 34 - "RuneBlade(가칭) 입력 시스템 기획 — 개요"
Cohesion: 0.40
Nodes (4): RuneBlade(가칭) 입력 시스템 기획 — 개요, 문서 구성, 작성 원칙, 전제

### Community 35 - "com.unity.modules.unitywebrequest"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequest

### Community 36 - "dependencies"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, version, dependencies, depth, source, version (+3 more)

### Community 37 - "com.unity.profiling.core"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.profiling.core

### Community 38 - "com.unity.modules.unitywebrequestassetbundle"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestassetbundle

### Community 39 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 41 - "com.unity.modules.unitywebrequestaudio"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestaudio

### Community 42 - "com.unity.modules.unitywebrequesttexture"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequesttexture

### Community 44 - "dependencies"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestwww

### Community 46 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

### Community 49 - "com.unity.modules.uielements"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, url, version, depth, source, version (+7 more)

### Community 57 - "com.unity.modules.audio"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.audio

### Community 60 - "com.unity.modules.video"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.video

## Knowledge Gaps
- **154 isolated node(s):** `Capacity`, `NewestTick`, `asset`, `bindingMask`, `devices` (+149 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 544 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **9 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.modules.jsonserialize`, `com.unity.burst`, `packages-lock.json`, `com.unity.test-framework`, `com.unity.render-pipelines.core`, `com.unity.modules.physics`, `com.unity.modules.imgui`, `com.unity.2d.tilemap`, `com.unity.modules.animation`, `com.unity.modules.imageconversion`, `dependencies`, `dependencies`, `com.unity.2d.sprite`, `com.unity.mathematics`, `com.unity.2d.common`, `com.unity.collab-proxy`, `com.unity.modules.assetbundle`, `com.unity.modules.particlesystem`, `com.unity.modules.ai`, `com.unity.modules.unitywebrequest`, `com.unity.profiling.core`, `com.unity.modules.unitywebrequestassetbundle`, `com.unity.modules.tetgen`, `com.unity.modules.unitywebrequestaudio`, `com.unity.modules.unitywebrequesttexture`, `dependencies`, `com.unity.modules.wind`, `com.unity.modules.uielements`, `com.unity.modules.audio`, `com.unity.modules.video`?**
  _High betweenness centrality (0.205) - this node is a cross-community bridge._
- **What connects `Capacity`, `NewestTick`, `asset` to the rest of the system?**
  _154 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `MovementSimulation` connect `MovementSimulation` to `JumpInputBuffer`, `Test`, `InMemoryTransport`, `KeyInputEvent`, `PredictionHistory`, `HorizontalDirection`?**
  _High betweenness centrality (0.023) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `@PlayerInputActions` connect `@PlayerInputActions` to `KeyInputEvent`, `RuneBlade.Simulation`, `InMemoryTransport`?**
  _High betweenness centrality (0.022) - this node is a cross-community bridge._
- **Should `com.unity.modules.jsonserialize` be split into smaller, more focused modules?**
  _Cohesion score 0.045454545454545456 - nodes in this community are weakly interconnected._