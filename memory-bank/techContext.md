# Technical Context: Swarm-2

## Technologies Used

### Core Technologies
- **Unity Game Engine**: The primary development platform
- **C#**: The programming language used throughout the project
- **Entity Component System (ECS)**: Custom implementation for game architecture

### Unity-Specific Technologies
- **NavMeshAgent**: Used for NPC navigation and pathfinding
- **Animator**: Handles character animations
- **Particle System**: Used for visual effects
- **Rigidbody**: Physics-based movement and interactions
- **LineRenderer**: Used for visual effects and UI elements
- **MMFeedbacks**: Likely a third-party asset for game feel and polish

### Custom Systems
- **Custom ECS**: A tailored Entity Component System implementation
- **State Machine**: Custom implementation for managing entity states
- **Stats System**: Flexible system for managing character statistics
- **Ability Framework**: Framework for creating and managing abilities
  - See [abilityFramework.md](./abilityFramework.md) for detailed documentation
  - Includes condition systems for various checks (distance, input, resources, velocity)
- **Action System**: Framework for defining and executing game actions
  - Includes configuration systems for transforms, rigidbodies, and other components
- **UI System**: Provider-based system for managing game UI elements
  - See [uiSystem.md](./uiSystem.md) for detailed documentation
  - Includes providers for player UI, score, start menu, and game over screens

## Development Setup

### Project Structure
- **Common/**: Core utilities and shared functionality
  - **Ecs/**: Custom ECS implementation
  - **State/**: State machine implementation
- **Features/**: Game features organized by functionality
  - **Ability/**: Ability-related systems
  - **Action/**: Action-related systems
  - **Condition/**: Condition checking systems
  - **PlayerResources/**: Player resource management
  - **StateMachine/**: State machine implementation
  - etc.
- **Game/**: Core game components and definitions
- **StaticData/**: Configuration and static game data
- **Stats/**: Statistics system implementation

### Naming Conventions
- **Components**: Data containers, typically named with nouns
- **Systems**: Logic processors, typically named with verbs or actions
- **Features**: Logical groupings of related systems
- **Configs**: Configuration data for systems
- **Providers**: Access points to important subsystems

## Technical Constraints

### ECS Architecture Constraints
- Systems should only operate on components, not directly on other systems
- Components should be data-only, with no behavior
- Entities are just IDs, with no inherent behavior

### Unity Integration Constraints
- Unity MonoBehaviours should be minimized and primarily used as entry points
- Game logic should reside in ECS systems, not in MonoBehaviours
- Unity components should be accessed through provider patterns when possible

### Performance Considerations
- Systems should be optimized for processing many entities efficiently
- Component data should be designed for cache-friendly access patterns
- Pooling should be used for frequently created/destroyed entities

## Dependencies

### Internal Dependencies
- Custom ECS framework
- State machine implementation
- Stats system
- Entity pooling system

### External Dependencies
- Unity Engine
- Possibly MMFeedbacks or similar third-party assets for game feel
- NavMesh system for pathfinding

## Build and Deployment

### Build Process
- Standard Unity build process
- Scenes and assets bundled according to Unity conventions

### Target Platforms
- Likely PC/desktop platforms (based on project structure)
- Possibly mobile platforms (would require specific optimizations)

### Performance Targets
- Efficient enough to handle many entities with complex behaviors
- Responsive input handling for player actions
- Smooth visual effects and animations

## Development Workflow

### Code Organization Principles
- New features should be placed in their own directory under Features/
- Component definitions should be placed in appropriate *Components.cs files
- Systems should follow the naming conventions (Action_, Check_, Init_, etc.)
- Configuration classes should be suffixed with Config

### Testing Approach
- Systems should be designed for testability
- Component-based design facilitates unit testing
- Separation of data and behavior improves test isolation

### Performance Considerations
- Use the StaticStash pattern for efficient component access
- Minimize GameObject creation/destruction during gameplay
- Use object pooling for frequently created/destroyed objects
- Design systems to operate efficiently on many entities

### Unity Integration
- Minimize MonoBehaviour usage
- Use Provider pattern to access Unity components
- Keep game logic in ECS systems, not MonoBehaviours
