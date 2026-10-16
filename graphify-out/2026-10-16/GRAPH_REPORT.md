# Graph Report - Portfolio_2D  (2026-10-15)

## Corpus Check
- 22 files · ~39,023 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 154 file(s) not represented in the graph (top: .meta 96, .asset 35, .anim 10)

## Summary
- 775 nodes · 1139 edges · 47 communities
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 65 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `f03260f4`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- manifest.json
- dependencies
- PlayerMovement
- com.unity.modules.jsonserialize
- KeyInputEvent
- LogicalKey
- @PlayerInputActions
- packages-lock.json
- com.unity.burst
- com.unity.render-pipelines.core
- RuneBlade.Simulation
- DoubleTapTracker
- com.unity.modules.physics
- com.unity.settings-manager
- com.unity.2d.tilemap
- com.unity.modules.animation
- dependencies
- com.unity.mathematics
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
- com.unity.modules.physicscore2d
- com.unity.splines
- com.unity.2d.common
- com.unity.2d.sprite
- com.unity.cinemachine
- com.unity.modules.accessibility
- com.unity.modules.ai
- com.unity.modules.hierarchycore
- com.unity.modules.imgui
- com.unity.modules.particlesystem
- com.unity.modules.physics2d
- com.unity.modules.tetgen
- com.unity.modules.ui
- com.unity.modules.uielements
- com.unity.modules.umbra
- com.unity.modules.unitywebrequestaudio
- dependencies
- com.unity.modules.vectorgraphics
- com.unity.modules.wind

## God Nodes (most connected - your core abstractions)
1. `@PlayerInputActions` - 29 edges
2. `DoubleTapTracker` - 29 edges
3. `PlayerMovement` - 29 edges
4. `LogicalKey` - 25 edges
5. `InputState` - 25 edges
6. `KeyInputEvent` - 24 edges
7. `PlayerMovementController` - 21 edges
8. `MovementSettings` - 17 edges
9. `TickInput` - 17 edges
10. `FixedPoint` - 17 edges

## Surprising Connections (you probably didn't know these)
- `PlayerMovementController` --references--> `PlayerMovementSettingSO`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Game/Player/ScriptableObject/PlayerMovementSettingSO.cs
- `PlayerMovementController` --references--> `MovementSettings`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Movement/MovementSettings.cs
- `PlayerMovementController` --references--> `LogicalKey`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Input/Record/LogicalKey.cs
- `PlayerMovementController` --references--> `PlayerMovement`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/PlayerMovementController.cs → Assets/RuneBlade/Scripts/Simulation/Player/PlayerMovement.cs
- `PlayerMovementSettingSO` --references--> `MovementSettings`  [EXTRACTED]
  Assets/RuneBlade/Scripts/Game/Player/ScriptableObject/PlayerMovementSettingSO.cs → Assets/RuneBlade/Scripts/Simulation/Input/Movement/MovementSettings.cs

## Import Cycles
- None detected.

## Communities (47 total, 0 thin omitted)

### Community 0 - "manifest.json"
Cohesion: 0.04
Nodes (55): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+47 more)

### Community 1 - "dependencies"
Cohesion: 0.04
Nodes (56): dependencies, com.unity.2d.animation, com.unity.2d.aseprite, com.unity.2d.psdimporter, com.unity.2d.sprite, com.unity.2d.spriteshape, com.unity.2d.tilemap, com.unity.2d.tilemap.extras (+48 more)

### Community 2 - "PlayerMovement"
Cohesion: 0.06
Nodes (20): JumpInputBuffer, IsStored, RemainingTicks, MovementSettings, DashSpeedPerTick, GravityPerTick, JumpVelocityPerTick, MoveSpeedPerTick (+12 more)

### Community 3 - "com.unity.modules.jsonserialize"
Cohesion: 0.05
Nodes (45): dependencies, depth, source, version, dependencies, depth, source, version (+37 more)

### Community 4 - "KeyInputEvent"
Cohesion: 0.06
Nodes (13): IPlayerActions, PlayerMovementController, KeyInputEvent, IsPress, IsRelease, IsValid, Key, Transition (+5 more)

### Community 5 - "LogicalKey"
Cohesion: 0.12
Nodes (17): LogicalKey, Down, Left, Right, Slot1, Slot2, Slot3, Slot4 (+9 more)

### Community 6 - "@PlayerInputActions"
Cohesion: 0.07
Nodes (13): PlayerActions, @DropDown, enabled, @JumpUp, @LeftMove, @RightMove, @PlayerInputActions, asset (+5 more)

### Community 7 - "packages-lock.json"
Cohesion: 0.05
Nodes (40): com.unity.2d.sprite, com.unity.2d.tilemap, com.unity.modules.animation, com.unity.modules.assetbundle, com.unity.modules.audio, com.unity.modules.director, com.unity.modules.imageconversion, com.unity.modules.imgui (+32 more)

### Community 8 - "com.unity.burst"
Cohesion: 0.05
Nodes (39): dependencies, depth, source, version, dependencies, dependencies, depth, source (+31 more)

### Community 9 - "com.unity.render-pipelines.core"
Cohesion: 0.06
Nodes (37): dependencies, depth, source, version, dependencies, depth, source, url (+29 more)

### Community 10 - "RuneBlade.Simulation"
Cohesion: 0.08
Nodes (10): PlayerMovementSettingSO, Settings, KeyTransition, Press, Release, Welcome2DScript, RuneBlade.Game, RuneBlade.Simulation (+2 more)

### Community 11 - "DoubleTapTracker"
Cohesion: 0.20
Nodes (5): DoubleTapTracker, IsActive, Key, RemainingTicks, DoubleTapTrackerTests

### Community 12 - "com.unity.modules.physics"
Cohesion: 0.08
Nodes (25): dependencies, depth, source, version, dependencies, depth, source, version (+17 more)

### Community 13 - "com.unity.settings-manager"
Cohesion: 0.12
Nodes (17): dependencies, depth, source, version, dependencies, depth, source, url (+9 more)

### Community 14 - "com.unity.2d.tilemap"
Cohesion: 0.13
Nodes (16): dependencies, depth, dependencies, depth, source, url, version, source (+8 more)

### Community 15 - "com.unity.modules.animation"
Cohesion: 0.14
Nodes (15): dependencies, depth, source, version, dependencies, depth, source, version (+7 more)

### Community 16 - "dependencies"
Cohesion: 0.20
Nodes (11): dependencies, depth, source, version, dependencies, depth, source, version (+3 more)

### Community 17 - "com.unity.mathematics"
Cohesion: 0.20
Nodes (10): dependencies, depth, source, version, dependencies, depth, source, version (+2 more)

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

### Community 28 - "com.unity.modules.physicscore2d"
Cohesion: 0.33
Nodes (6): dependencies, dependencies, depth, source, version, com.unity.modules.physicscore2d

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

### Community 33 - "com.unity.modules.accessibility"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.accessibility

### Community 34 - "com.unity.modules.ai"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.ai

### Community 35 - "com.unity.modules.hierarchycore"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.hierarchycore

### Community 36 - "com.unity.modules.imgui"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.imgui

### Community 37 - "com.unity.modules.particlesystem"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.particlesystem

### Community 38 - "com.unity.modules.physics2d"
Cohesion: 0.40
Nodes (5): depth, source, version, dependencies, com.unity.modules.physics2d

### Community 39 - "com.unity.modules.tetgen"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.tetgen

### Community 40 - "com.unity.modules.ui"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.ui

### Community 41 - "com.unity.modules.uielements"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.uielements

### Community 42 - "com.unity.modules.umbra"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.umbra

### Community 43 - "com.unity.modules.unitywebrequestaudio"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.unitywebrequestaudio

### Community 44 - "dependencies"
Cohesion: 0.40
Nodes (5): depth, source, version, dependencies, com.unity.modules.unitywebrequestwww

### Community 45 - "com.unity.modules.vectorgraphics"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.vectorgraphics

### Community 46 - "com.unity.modules.wind"
Cohesion: 0.40
Nodes (5): dependencies, depth, source, version, com.unity.modules.wind

## Knowledge Gaps
- **149 isolated node(s):** `asset`, `bindingMask`, `devices`, `controlSchemes`, `bindings` (+144 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 513 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `dependencies` connect `dependencies` to `com.unity.modules.jsonserialize`, `packages-lock.json`, `com.unity.burst`, `com.unity.render-pipelines.core`, `com.unity.modules.physics`, `com.unity.settings-manager`, `com.unity.2d.tilemap`, `com.unity.modules.animation`, `dependencies`, `com.unity.mathematics`, `com.unity.modules.audio`, `com.unity.modules.imageconversion`, `com.unity.modules.unitywebrequest`, `dependencies`, `dependencies`, `com.unity.2d.psdimporter`, `com.unity.2d.spriteshape`, `com.unity.2d.tooling`, `com.unity.collab-proxy`, `com.unity.inputsystem`, `com.unity.modules.physicscore2d`, `com.unity.splines`, `com.unity.2d.common`, `com.unity.2d.sprite`, `com.unity.cinemachine`, `com.unity.modules.accessibility`, `com.unity.modules.ai`, `com.unity.modules.hierarchycore`, `com.unity.modules.imgui`, `com.unity.modules.particlesystem`, `com.unity.modules.physics2d`, `com.unity.modules.tetgen`, `com.unity.modules.ui`, `com.unity.modules.uielements`, `com.unity.modules.umbra`, `com.unity.modules.unitywebrequestaudio`, `com.unity.modules.vectorgraphics`, `com.unity.modules.wind`?**
  _High betweenness centrality (0.280) - this node is a cross-community bridge._
- **Are the 13 inferred relationships involving `DoubleTapTracker` (e.g. with `.AfterRecognition_ThirdPressStartsNewRecord()` and `.Boundary_PressAfterWindow_IsNotRecognized_AndBecomesNewFirstPress()`) actually correct?**
  _`DoubleTapTracker` has 13 INFERRED edges - model-reasoned connections that need verification._
- **What connects `asset`, `bindingMask`, `devices` to the rest of the system?**
  _149 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `manifest.json` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `PlayerMovementController` connect `KeyInputEvent` to `RuneBlade.Simulation`, `PlayerMovement`, `LogicalKey`, `@PlayerInputActions`?**
  _High betweenness centrality (0.031) - this node is a cross-community bridge._
- **Should `dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.03571428571428571 - nodes in this community are weakly interconnected._
- **Why does `PlayerMovement` connect `PlayerMovement` to `RuneBlade.Simulation`, `DoubleTapTracker`, `KeyInputEvent`, `LogicalKey`?**
  _High betweenness centrality (0.022) - this node is a cross-community bridge._