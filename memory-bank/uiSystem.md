# UI System: Swarm-2

## Overview
The UI system in Swarm-2 follows the Provider pattern used throughout the project. It consists of a central UiService that manages various UI providers and handles transitions between different game states. The system integrates with the game's ECS architecture through utility classes and the StaticStash pattern.

## UI Architecture
The UI architecture is built around the Provider pattern, with a central service managing multiple providers:

- **UiService**: Central manager that initializes and coordinates all UI providers
- **UI Providers**: MonoBehaviour classes that expose UI elements to the game systems
  - **PlayerUIProvider**: Displays player stats and crosshair
  - **PlayerScoreUiProvider**: Manages score display
  - **StartMenuUiProvider**: Handles the start menu interface
  - **GameOverUiProvider**: Manages the game over screen

The UiService maintains references to all providers and handles transitions between different UI states based on game events.

## UI Integration with Game Systems

### Level System Integration
- Level selection in the start menu
- Level loading/unloading triggered by UI buttons
- Using the StaticStash pattern to create level requests

### Player System Integration
- Displaying player stats (health, mana, velocity)
- Crosshair feedback for shooting and hitting targets
- Player destruction triggered by UI buttons

### Input System Integration
- Enabling/disabling input based on UI state
- Managing cursor lock state during different game phases

### Score System Integration
- Displaying score animations
- Updating total score
- Using pooling for efficient score UI elements

## UI Workflows

### Game Start Flow
- Initial UI state based on configuration
- Transition from start menu to gameplay
- Input and cursor state management

### Game Over Flow
- Transition to game over screen
- Options for retry or return to menu
- State management for UI elements

### Score Display Flow
- Score event triggering
- Score animation display
- Total score updates

## UI Patterns

### Provider Pattern
- UI elements accessed through provider classes
- Clear separation between UI and game logic

### Centralized Management
- UiService as the central point of control
- Consistent UI state transitions

### Animation System
- UI animations for feedback and polish
- Transitions between UI states

### Object Pooling
- Efficient reuse of UI elements for performance
- Applied to frequently created/destroyed elements

## Integration with ECS
- UI events trigger ECS actions through utility classes
- StaticStash pattern used to create requests
- UI providers stored in StaticData for global access

## Best Practices
- Keep UI logic separate from game logic
- Use the Provider pattern for UI access
- Centralize UI state management
- Use animation for feedback and transitions
- Implement pooling for frequently created UI elements
- Configure UI providers through StaticData