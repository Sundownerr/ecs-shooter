# 🎮 ECS Shooter

A shooter game with 1000+ enemies built with [Morpeh ECS](https://github.com/scellecs/morpeh) framework for action-packed high-performance gameplay.


https://github.com/user-attachments/assets/49e4bfb5-8865-4397-a664-78482c4abc0c


## Features

- ECS architecture with feature-based organization
- Modular ability framework for extensible gameplay actions
- Data-oriented component design
- Separation of game logic into independent features

## Project Structure

| Folder / File | Purpose |
|--------------|---------|
| `AbilityFramework/` | Base logic for abilities and gameplay actions |
| `Common/` | Shared helpers and utility code |
| `Data/` | ECS components and pure data structs |
| `Features/` | Independent gameplay modules (movement, shooting, input, etc.) |
| `Game/` | Initialization and main entry point |
| `Providers/` | Interfaces and wrappers for Unity services (input, addressables, etc.) |
| `EcsFeatures.cs` | Registers and manages ECS systems and features |
| `EntryPoint.cs` | Project bootstrap and initialization logic |
