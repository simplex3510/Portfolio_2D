# Graph Report - Portfolio_2D  (2026-10-19)

## Corpus Check
- 45 files · ~41,734 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 167 file(s) not represented in the graph (top: .meta 108, .asset 35, .anim 10)

## Summary
- 881 nodes · 1399 edges · 62 communities (51 shown, 11 thin omitted)
- Extraction: 90% EXTRACTED · 10% INFERRED · 0% AMBIGUOUS · INFERRED: 143 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `27410e91`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- manifest.json
- dependencies
- MovementSettings
- com.unity.modules.subsystems
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
- com.unity.modules.particlesystem
- 서버 권위 멀티플레이 환경 구현 방향성 참고 문서
- KeyInputEvent
- com.unity.modules.ui
- com.unity.modules.unitywebrequest
- TickInput
- dependencies
- dependencies
- com.unity.2d.psdimporter
- com.unity.mathematics
- com.unity.2d.tooling
- com.unity.collab-proxy
- com.unity.graph-authoring
- com.unity.nuget.mono-cecil
- PlayerMovementController
- PlayerMovement
- RuneBlade(가칭) 입력 시스템 기획 — 이동 입력
- RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력
- com.unity.modules.ai
- RuneBlade(가칭) 입력 시스템 기획 — 개요
- com.unity.modules.uielements
- com.unity.modules.accessibility
- com.unity.profiling.core
- com.unity.modules.jsonserialize
- com.unity.modules.tetgen
- progress-overview.md
- com.unity.ugui
- com.unity.modules.umbra
- game-design-overview.md
- com.unity.searcher
- com.unity.modules.wind
- CallbackContext
- com.unity.inputsystem
- com.unity.modules.animation
- com.unity.2d.common
- com.unity.2d.sprite
- com.unity.modules.audio
- com.unity.modules.director
- dependencies
- com.unity.modules.video
- decisions-overview.md

## God Nodes (most connected - your core abstractions)
1. `@PlayerInputActions` - 29 edges
2. `LogicalKey` - 27 edges
3. `DoubleTapTracker` - 24 edges
4. `PlayerMovement` - 24 edges
5. `KeyInputEvent` - 23 edges
6. `InMemoryTransport` - 21 edges
7. `InputState` - 20 edges
8. `PlayerMovementController` - 19 edges
9. `UnreliableTransport` - 19 edges
10. `InputMessage` - 18 edges

## Surprising Connections (you probably didn't know these)
- `LocalInputCollector` --references--> `KeyInputEvent`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/LocalInputCollector.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/KeyInputEvent.cs
- `PlayerMovementController` --references--> `LocalInputCollector`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Game/Player/LocalInputCollector.cs
- `PlayerMovementController` --references--> `PlayerMovementSettingSO`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Game/Player/ScriptableObject/PlayerMovementSettingSO.cs
- `PlayerMovementController` --references--> `MovementSettings`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Movement/MovementSettings.cs
- `PlayerMovementController` --references--> `LogicalKey`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/LogicalKey.cs

## Import Cycles
- None detected.

## Communities (62 total, 11 thin omitted)

### Community 0 - "manifest.json"
Cohesion: 0.04
Nodes (55): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+47 more)

### Community 1 - "dependencies"
Cohesion: 0.04
Nodes (56): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+48 more)

### Community 2 - "MovementSettings"
Cohesion: 0.13
Nodes (8): JumpInputBuffer, IsStored, RemainingTicks, MovementSettings, DashSpeedPerTick, GravityPerTick, JumpVelocityPerTick, MoveSpeedPerTick

### Community 3 - "com.unity.modules.subsystems"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 4 - "com.unity.burst"
Cohesion: 0.20
Nodes (11): dependencies, dependencies, depth, source, version, dependencies, depth, source (+3 more)

### Community 5 - "Test"
Cohesion: 0.08
Nodes (22): LogicalKey, Down, Left, Right, Slot1, Slot2, Slot3, Slot4 (+14 more)

### Community 6 - "@PlayerInputActions"
Cohesion: 0.07
Nodes (13): PlayerActions, @DropDown, enabled, @JumpUp, @LeftMove, @RightMove, @PlayerInputActions, asset (+5 more)

### Community 7 - "packages-lock.json"
Cohesion: 0.05
Nodes (40): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+32 more)

### Community 8 - "com.unity.test-framework"
Cohesion: 0.07
Nodes (27): dependencies, depth, source, version, dependencies, depth, source, url (+19 more)

### Community 9 - "com.unity.render-pipelines.core"
Cohesion: 0.14
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 10 - "RuneBlade.Simulation"
Cohesion: 0.09
Nodes (9): PlayerMovementSettingSO, Settings, Welcome2DScript, RuneBlade.Game, RuneBlade.Simulation, Unity.U2D.Welcome, RuneBlade.Simulation.Tests, RuneBlade.Network.Tests (+1 more)

### Community 11 - "InMemoryTransport"
Cohesion: 0.09
Nodes (15): ClientHandle, InMemoryTransport, Server, ServerHandle, InputMessage, LastTick, IClientTransport, IServerTransport (+7 more)

### Community 12 - "com.unity.modules.physics"
Cohesion: 0.08
Nodes (25): dependencies, depth, source, version, dependencies, depth, source, version (+17 more)

### Community 13 - "com.unity.modules.imgui"
Cohesion: 0.06
Nodes (33): dependencies, depth, source, version, dependencies, depth, source, version (+25 more)

### Community 14 - "com.unity.2d.tilemap"
Cohesion: 0.08
Nodes (26): dependencies, depth, dependencies, depth, source, url, version, source (+18 more)

### Community 15 - "com.unity.modules.particlesystem"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 16 - "서버 권위 멀티플레이 환경 구현 방향성 참고 문서"
Cohesion: 0.15
Nodes (12): 1. 배경과 목적, 1순위 포함, 2. 범위, 2순위 포함, 3. 서버와 클라이언트의 역할 개요, 4. 원칙, 5. 방향성 이탈 점검 기준, 6. 결정 사항과 미정 사항 (+4 more)

### Community 17 - "KeyInputEvent"
Cohesion: 0.20
Nodes (9): KeyInputEvent, IsPress, IsRelease, IsValid, Key, Transition, KeyTransition, Press (+1 more)

### Community 18 - "com.unity.modules.ui"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, version, dependencies, com.unity.modules.ui

### Community 19 - "com.unity.modules.unitywebrequest"
Cohesion: 0.05
Nodes (46): dependencies, depth, source, version, dependencies, depth, source, version (+38 more)

### Community 20 - "TickInput"
Cohesion: 0.21
Nodes (6): LocalInputCollector, TickInput, EventCount, IsEmpty, IsFull, RawEvents

### Community 21 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.animation

### Community 22 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.aseprite

### Community 23 - "com.unity.2d.psdimporter"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.psdimporter

### Community 24 - "com.unity.mathematics"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 25 - "com.unity.2d.tooling"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.tooling

### Community 26 - "com.unity.collab-proxy"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.collab-proxy

### Community 27 - "com.unity.graph-authoring"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 28 - "com.unity.nuget.mono-cecil"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.nuget.mono-cecil

### Community 30 - "PlayerMovement"
Cohesion: 0.14
Nodes (11): HorizontalDirection, Left, None, Right, PlayerMovement, DashDirection, IsDashing, IsGrounded (+3 more)

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

### Community 35 - "com.unity.modules.uielements"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 36 - "com.unity.modules.accessibility"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.accessibility

### Community 37 - "com.unity.profiling.core"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.profiling.core

### Community 38 - "com.unity.modules.jsonserialize"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 39 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 41 - "com.unity.ugui"
Cohesion: 0.20
Nodes (10): depth, source, version, dependencies, depth, source, url, version (+2 more)

### Community 42 - "com.unity.modules.umbra"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 44 - "com.unity.searcher"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.searcher

### Community 46 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

### Community 49 - "com.unity.inputsystem"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.inputsystem

### Community 53 - "com.unity.modules.animation"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.animation

### Community 55 - "com.unity.2d.common"
Cohesion: 0.40
Nodes (5): depth, source, url, version, com.unity.2d.common

### Community 56 - "com.unity.2d.sprite"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.2d.sprite

### Community 57 - "com.unity.modules.audio"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.audio

### Community 58 - "com.unity.modules.director"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.director

### Community 59 - "dependencies"
Cohesion: 0.40
Nodes (5): depth, source, version, dependencies, com.unity.modules.unityanalytics

### Community 60 - "com.unity.modules.video"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.video

## Knowledge Gaps
- **152 isolated node(s):** `asset`, `bindingMask`, `devices`, `controlSchemes`, `bindings` (+147 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 545 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **11 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.modules.subsystems`, `com.unity.burst`, `packages-lock.json`, `com.unity.test-framework`, `com.unity.render-pipelines.core`, `com.unity.modules.physics`, `com.unity.modules.imgui`, `com.unity.2d.tilemap`, `com.unity.modules.particlesystem`, `com.unity.modules.ui`, `com.unity.modules.unitywebrequest`, `dependencies`, `dependencies`, `com.unity.2d.psdimporter`, `com.unity.mathematics`, `com.unity.2d.tooling`, `com.unity.collab-proxy`, `com.unity.graph-authoring`, `com.unity.nuget.mono-cecil`, `com.unity.modules.ai`, `com.unity.modules.uielements`, `com.unity.modules.accessibility`, `com.unity.profiling.core`, `com.unity.modules.jsonserialize`, `com.unity.modules.tetgen`, `com.unity.ugui`, `com.unity.modules.umbra`, `com.unity.searcher`, `com.unity.modules.wind`, `com.unity.inputsystem`, `com.unity.modules.animation`, `com.unity.2d.common`, `com.unity.2d.sprite`, `com.unity.modules.audio`, `com.unity.modules.director`, `com.unity.modules.video`?**
  _High betweenness centrality (0.217) - this node is a cross-community bridge._
- **What connects `asset`, `bindingMask`, `devices` to the rest of the system?**
  _152 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `PlayerMovementController` connect `PlayerMovementController` to `MovementSettings`, `Test`, `@PlayerInputActions`, `RuneBlade.Simulation`, `CallbackContext`, `TickInput`, `PlayerMovement`?**
  _High betweenness centrality (0.022) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `@PlayerInputActions` connect `@PlayerInputActions` to `RuneBlade.Simulation`, `InMemoryTransport`, `CallbackContext`?**
  _High betweenness centrality (0.021) - this node is a cross-community bridge._
- **Should `MovementSettings` be split into smaller, more focused modules?**
  _Cohesion score 0.12987012987012986 - nodes in this community are weakly interconnected._