# ECS Systems Communication Patterns

## Overview

The Swarm-2 project implements structured communication patterns between ECS systems through the Request and Event mechanisms. These patterns facilitate decoupled, efficient communication while maintaining the separation of concerns that is central to the ECS architecture.

## Request Pattern

The Request pattern provides a way for systems to request actions that should be processed by other systems. Requests persist in the world until explicitly completed.

### Implementation

Located in `Common/EcsSystemsCommunication/Request.cs`, the Request pattern is implemented as a static class with extension methods for Stash<T>:

```csharp
public static class Request
{
    private static World _world;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ClearOnReload() =>
        _world = null;

    public static void Initialize(World world) =>
        _world = world;

    public static Entity CreateRequest<T>(this Stash<T> stash, in T component) where T : struct, IComponent
    {
        var entity = _world.CreateEntity();
        stash.Set(entity, component);
        return entity;
    }

    public static Entity CreateRequest<T>(this Stash<T> stash) where T : struct, IComponent
    {
        var entity = _world.CreateEntity();
        stash.Add(entity);
        return entity;
    }

    public static void CompleteRequest(this Entity entity) =>
        _world.RemoveEntity(entity);
}
```

### Key Methods

1. **CreateRequest<T>(this Stash<T> stash, in T component)**
   - Creates a request entity with a specific component value
   - Returns the created entity for tracking

2. **CreateRequest<T>(this Stash<T> stash)**
   - Creates a request entity with default component value
   - Returns the created entity for tracking

3. **CompleteRequest(this Entity entity)**
   - Completes (removes) a request entity
   - Called when the request has been processed

### Usage Pattern

1. System A creates a request entity with a specific component:
   ```csharp
   var requestEntity = StaticStash.SomeRequestComponent.CreateRequest(new SomeRequestComponent { Value = 42 });
   ```

2. System B filters for entities with that component and processes them:
   ```csharp
   // In a system's OnUpdate method
   foreach (var entity in Filter)
   {
       ref var request = ref _someRequestComponent.Get(entity);
       // Process the request

       // Complete the request when done
       entity.CompleteRequest();
   }
   ```

### Benefits

- Decouples the system making the request from the system processing it
- Requests persist until explicitly completed, allowing for asynchronous processing
- Provides a structured way to queue work for other systems
- Returns the request entity, allowing the requester to track or modify it if needed

## Event Pattern

The Event pattern provides a way for systems to broadcast that something has happened. Events automatically exist for only one frame.

### Implementation

Located in `Common/EcsSystemsCommunication/Event.cs`, the Event pattern is implemented as a static class with extension methods for Stash<T>:

```csharp
public static class Event
{
    private static Stash<OneFrame> _oneFrame;
    private static World _world;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ClearOnReload() =>
        _world = null;

    public static void Initialize(World world)
    {
        _world = world;
        _oneFrame = world.GetStash<OneFrame>();
    }

    public static void CreateEvent<T>(this Stash<T> stash, in T component) where T : struct, IComponent
    {
        var entity = _world.CreateEntity();
        stash.Set(entity, component);
        _oneFrame.Add(entity);
    }

    public static void CreateEvent<T>(this Stash<T> stash) where T : struct, IComponent
    {
        var entity = _world.CreateEntity();
        stash.Add(entity);
        _oneFrame.Add(entity);
    }
}

[Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct OneFrame : IComponent { }
```

### Key Methods

1. **CreateEvent<T>(this Stash<T> stash, in T component)**
   - Creates an event entity with a specific component value
   - Adds the OneFrame component to mark it for automatic removal

2. **CreateEvent<T>(this Stash<T> stash)**
   - Creates an event entity with default component value
   - Adds the OneFrame component to mark it for automatic removal

### OneFrame Component

The OneFrame component is a special marker that indicates an entity should be automatically removed after one frame. This is used by the Event system to ensure events don't persist beyond their intended lifespan.

### Usage Pattern

1. System A creates an event entity with a specific component:
   ```csharp
   StaticStash.SomeEventComponent.CreateEvent(new SomeEventComponent { Value = 42 });
   ```

2. System B filters for entities with that component and reacts to them:
   ```csharp
   // In a system's OnUpdate method
   foreach (var entity in Filter)
   {
       ref var eventData = ref _someEventComponent.Get(entity);
       // React to the event

       // No need to manually remove the entity - it will be removed automatically
   }
   ```

### Benefits

- Simplifies event-based communication between systems
- Automatic cleanup prevents event accumulation
- Provides a structured way for systems to react to game events
- Fire-and-forget approach reduces boilerplate code

## Differences Between Requests and Events

| Aspect | Request | Event |
|--------|---------|-------|
| **Persistence** | Persists until explicitly completed | Exists for only one frame |
| **Return Value** | Returns the created entity | Void (fire-and-forget) |
| **Cleanup** | Manual via CompleteRequest() | Automatic via OneFrame component |
| **Use Case** | When processing needs to be tracked or is asynchronous | When systems need notification without response |

## Integration with ECS Architecture

Both patterns integrate seamlessly with the ECS architecture:
- They use entities and components for communication
- They follow the component-based design principles
- They maintain separation between systems
- They use the StaticStash pattern for component access

## Example Usage Scenarios

### Request Examples

1. **Ability Activation**:
   ```csharp
   // Request to activate an ability
   var requestEntity = StaticStash.ActivateAbilityRequest.CreateRequest(new ActivateAbilityRequest { AbilityId = 5 });

   // System that processes ability activation
   foreach (var entity in Filter)
   {
       ref var request = ref _activateAbilityRequest.Get(entity);
       // Activate the ability

       entity.CompleteRequest();
   }
   ```

2. **Resource Allocation**:
   ```csharp
   // Request to allocate mana
   var requestEntity = StaticStash.AllocateManaRequest.CreateRequest(new AllocateManaRequest { Amount = 25 });

   // System that processes mana allocation
   foreach (var entity in Filter)
   {
       ref var request = ref _allocateManaRequest.Get(entity);
       // Allocate mana

       entity.CompleteRequest();
   }
   ```

### Event Examples

1. **Damage Notification**:
   ```csharp
   // Create a damage event
   StaticStash.DamageDealtEvent.CreateEvent(new DamageDealtEvent { Amount = 10, TargetEntity = targetEntity });

   // System that reacts to damage events
   foreach (var entity in Filter)
   {
       ref var damageEvent = ref _damageDealtEvent.Get(entity);
       // React to the damage event (e.g., play sound, show visual effect)
   }
   ```

2. **State Change Notification**:
   ```csharp
   // Create a state change event
   StaticStash.StateChangedEvent.CreateEvent(new StateChangedEvent { OldState = oldState, NewState = newState });

   // System that reacts to state changes
   foreach (var entity in Filter)
   {
       ref var stateEvent = ref _stateChangedEvent.Get(entity);
       // React to the state change (e.g., update UI, play transition animation)
   }
   ```

## Best Practices

1. **Use Requests When**:
   - The operation might take multiple frames to complete
   - The requester needs to track the status of the request
   - The operation might need to be cancelled
   - The operation requires asynchronous processing

2. **Use Events When**:
   - You need to broadcast that something has happened
   - Multiple systems might need to react to the same event
   - The notification doesn't require a response
   - The event is instantaneous and doesn't need tracking

3. **Component Naming Conventions**:
   - Request components should be suffixed with "Request" (e.g., `ActivateAbilityRequest`)
   - Event components should be suffixed with "Event" (e.g., `DamageDealtEvent`)

4. **Initialization**:
   - Both Request and Event static classes must be initialized with the ECS World
   - This is typically done in the EntryPoint class during system initialization

5. **Component Access**:
   - Always use the StaticStash pattern for accessing request and event components
   - Follow the Component Access Pattern guidelines

## Conclusion

The Request and Event patterns provide powerful, structured communication mechanisms for the ECS architecture in Swarm-2. By using these patterns, systems can communicate effectively while maintaining separation of concerns and adhering to ECS principles.