using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // public sealed class GameOverRetrySystem : ISystem
    // {
    //     private readonly FPSInput _input;
    //     private Filter _filter;
    //
    //     public GameOverRetrySystem(FPSInput input)
    //     {
    //         _input = input;
    //     }
    //
    //     public void Dispose() { }
    //
    //     public void OnAwake() =>
    //         _filter = Entities.With<Event_GameOverContinuePressed>();
    //
    //     public World World { get; set; }
    //
    //     public void OnUpdate(float deltaTime)
    //     {
    //         foreach (var entity in _filter) {
    //             LevelsUtility.NotifyLoadingNewLevel(0);
    //             PlayerUtility.DestroyPlayer();
    //             _input.Enable();
    //         }
    //     }
    // }
}