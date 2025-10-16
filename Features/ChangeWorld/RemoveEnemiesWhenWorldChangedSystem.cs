using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // public sealed class RemoveEnemiesWhenWorldChangedSystem : ISystem
    // {
    //     private Filter _currentLevelFilter;
    //     private Filter _enemiesFilter;
    //     private Filter _filter;
    //     private Filter _eventGameOverContinuePressedFilter;
    //     private Stash<WillBeDestroyed> _willBeDestroyed;
    //
    //     public void Dispose() { }
    //
    //     public void OnAwake()
    //     {
    //         _filter = Entities.With<Event_LevelChanged>();
    //         _eventGameOverContinuePressedFilter = Entities.With<Event_GameOverContinuePressed>();
    //         _enemiesFilter = Entities.With<Npc>();
    //         _willBeDestroyed = World.GetStash<WillBeDestroyed>();
    //     }
    //
    //     public World World { get; set; }
    //
    //     public void OnUpdate(float deltaTime)
    //     {
    //         foreach (var entity in _filter)
    //             MarkEnemiesToDestroy();
    //
    //         foreach (var entity in _eventGameOverContinuePressedFilter)
    //             MarkEnemiesToDestroy();
    //     }
    //
    //     private void MarkEnemiesToDestroy()
    //     {
    //         foreach (var enemyEntity in _enemiesFilter)
    //             _willBeDestroyed.Add(enemyEntity);
    //     }
    // }
}
