# Tidefall Colony

A compact Unity city-builder / level design prototype built around a simple spatial risk-reward mechanic:

> **Low ground provides better productivity, but is periodically flooded. High ground is safer, but less efficient.**

The project was designed as a short 5-minute playable scenario focused on **level design, gameplay readability, progression, environmental risk and simple systemic interactions**.

![Tidefall Colony Overview](docs/images/01_overview.png)

## Project Goals

Tidefall Colony was created as a focused portfolio project demonstrating:

- Level and spatial design in Unity
- Risk/reward design based on terrain elevation
- Objective-driven scenario progression
- Simple resource economy and production systems
- Environmental hazards affecting gameplay
- Visual Scripting used for level-state logic
- C# gameplay systems and UI
- Iteration from greybox to a playable polished prototype

The scope was intentionally kept small so the project could focus on clear design decisions rather than content volume.

## Core Gameplay Loop

The player develops a small settlement while preparing for recurring floods.

The scenario progression is:

**Build Lumber Camp → Expand storage → Build Flood Watchtower → Build Quarry → Survive Flood → Repair Crossing → Establish Food Production → Survive Second Flood → Activate Ancient Beacon**

Resources are:

- **Wood** — construction and early progression
- **Stone** — infrastructure and crossing repair
- **Food** — final settlement objective

The initial storage capacity is limited to **20 resources**. Building a Warehouse increases the capacity to **50**, making infrastructure expansion part of the progression rather than a passive upgrade.

## Flood System

Flooding is the central environmental mechanic.

The scenario cycles through:

**SAFE → FLOOD WARNING → FLOODED → WATER RECEDING**

![Flood Warning](docs/images/02_flood_warning.png)

The Flood Watchtower provides additional warning time before the flood begins, turning information and preparation time into a gameplay resource.

During a flood, the lower part of the map becomes submerged while elevated areas remain safe.

![Flooded Lowland](docs/images/03_flooded_lowland.png)

The flood cycle and level-state transitions are implemented using **Unity Visual Scripting**.

## Spatial Risk / Reward

The map is divided vertically into safe and dangerous terrain.

Players eventually choose between two approaches to food production:

**High Ground Farm**
- Lower production rate
- Safe from flooding
- No repair costs

**Lowland Farm**
- Higher production rate
- Located in the flood zone
- Becomes damaged during floods
- Must be repaired for 50% of its original construction cost

This creates a simple but visible trade-off between **efficiency and resilience**.

![Crossing and Farms](docs/images/04_crossing_and_farms.png)

The Broken Crossing also acts as a progression gate. Repairing it opens access to the food-production stage of the scenario and visually connects previously separated gameplay areas.

## Building Interaction & Feedback

Building plots communicate their current state through materials and contextual UI.

Plots can be:

**Locked → Available → Built → Damaged**

Hovering over a plot displays:

- Building name
- Construction or repair cost
- Availability state
- Resource requirements

Damaged Lowland Farms expose the repair system directly through the same interaction.

![Building Tooltip](docs/images/05_building_tooltip.png)

## Scenario Completion

The final objective is to restore the Ancient Beacon after establishing a functioning settlement and surviving the required flood cycles.

![Scenario Complete](docs/images/06_scenario_complete.png)

The complete scenario takes approximately **5 minutes** during a normal playthrough.

## Technical Implementation

**Engine:** Unity 6 (6000.5.9f1) 
**Render Pipeline:** Universal Render Pipeline (URP)  
**Programming:** C#  
**Level Logic:** Unity Visual Scripting  
**Input:** Unity Input System  
**UI:** TextMeshPro  

Key systems include:

- Objective progression system
- Resource production and spending
- Dynamic resource storage capacity
- Building plot locking and unlocking
- Contextual building tooltips
- Flood state machine
- Flood warning extension from Watchtower
- Flood-sensitive building production
- Building damage and repair
- Scenario completion and restart
- RTS-style camera movement and zoom

## Level Design Process

The level was first created as a functional greybox focused on:

- clear elevation differences,
- readable routes between gameplay zones,
- landmark placement,
- progression gating,
- flood-safe and flood-risk areas,
- sightlines and visual guidance.

The final environment art pass preserved the original greybox layout and gameplay collision while replacing or decorating the level with stylized low-poly assets.

This allowed the visual presentation to improve without changing the tested spatial structure of the level.

## Controls

| Input | Action |
|---|---|
| **WASD** | Move camera |
| **Mouse Wheel** | Zoom |
| **Mouse Hover** | Inspect building plot |
| **Left Mouse Button** | Build / repair |

## Art & Third-Party Assets

The environment uses selected free low-poly assets from **Polyfork**, combined with custom materials, layout work and additional prototype assets created specifically for Tidefall Colony.

Polyfork assets are used under the Polyfork license:

https://polyfork.dev/licensing

The original Polyfork model files are **not included in this public source repository**, as their license does not allow redistribution of the standalone asset files.

Playable builds may include these assets as part of the finished game.

For more information, see [`THIRD_PARTY_NOTICES.md`](THIRD_PARTY_NOTICES.md).

## Repository Structure

```text
Assets/
├── Art/                 # Environment and gameplay art
├── Materials/           # Level and gameplay materials
├── Scenes/              # Greybox and final prototype scenes
├── Scripts/
│   ├── Building/
│   ├── Camera/
│   ├── Objectives/
│   ├── Resources/
│   └── UI/
└── VisualScripting/     # Flood / level-state graph

docs/
├── design/              # Design documentation
└── images/              # Portfolio screenshots
```

## Source Project

The repository contains the Unity project, gameplay systems, scenes, scripts and design logic used to develop Tidefall Colony.

The project was developed with:

**Unity 6 (6000.5.9f1)**

The main prototype scene is:

`Assets/Scenes/TidefallColony_Prototype.unity`

Some third-party visual assets used in the final presentation are intentionally excluded from the public repository due to their redistribution license.

As a result, cloning the repository does not reproduce the complete final visual presentation without restoring the required third-party assets.

For the complete playable version, use one of the published builds.

## Design Focus

Tidefall Colony is not intended to be a full city-builder.

It is a small, deliberately scoped prototype exploring how **terrain, resource constraints, environmental pressure and level progression can combine to produce meaningful decisions with a limited number of systems**.

## License

Original source code created for Tidefall Colony is available under the [MIT License](LICENSE).

Third-party assets are not covered by the MIT License. See [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) for details.

## Author

**Alan Pawleta**
