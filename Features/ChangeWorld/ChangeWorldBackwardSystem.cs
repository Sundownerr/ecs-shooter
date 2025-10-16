using EcsMagic.PlayerComponenets;
using Game.Components;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // public sealed class ChangeWorldBackwardSystem : ISystem
    // {
    //     private readonly RuntimeData _runtimeData;
    //     private readonly StaticData _staticData;
    //     private Filter _filter;
    //
    //     public ChangeWorldBackwardSystem(StaticData staticData, RuntimeData runtimeData)
    //     {
    //         _runtimeData = runtimeData;
    //         _staticData = staticData;
    //     }
    //
    //     public void Dispose() { }
    //
    //     public void OnAwake() =>
    //         _filter = Entities.With<Player, PlayerInput_ChangeWorldBackward_WasPressed>();
    //
    //     public World World { get; set; }
    //
    //     public void OnUpdate(float deltaTime)
    //     {
    //         foreach (var entity in _filter) {
    //             ref var keyPressed = ref entity.GetComponent<PlayerInput_ChangeWorldBackward_WasPressed>();
    //
    //             if (keyPressed.Value) {
    //                 var levelIndex = _runtimeData.CurrentLevelIndex - 1;
    //
    //                 if (levelIndex >= 0)
    //                     Send.Signal(new Rq_CreateLevel {Index = levelIndex,});
    //             }
    //         }
    //     }
    // }
}