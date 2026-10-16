# Graph Report - Portfolio_2D  (2026-10-16)

## Corpus Check
- 28 files · ~39,249 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 164 file(s) not represented in the graph (top: .meta 105, .asset 35, .anim 10)

## Summary
- 797 nodes · 1231 edges · 58 communities (56 shown, 2 thin omitted)
- Extraction: 91% EXTRACTED · 9% INFERRED · 0% AMBIGUOUS · INFERRED: 106 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `ba0e6c1e`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- manifest.json
- dependencies
- PlayerMovement
- com.unity.modules.subsystems
- PlayerMovementController
- Test
- @PlayerInputActions
- packages-lock.json
- com.unity.ext.nunit
- com.unity.burst
- RuneBlade.Simulation
- InMemoryTransport
- com.unity.modules.physics
- com.unity.settings-manager
- com.unity.2d.tilemap
- com.unity.modules.particlesystem
- dependencies
- com.unity.collections
- com.unity.modules.audio
- com.unity.modules.imageconversion
- com.unity.modules.unitywebrequest
- dependencies
- dependencies
- com.unity.2d.psdimporter
- com.unity.2d.spriteshape
- com.unity.2d.tooling
- com.unity.collab-proxy
- com.unity.inputsystem
- KeyInputEvent
- com.unity.splines
- com.unity.2d.common
- com.unity.2d.sprite
- com.unity.cinemachine
- dependencies
- com.unity.modules.ai
- dependencies
- com.unity.modules.imgui
- com.unity.test-framework
- com.unity.modules.jsonserialize
- com.unity.modules.tetgen
- com.unity.modules.ui
- com.unity.ugui
- com.unity.modules.umbra
- com.unity.modules.unitywebrequestaudio
- TickInput
- com.unity.modules.uielements
- com.unity.modules.wind
- CallbackContext
- JumpInputBuffer
- HorizontalDirection
- com.unity.ide.visualstudio
- com.unity.nuget.mono-cecil
- com.unity.mathematics
- com.unity.modules.animation
- com.unity.modules.director
- com.unity.modules.timelinefoundation
- com.unity.modules.unityanalytics
- com.unity.modules.xr

## God Nodes (most connected - your core abstractions)
1. `@PlayerInputActions` - 29 edges
2. `LogicalKey` - 27 edges
3. `DoubleTapTracker` - 24 edges
4. `PlayerMovement` - 24 edges
5. `KeyInputEvent` - 21 edges
6. `PlayerMovementController` - 20 edges
7. `InputState` - 20 edges
8. `InMemoryTransport` - 18 edges
9. `InputStateTests` - 17 edges
10. `RuneBlade.Simulation` - 16 edges

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

## Communities (58 total, 2 thin omitted)

### Community 0 - "manifest.json"
Cohesion: 0.04
Nodes (55): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+47 more)

### Community 1 - "dependencies"
Cohesion: 0.04
Nodes (56): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+48 more)

### Community 2 - "PlayerMovement"
Cohesion: 0.13
Nodes (12): MovementSettings, DashSpeedPerTick, GravityPerTick, JumpVelocityPerTick, MoveSpeedPerTick, PlayerMovement, DashDirection, IsDashing (+4 more)

### Community 3 - "com.unity.modules.subsystems"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 5 - "Test"
Cohesion: 0.09
Nodes (22): LogicalKey, Down, Left, Right, Slot1, Slot2, Slot3, Slot4 (+14 more)

### Community 6 - "@PlayerInputActions"
Cohesion: 0.07
Nodes (13): PlayerActions, @DropDown, enabled, @JumpUp, @LeftMove, @RightMove, @PlayerInputActions, asset (+5 more)

### Community 7 - "packages-lock.json"
Cohesion: 0.05
Nodes (40): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+32 more)

### Community 8 - "com.unity.ext.nunit"
Cohesion: 0.17
Nodes (12): dependencies, depth, source, version, dependencies, depth, source, url (+4 more)

### Community 9 - "com.unity.burst"
Cohesion: 0.05
Nodes (42): dependencies, depth, source, version, dependencies, depth, source, version (+34 more)

### Community 10 - "RuneBlade.Simulation"
Cohesion: 0.08
Nodes (9): PlayerMovementSettingSO, Settings, Welcome2DScript, RuneBlade.Game, RuneBlade.Simulation, Unity.U2D.Welcome, RuneBlade.Simulation.Tests, RuneBlade.Network.Tests (+1 more)

### Community 11 - "InMemoryTransport"
Cohesion: 0.13
Nodes (11): ClientHandle, InMemoryTransport, Server, ServerHandle, InputMessage, LastTick, IClientTransport, IServerTransport (+3 more)

### Community 12 - "com.unity.modules.physics"
Cohesion: 0.08
Nodes (25): dependencies, depth, source, version, dependencies, depth, source, version (+17 more)

### Community 13 - "com.unity.settings-manager"
Cohesion: 0.12
Nodes (17): dependencies, depth, source, version, dependencies, depth, source, url (+9 more)

### Community 14 - "com.unity.2d.tilemap"
Cohesion: 0.08
Nodes (26): dependencies, depth, dependencies, depth, source, url, version, source (+18 more)

### Community 15 - "com.unity.modules.particlesystem"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 16 - "dependencies"
Cohesion: 0.14
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 17 - "com.unity.collections"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.collections

### Community 18 - "com.unity.modules.audio"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 19 - "com.unity.modules.imageconversion"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 20 - "com.unity.modules.unitywebrequest"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 21 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.animation

### Community 22 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.aseprite

### Community 23 - "com.unity.2d.psdimporter"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.psdimporter

### Community 24 - "com.unity.2d.spriteshape"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.spriteshape

### Community 25 - "com.unity.2d.tooling"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.2d.tooling

### Community 26 - "com.unity.collab-proxy"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.collab-proxy

### Community 27 - "com.unity.inputsystem"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.inputsystem

### Community 28 - "KeyInputEvent"
Cohesion: 0.20
Nodes (9): KeyInputEvent, IsPress, IsRelease, IsValid, Key, Transition, KeyTransition, Press (+1 more)

### Community 29 - "com.unity.splines"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.splines

### Community 30 - "com.unity.2d.common"
Cohesion: 0.40
Nodes (5): depth, source, url, version, com.unity.2d.common

### Community 31 - "com.unity.2d.sprite"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.2d.sprite

### Community 32 - "com.unity.cinemachine"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.cinemachine

### Community 33 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, version, dependencies, com.unity.modules.accessibility

### Community 34 - "com.unity.modules.ai"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.ai

### Community 35 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, version, dependencies, com.unity.modules.hierarchycore

### Community 36 - "com.unity.modules.imgui"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.imgui

### Community 37 - "com.unity.test-framework"
Cohesion: 0.22
Nodes (10): dependencies, depth, dependencies, depth, source, version, source, version (+2 more)

### Community 38 - "com.unity.modules.jsonserialize"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 39 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 40 - "com.unity.modules.ui"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, version, dependencies, com.unity.modules.ui

### Community 41 - "com.unity.ugui"
Cohesion: 0.20
Nodes (10): depth, source, version, dependencies, depth, source, url, version (+2 more)

### Community 42 - "com.unity.modules.umbra"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 43 - "com.unity.modules.unitywebrequestaudio"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestaudio

### Community 44 - "TickInput"
Cohesion: 0.28
Nodes (5): TickInput, EventCount, IsEmpty, IsFull, RawEvents

### Community 45 - "com.unity.modules.uielements"
Cohesion: 0.22
Nodes (9): depth, source, version, dependencies, depth, source, version, com.unity.modules.uielements (+1 more)

### Community 46 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

### Community 48 - "JumpInputBuffer"
Cohesion: 0.33
Nodes (3): JumpInputBuffer, IsStored, RemainingTicks

### Community 49 - "HorizontalDirection"
Cohesion: 0.33
Nodes (4): HorizontalDirection, Left, None, Right

### Community 50 - "com.unity.ide.visualstudio"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.ide.visualstudio

### Community 51 - "com.unity.nuget.mono-cecil"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, url, version, com.unity.nuget.mono-cecil

### Community 52 - "com.unity.mathematics"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.mathematics

### Community 53 - "com.unity.modules.animation"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.animation

### Community 54 - "com.unity.modules.director"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.director

### Community 55 - "com.unity.modules.timelinefoundation"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.timelinefoundation

### Community 56 - "com.unity.modules.unityanalytics"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unityanalytics

### Community 57 - "com.unity.modules.xr"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.xr

## Knowledge Gaps
- **151 isolated node(s):** `asset`, `bindingMask`, `devices`, `controlSchemes`, `bindings` (+146 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 502 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **2 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.modules.subsystems`, `packages-lock.json`, `com.unity.ext.nunit`, `com.unity.burst`, `com.unity.modules.physics`, `com.unity.settings-manager`, `com.unity.2d.tilemap`, `com.unity.modules.particlesystem`, `dependencies`, `com.unity.collections`, `com.unity.modules.audio`, `com.unity.modules.imageconversion`, `com.unity.modules.unitywebrequest`, `dependencies`, `dependencies`, `com.unity.2d.psdimporter`, `com.unity.2d.spriteshape`, `com.unity.2d.tooling`, `com.unity.collab-proxy`, `com.unity.inputsystem`, `com.unity.splines`, `com.unity.2d.common`, `com.unity.2d.sprite`, `com.unity.cinemachine`, `com.unity.modules.ai`, `dependencies`, `com.unity.modules.imgui`, `com.unity.test-framework`, `com.unity.modules.jsonserialize`, `com.unity.modules.tetgen`, `com.unity.modules.ui`, `com.unity.ugui`, `com.unity.modules.umbra`, `com.unity.modules.unitywebrequestaudio`, `com.unity.modules.uielements`, `com.unity.modules.wind`, `com.unity.ide.visualstudio`, `com.unity.nuget.mono-cecil`, `com.unity.mathematics`, `com.unity.modules.animation`, `com.unity.modules.director`, `com.unity.modules.timelinefoundation`, `com.unity.modules.unityanalytics`, `com.unity.modules.xr`?**
  _High betweenness centrality (0.265) - this node is a cross-community bridge._
- **What connects `asset`, `bindingMask`, `devices` to the rest of the system?**
  _151 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `PlayerMovementController` connect `PlayerMovementController` to `PlayerMovement`, `Test`, `@PlayerInputActions`, `RuneBlade.Simulation`, `InMemoryTransport`, `KeyInputEvent`?**
  _High betweenness centrality (0.032) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `PlayerMovement` connect `PlayerMovement` to `PlayerMovementController`, `Test`, `RuneBlade.Simulation`, `InMemoryTransport`, `JumpInputBuffer`, `HorizontalDirection`?**
  _High betweenness centrality (0.021) - this node is a cross-community bridge._
- **Should `PlayerMovement` be split into smaller, more focused modules?**
  _Cohesion score 0.13 - nodes in this community are weakly interconnected._