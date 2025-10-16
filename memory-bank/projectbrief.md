# Project Brief: Swarm-2

## Project Overview
Swarm-2 is a game built on a custom Entity Component System (ECS) architecture that focuses on ability-based gameplay. The game appears to feature player characters with various abilities, resources (health and mana), movement mechanics (basic movement, dash, sprint), and combat systems.

## Core Requirements
1. **Flexible Ability System**: A comprehensive framework for creating and managing complex game abilities
2. **Responsive Player Controls**: Smooth and responsive player movement and ability activation
3. **Resource Management**: Systems for managing player resources like health and mana
4. **Combat Mechanics**: Systems for dealing and receiving damage, weapon usage, and targeting
5. **NPC Behavior**: Systems for NPC movement, actions, and AI
6. **Performance Optimization**: Efficient ECS architecture to handle many entities with complex behaviors

## Main Features

### Player Systems
- Character movement with multiple movement types (basic, dash, sprint)
- Resource management (health, mana)
- Ability activation and usage
- Input handling and mapping

### UI System
- Provider-based UI architecture
- Player stats and feedback display
- Menu and game state management
- Score visualization

### Ability Framework
- Flexible system for defining and executing abilities
- Condition-based activation
- Cooldown management
- Targeting systems
- Action execution

### Combat System
- Damage dealing and receiving
- Weapon mechanics
- Targeting and hit detection

### NPC Systems
- NPC spawning and factory systems
- Movement and pathfinding
- Action selection and execution

### State Machine
- State-based behavior for entities
- Condition-based transitions
- Action triggering on state changes

## Technical Goals
1. Maintain clean separation between data (components) and behavior (systems)
2. Organize code by feature rather than technical layer
3. Implement consistent patterns for component access and system design
4. Ensure performance through optimized ECS architecture
5. Create reusable and modular systems

## Project Scope
The project appears to be a game with:
- 3D environment (based on references to transforms, rigidbodies, etc.)
- Ability-based gameplay
- Resource management
- Combat mechanics
- NPC interactions

## Development Approach
- Feature-driven development
- Strong emphasis on architectural patterns
- Clear separation of concerns
- Reusable and modular systems
- Performance-focused implementation