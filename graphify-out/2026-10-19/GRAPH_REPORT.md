# Graph Report - Portfolio_2D  (2026-10-19)

## Corpus Check
- 43 files · ~41,430 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 167 file(s) not represented in the graph (top: .meta 108, .asset 35, .anim 10)

## Summary
- 873 nodes · 1385 edges · 55 communities (44 shown, 11 thin omitted)
- Extraction: 90% EXTRACTED · 10% INFERRED · 0% AMBIGUOUS · INFERRED: 140 edges (avg confidence: 0.85)
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
- Test
- LogicalKey
- @PlayerInputActions
- packages-lock.json
- com.unity.burst
- com.unity.render-pipelines.core
- RuneBlade.Simulation
- InMemoryTransport
- com.unity.modules.physics
- com.unity.modules.imgui
- com.unity.modules.tilemap
- com.unity.modules.particlesystem
- 서버 권위 멀티플레이 환경 구현 방향성 참고 문서
- KeyInputEvent
- com.unity.modules.ui
- com.unity.modules.unitywebrequest
- TickInput
- com.unity.2d.sprite
- dependencies
- com.unity.2d.psdimporter
- com.unity.mathematics
- com.unity.2d.common
- com.unity.collab-proxy
- com.unity.modules.terrain
- .PressOf
- PlayerMovementController
- PlayerMovement
- RuneBlade(가칭) 입력 시스템 기획 — 이동 입력
- RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력
- dependencies
- RuneBlade(가칭) 입력 시스템 기획 — 개요
- dependencies
- com.unity.modules.accessibility
- com.unity.modules.physics2d
- com.unity.modules.jsonserialize
- com.unity.modules.tetgen
- progress-overview.md
- dependencies
- com.unity.modules.umbra
- game-design-overview.md
- com.unity.modules.uielements
- com.unity.modules.wind
- CallbackContext
- com.unity.inputsystem
- com.unity.modules.animation

## God Nodes (most connected - your core abstractions)
1. `@PlayerInputActions` - 29 edges
2. `LogicalKey` - 27 edges
3. `DoubleTapTracker` - 24 edges
4. `PlayerMovement` - 24 edges
5. `KeyInputEvent` - 22 edges
6. `InMemoryTransport` - 21 edges
7. `PlayerMovementController` - 20 edges
8. `InputState` - 20 edges
9. `UnreliableTransport` - 19 edges
10. `InputMessage` - 18 edges

## Surprising Connections (you probably didn't know these)
- `PlayerMovementController` --references--> `PlayerMovementSettingSO`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Game/Player/ScriptableObject/PlayerMovementSettingSO.cs
- `PlayerMovementController` --references--> `MovementSettings`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Movement/MovementSettings.cs
- `PlayerMovementController` --references--> `KeyInputEvent`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/KeyInputEvent.cs
- `PlayerMovementController` --references--> `LogicalKey`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/LogicalKey.cs
- `PlayerMovementController` --references--> `PlayerMovement`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Player/PlayerMovement.cs

## Import Cycles
- None detected.

## Communities (55 total, 11 thin omitted)

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

### Community 4 - "Test"
Cohesion: 0.24
Nodes (5): DoubleTapTracker, IsActive, Key, RemainingTicks, DoubleTapTrackerTests

### Community 5 - "LogicalKey"
Cohesion: 0.13
Nodes (17): LogicalKey, Down, Left, Right, Slot1, Slot2, Slot3, Slot4 (+9 more)

### Community 6 - "@PlayerInputActions"
Cohesion: 0.07
Nodes (13): PlayerActions, @DropDown, enabled, @JumpUp, @LeftMove, @RightMove, @PlayerInputActions, asset (+5 more)

### Community 7 - "packages-lock.json"
Cohesion: 0.05
Nodes (40): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+32 more)

### Community 8 - "com.unity.burst"
Cohesion: 0.06
Nodes (39): dependencies, dependencies, depth, source, version, dependencies, depth, source (+31 more)

### Community 9 - "com.unity.render-pipelines.core"
Cohesion: 0.07
Nodes (30): dependencies, depth, source, version, depth, source, version, dependencies (+22 more)

### Community 10 - "RuneBlade.Simulation"
Cohesion: 0.09
Nodes (9): PlayerMovementSettingSO, Settings, Welcome2DScript, RuneBlade.Game, RuneBlade.Simulation, Unity.U2D.Welcome, RuneBlade.Simulation.Tests, RuneBlade.Network.Tests (+1 more)

### Community 11 - "InMemoryTransport"
Cohesion: 0.10
Nodes (14): ClientHandle, InMemoryTransport, Server, ServerHandle, InputMessage, LastTick, IClientTransport, IServerTransport (+6 more)

### Community 12 - "com.unity.modules.physics"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 13 - "com.unity.modules.imgui"
Cohesion: 0.04
Nodes (46): dependencies, dependencies, depth, source, version, dependencies, depth, source (+38 more)

### Community 14 - "com.unity.modules.tilemap"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 15 - "com.unity.modules.particlesystem"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.particlesystem

### Community 16 - "서버 권위 멀티플레이 환경 구현 방향성 참고 문서"
Cohesion: 0.15
Nodes (12): 1. 배경과 목적, 1순위 포함, 2. 범위, 2순위 포함, 3. 서버와 클라이언트의 역할 개요, 4. 원칙, 5. 방향성 이탈 점검 기준, 6. 결정 사항과 미정 사항 (+4 more)

### Community 17 - "KeyInputEvent"
Cohesion: 0.20
Nodes (9): KeyInputEvent, IsPress, IsRelease, IsValid, Key, Transition, KeyTransition, Press (+1 more)

### Community 18 - "com.unity.modules.ui"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 19 - "com.unity.modules.unitywebrequest"
Cohesion: 0.05
Nodes (45): dependencies, depth, source, version, dependencies, depth, source, version (+37 more)

### Community 20 - "TickInput"
Cohesion: 0.28
Nodes (5): TickInput, EventCount, IsEmpty, IsFull, RawEvents

### Community 21 - "com.unity.2d.sprite"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 22 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.aseprite

### Community 23 - "com.unity.2d.psdimporter"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.psdimporter

### Community 24 - "com.unity.mathematics"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, url, version, dependencies, depth, source (+3 more)

### Community 25 - "com.unity.2d.common"
Cohesion: 0.18
Nodes (11): depth, source, url, version, dependencies, depth, source, url (+3 more)

### Community 26 - "com.unity.collab-proxy"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.collab-proxy

### Community 27 - "com.unity.modules.terrain"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 30 - "PlayerMovement"
Cohesion: 0.13
Nodes (11): HorizontalDirection, Left, None, Right, PlayerMovement, DashDirection, IsDashing, IsGrounded (+3 more)

### Community 31 - "RuneBlade(가칭) 입력 시스템 기획 — 이동 입력"
Cohesion: 0.29
Nodes (6): RuneBlade(가칭) 입력 시스템 기획 — 이동 입력, 더블탭 이력 규칙, 이동 입력 규칙, 이동 입력: 방향키, 이동 종류, 일반 점프 선입력 버퍼

### Community 32 - "RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력"
Cohesion: 0.29
Nodes (6): RuneBlade(가칭) 입력 시스템 기획 — 스킬 입력, 공격 입력 시 제약, 스킬 선입력 버퍼, 입력 제스처, 입력과 키 슬롯, 키 슬롯과 스킬 할당

### Community 33 - "dependencies"
Cohesion: 0.20
Nodes (10): depth, source, version, dependencies, depth, source, version, dependencies (+2 more)

### Community 34 - "RuneBlade(가칭) 입력 시스템 기획 — 개요"
Cohesion: 0.40
Nodes (4): RuneBlade(가칭) 입력 시스템 기획 — 개요, 문서 구성, 작성 원칙, 전제

### Community 35 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, version, dependencies, com.unity.modules.hierarchycore

### Community 36 - "com.unity.modules.accessibility"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.accessibility

### Community 37 - "com.unity.modules.physics2d"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 38 - "com.unity.modules.jsonserialize"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 39 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 41 - "dependencies"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, version, dependencies, depth, source, url (+3 more)

### Community 42 - "com.unity.modules.umbra"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 44 - "com.unity.modules.uielements"
Cohesion: 0.22
Nodes (9): dependencies, depth, source, version, depth, source, version, com.unity.2d.tilemap (+1 more)

### Community 46 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

### Community 49 - "com.unity.inputsystem"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.inputsystem

### Community 53 - "com.unity.modules.animation"
Cohesion: 0.11
Nodes (20): dependencies, depth, source, version, dependencies, depth, source, version (+12 more)

## Knowledge Gaps
- **152 isolated node(s):** `asset`, `bindingMask`, `devices`, `controlSchemes`, `bindings` (+147 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 542 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **11 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.modules.subsystems`, `packages-lock.json`, `com.unity.burst`, `com.unity.render-pipelines.core`, `com.unity.modules.physics`, `com.unity.modules.imgui`, `com.unity.modules.tilemap`, `com.unity.modules.particlesystem`, `com.unity.modules.ui`, `com.unity.modules.unitywebrequest`, `com.unity.2d.sprite`, `dependencies`, `com.unity.2d.psdimporter`, `com.unity.mathematics`, `com.unity.2d.common`, `com.unity.collab-proxy`, `com.unity.modules.terrain`, `dependencies`, `com.unity.modules.accessibility`, `com.unity.modules.physics2d`, `com.unity.modules.jsonserialize`, `com.unity.modules.tetgen`, `dependencies`, `com.unity.modules.umbra`, `com.unity.modules.uielements`, `com.unity.modules.wind`, `com.unity.inputsystem`, `com.unity.modules.animation`?**
  _High betweenness centrality (0.221) - this node is a cross-community bridge._
- **What connects `asset`, `bindingMask`, `devices` to the rest of the system?**
  _152 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `PlayerMovementController` connect `PlayerMovementController` to `MovementSettings`, `LogicalKey`, `@PlayerInputActions`, `RuneBlade.Simulation`, `InMemoryTransport`, `KeyInputEvent`, `PlayerMovement`?**
  _High betweenness centrality (0.025) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `@PlayerInputActions` connect `@PlayerInputActions` to `RuneBlade.Simulation`, `InMemoryTransport`, `CallbackContext`?**
  _High betweenness centrality (0.020) - this node is a cross-community bridge._
- **Should `MovementSettings` be split into smaller, more focused modules?**
  _Cohesion score 0.12987012987012986 - nodes in this community are weakly interconnected._