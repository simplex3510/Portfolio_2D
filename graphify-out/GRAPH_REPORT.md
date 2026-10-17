# Graph Report - Portfolio_2D  (2026-10-17)

## Corpus Check
- 31 files · ~40,189 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 167 file(s) not represented in the graph (top: .meta 108, .asset 35, .anim 10)

## Summary
- 826 nodes · 1350 edges · 48 communities
- Extraction: 90% EXTRACTED · 10% INFERRED · 0% AMBIGUOUS · INFERRED: 140 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `1f7c90af`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- manifest.json
- dependencies
- PlayerMovement
- com.unity.modules.subsystems
- Test
- LogicalKey
- @PlayerInputActions
- packages-lock.json
- com.unity.test-framework
- com.unity.burst
- RuneBlade.Simulation
- InMemoryTransport
- com.unity.modules.physics
- com.unity.modules.imgui
- com.unity.2d.tilemap
- com.unity.modules.particlesystem
- com.unity.modules.assetbundle
- com.unity.collections
- com.unity.modules.ui
- com.unity.modules.imageconversion
- com.unity.modules.unitywebrequest
- dependencies
- dependencies
- com.unity.2d.psdimporter
- com.unity.2d.spriteshape
- com.unity.2d.tooling
- com.unity.collab-proxy
- com.unity.modules.terrain
- dependencies
- com.unity.modules.audio
- com.unity.2d.common
- com.unity.2d.sprite
- com.unity.modules.unitywebrequestassetbundle
- com.unity.modules.accessibility
- com.unity.modules.ai
- dependencies
- dependencies
- com.unity.modules.jsonserialize
- com.unity.modules.tetgen
- dependencies
- com.unity.modules.umbra
- com.unity.modules.unitywebrequestaudio
- com.unity.modules.wind
- com.unity.mathematics
- com.unity.modules.animation
- com.unity.modules.director
- com.unity.modules.unityanalytics
- com.unity.modules.xr

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
- `PlayerMovementController` --references--> `KeyInputEvent`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/KeyInputEvent.cs
- `PlayerMovementController` --references--> `LogicalKey`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/LogicalKey.cs
- `PlayerMovementSettingSO` --references--> `MovementSettings`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/ScriptableObject/PlayerMovementSettingSO.cs → Assets/RuneBlade/Scripts/Simulation/Input/Movement/MovementSettings.cs
- `InputMessage` --references--> `TickInput`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Network/InputMessage.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/TickInput.cs

## Import Cycles
- None detected.

## Communities (48 total, 0 thin omitted)

### Community 0 - "manifest.json"
Cohesion: 0.04
Nodes (55): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+47 more)

### Community 1 - "dependencies"
Cohesion: 0.04
Nodes (56): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+48 more)

### Community 2 - "PlayerMovement"
Cohesion: 0.05
Nodes (21): IPlayerActions, PlayerMovementController, JumpInputBuffer, IsStored, RemainingTicks, MovementSettings, DashSpeedPerTick, GravityPerTick (+13 more)

### Community 3 - "com.unity.modules.subsystems"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 4 - "Test"
Cohesion: 0.24
Nodes (5): DoubleTapTracker, IsActive, Key, RemainingTicks, DoubleTapTrackerTests

### Community 5 - "LogicalKey"
Cohesion: 0.07
Nodes (31): KeyInputEvent, IsPress, IsRelease, IsValid, Key, Transition, KeyTransition, Press (+23 more)

### Community 6 - "@PlayerInputActions"
Cohesion: 0.07
Nodes (13): PlayerActions, @DropDown, enabled, @JumpUp, @LeftMove, @RightMove, @PlayerInputActions, asset (+5 more)

### Community 7 - "packages-lock.json"
Cohesion: 0.05
Nodes (40): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+32 more)

### Community 8 - "com.unity.test-framework"
Cohesion: 0.06
Nodes (34): dependencies, dependencies, depth, source, version, dependencies, depth, source (+26 more)

### Community 9 - "com.unity.burst"
Cohesion: 0.05
Nodes (42): dependencies, depth, source, version, dependencies, depth, source, version (+34 more)

### Community 10 - "RuneBlade.Simulation"
Cohesion: 0.09
Nodes (9): PlayerMovementSettingSO, Settings, Welcome2DScript, RuneBlade.Game, RuneBlade.Simulation, Unity.U2D.Welcome, RuneBlade.Simulation.Tests, RuneBlade.Network.Tests (+1 more)

### Community 11 - "InMemoryTransport"
Cohesion: 0.09
Nodes (15): ClientHandle, InMemoryTransport, Server, ServerHandle, InputMessage, LastTick, IClientTransport, IServerTransport (+7 more)

### Community 12 - "com.unity.modules.physics"
Cohesion: 0.13
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 13 - "com.unity.modules.imgui"
Cohesion: 0.06
Nodes (33): dependencies, depth, source, version, dependencies, depth, source, version (+25 more)

### Community 14 - "com.unity.2d.tilemap"
Cohesion: 0.08
Nodes (26): dependencies, depth, dependencies, depth, source, url, version, source (+18 more)

### Community 15 - "com.unity.modules.particlesystem"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 16 - "com.unity.modules.assetbundle"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.assetbundle

### Community 17 - "com.unity.collections"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.collections

### Community 18 - "com.unity.modules.ui"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 19 - "com.unity.modules.imageconversion"
Cohesion: 0.07
Nodes (30): dependencies, depth, source, url, version, dependencies, depth, source (+22 more)

### Community 20 - "com.unity.modules.unitywebrequest"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequest

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

### Community 27 - "com.unity.modules.terrain"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

### Community 28 - "dependencies"
Cohesion: 0.40
Nodes (5): depth, source, version, dependencies, com.unity.modules.androidjni

### Community 29 - "com.unity.modules.audio"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.audio

### Community 30 - "com.unity.2d.common"
Cohesion: 0.40
Nodes (5): depth, source, url, version, com.unity.2d.common

### Community 31 - "com.unity.2d.sprite"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.2d.sprite

### Community 32 - "com.unity.modules.unitywebrequestassetbundle"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestassetbundle

### Community 33 - "com.unity.modules.accessibility"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.accessibility

### Community 34 - "com.unity.modules.ai"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.ai

### Community 35 - "dependencies"
Cohesion: 0.33
Nodes (6): dependencies, depth, source, version, dependencies, com.unity.modules.hierarchycore

### Community 36 - "dependencies"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestwww

### Community 38 - "com.unity.modules.jsonserialize"
Cohesion: 0.18
Nodes (11): dependencies, dependencies, depth, source, version, dependencies, depth, source (+3 more)

### Community 39 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 41 - "dependencies"
Cohesion: 0.18
Nodes (11): dependencies, depth, source, version, dependencies, depth, source, url (+3 more)

### Community 42 - "com.unity.modules.umbra"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 43 - "com.unity.modules.unitywebrequestaudio"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestaudio

### Community 46 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

### Community 52 - "com.unity.mathematics"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.mathematics

### Community 53 - "com.unity.modules.animation"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.animation

### Community 54 - "com.unity.modules.director"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.director

### Community 56 - "com.unity.modules.unityanalytics"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unityanalytics

### Community 57 - "com.unity.modules.xr"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.xr

## Knowledge Gaps
- **152 isolated node(s):** `asset`, `bindingMask`, `devices`, `controlSchemes`, `bindings` (+147 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 503 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.modules.subsystems`, `packages-lock.json`, `com.unity.test-framework`, `com.unity.burst`, `com.unity.modules.physics`, `com.unity.modules.imgui`, `com.unity.2d.tilemap`, `com.unity.modules.particlesystem`, `com.unity.modules.assetbundle`, `com.unity.collections`, `com.unity.modules.ui`, `com.unity.modules.imageconversion`, `com.unity.modules.unitywebrequest`, `dependencies`, `dependencies`, `com.unity.2d.psdimporter`, `com.unity.2d.spriteshape`, `com.unity.2d.tooling`, `com.unity.collab-proxy`, `com.unity.modules.terrain`, `com.unity.modules.audio`, `com.unity.2d.common`, `com.unity.2d.sprite`, `com.unity.modules.unitywebrequestassetbundle`, `com.unity.modules.accessibility`, `com.unity.modules.ai`, `dependencies`, `dependencies`, `com.unity.modules.jsonserialize`, `com.unity.modules.tetgen`, `dependencies`, `com.unity.modules.umbra`, `com.unity.modules.unitywebrequestaudio`, `com.unity.modules.wind`, `com.unity.mathematics`, `com.unity.modules.animation`, `com.unity.modules.director`, `com.unity.modules.unityanalytics`, `com.unity.modules.xr`?**
  _High betweenness centrality (0.246) - this node is a cross-community bridge._
- **What connects `asset`, `bindingMask`, `devices` to the rest of the system?**
  _152 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `PlayerMovementController` connect `PlayerMovement` to `RuneBlade.Simulation`, `InMemoryTransport`, `LogicalKey`, `@PlayerInputActions`?**
  _High betweenness centrality (0.028) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `@PlayerInputActions` connect `@PlayerInputActions` to `InMemoryTransport`, `RuneBlade.Simulation`, `PlayerMovement`?**
  _High betweenness centrality (0.023) - this node is a cross-community bridge._
- **Should `PlayerMovement` be split into smaller, more focused modules?**
  _Cohesion score 0.0519774011299435 - nodes in this community are weakly interconnected._