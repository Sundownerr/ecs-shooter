# System Patterns: Swarm-2

## System Architecture
Swarm-2 is built on an Entity Component System (ECS) architecture, which separates data (components) from logic (systems). This architecture provides performance benefits and clear separation of concerns.

### Core Architecture Components
1. **Entities**: Game objects represented as entity IDs
2. **Components**: Data containers attached to entities
3. **Systems**: Logic that processes entities with specific components
4. **Features**: Logical groupings of related systems

## Key Technical Decisions

### ECS Implementation
- Custom ECS implementation with features for pooling and reuse
- Systems organized into features for better maintainability
- Clear separation between data (components) and behavior (systems)

### State Machine Pattern
- Robust state machine implementation for managing entity states
- State transitions based on conditions
- Actions triggered by state changes
- Support for complex behavior through state combinations

### Factory Pattern
- Factory systems for creating complex entities (Player, NPC, Abilities)
- Factories handle the creation and initialization of entities with appropriate components

### Provider Pattern
- Provider classes serve as access points to important game subsystems
- Examples: PlayerProvider, NpcProvider, AbilityProvider, StateMachineProvider

### Component-Based Design
- Game functionality broken down into small, reusable components
- Systems operate on specific component combinations
- Clear interfaces between different parts of the system

## Design Patterns in Use

### Command Pattern
- Actions represent commands that can be executed on entities
- Action configurations define parameters for actions
- Action systems execute the commands based on configurations

### Observer Pattern
- Systems react to changes in component data
- Events trigger appropriate responses in relevant systems

### Strategy Pattern
- Different implementations of similar behaviors (e.g., movement types, ability effects)
- Behavior selected based on configuration or state

### Composite Pattern
- Complex behaviors built from combinations of simpler behaviors
- Actions and conditions can be combined to create complex game mechanics

## Component Access Pattern

A critical pattern in the ECS architecture of this project is how components are accessed. All component access must be done through stashes, with no direct component manipulation allowed.

1. **Stash Types**:
   - **System-Level Stashes**: Private fields in systems, initialized in OnAwake
   ```csharp
   private Stash<VelocityGreaterThan> _velocityGreaterThan;
   ```
   - **Static Stashes**: Shared access points in StaticStash class
   ```csharp
   public static Stash<UsageProgressPart> UsageProgressPart;
   public static Stash<UsageConfigs> UsageConfigs;
   ```

2. **Initialization Requirements**:
   - System stashes: Initialize in the `OnAwake` method
   - Static stashes: Initialize in the `Initialize` method
   - All stashes must be initialized before use

3. **Usage Patterns**:
   - Use stash methods instead of direct entity methods:
   ```csharp
   // CORRECT - Using stash methods
   _stash.Add(entity);                    // Instead of entity.AddComponent<T>()
   _stash.Remove(entity);                 // Instead of entity.RemoveComponent<T>()
   _stash.Has(entity);                    // Instead of entity.Has<T>()
   ref var component = ref _stash.Get(entity);  // Instead of entity.GetComponent<T>()
   _stash.Set(entity, new Component());   // Instead of entity.SetComponent(...)

   // Example from AbilityUsage.cs
   StaticStash.UsageProgressPart.Set(usageEntity, new UsageProgressPart());
   StaticStash.UsageConfigs.Set(parentEntity, new UsageConfigs { List = UsageEntries });
   ```

4. **Benefits**:
   - Improved performance through optimized component access
   - Consistent access patterns across the codebase
   - Better code organization and maintainability
   - Clearer component dependencies

## Component Relationships

### Player Systems
- PlayerFactory creates player entities
- PlayerInput handles user input
- PlayerMovement manages movement based on input
- PlayerResources manages health, mana, and other resources
- PlayerUI provides feedback to the user

### UI Systems
- UiService manages UI providers and state transitions
- UI Providers expose UI elements to game systems
- For detailed documentation on the UI system, see [uiSystem.md](./uiSystem.md)

### Ability Systems
- AbilityProvider manages available abilities
- AbilityCondition checks if abilities can be used
  - Conditions include distance, input, resource, and velocity-based checks
  - ConditionFulfilled component tracks when conditions are met
- AbilityCooldown manages ability cooldowns
- AbilityTarget handles targeting for abilities
- AbilityAction defines what happens when abilities are used

### AbilityActionNames Mapping Pattern

The AbilityActionNames.cs file implements a critical mapping system for ability actions:

1. **Structure**: The mapping uses a nested dictionary structure:
   ```csharp
   Dictionary<int, Dictionary<int, Type>> NameToType
   ```
   - The outer dictionary key is the ActionId for the action group (Create, Destroy, Enable, SetActive)
   - The inner dictionary key is the ActionId for the action name (GameObject, NavMeshAgent)
   - The inner dictionary value is the Type of the configuration class for that combination

2. **Mapping Process**:
   - Each ability action is categorized by its group (what kind of action it is) and its target (what it acts upon)
   - For example:
     - Create + GameObject → CreateGameObjectConfig
     - Destroy + GameObject → DestroyGameObjectConfig
     - Enable + NavMeshAgent → NavMeshAgentConfig
     - SetActive + GameObject → SetGameObjectActiveConfig

3. **Usage**:
   - This mapping is used by the ability system to determine which configuration type to use for a given action
   - The NamesInGroup and Groups methods provide dropdown options in the Unity editor for selecting actions
   - The ConvertToActionName method converts ActionId constants to human-readable strings

4. **Extension Process**:
   - To add a new action group: Add a new constant to ActionId, add a string to ActionName, update ConvertToActionName, and add a new entry to the NameToType dictionary
   - To add a new action target: Add a new constant to ActionId, add a string to ActionName, update ConvertToActionName, and add a new entry to the appropriate inner dictionary in NameToType

### Action Systems
- Actions define behaviors that can be triggered
- ActionConfig provides configuration for actions
  - Includes TransformConfig, RigidbodyTargetConfig for targeting
- ActionInitialize sets up actions when they start
- ActionUsage manages ongoing actions
- ActionCancel handles interruption of actions

### State Machine Systems
- StateMachineProvider manages state machines
- States define behavior modes for entities
- Transitions move between states based on conditions
- Actions triggered by state changes

### Condition Systems
- Conditions check if certain criteria are met
- Used for ability activation, state transitions, etc.
- Various condition types for different game mechanics:
  - Spatial conditions (distance, raycast, sphere checks)
  - Input conditions (button presses, releases)
  - Resource conditions (mana, health)
  - Physics conditions (velocity checks)
  - GameObject state conditions (active/inactive checks)
- Condition fulfillment is tracked through the ConditionFulfilled component

### Ability Framework
The Ability Framework is a core system that enables complex ability behaviors. For detailed documentation, see [abilityFramework.md](./abilityFramework.md).

Key integration points:
- Uses the Component Access Pattern through StaticStash
- Implements the Command Pattern for ability actions
- Leverages the State Machine Pattern for ability states

## System Initialization Flow

The initialization flow in Swarm-2 follows a specific order to ensure dependencies are properly set up:

1. **World Creation**: The ECS world is created first
2. **Static Stash Initialization**: The StaticStash is initialized to provide global component access
3. **Feature Registration**: Features are registered with the world
4. **System Registration**: Systems are registered within their features
5. **Provider Initialization**: Providers are initialized to bridge Unity and ECS
6. **Entity Creation**: Factory systems create initial entities

This flow ensures that all systems have access to their required dependencies when they begin execution.

## System Communication Patterns

Systems in Swarm-2 communicate primarily through components:

1. **Component-Based Communication**: Systems communicate by adding, removing, or modifying components on entities
2. **State-Based Communication**: State changes trigger actions and transitions
3. **Condition Fulfillment**: Conditions track when criteria are met through the ConditionFulfilled component
4. **Provider-Based Access**: Providers give systems access to shared resources and Unity objects

These communication patterns maintain the separation between systems while allowing for complex interactions.

5. **Request and Event Communication**: Structured patterns for system-to-system communication
   - See [ecsSystemsCommunication.md](./ecsSystemsCommunication.md) for detailed documentation
   - Requests: Long-lived entities for asynchronous processing
   - Events: Single-frame entities for broadcasting notifications
