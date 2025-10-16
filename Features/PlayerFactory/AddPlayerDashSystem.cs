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
    // public sealed class AddPlayerDashSystem : ISystem
    // {
    //     private readonly StaticData _staticData;
    //     private Filter _filter;
    //     private Filter _playerFilter;
    //
    //     public AddPlayerDashSystem(StaticData staticData)
    //     {
    //         _staticData = staticData;
    //     }
    //
    //     public void Dispose() { }
    //
    //     public void OnAwake()
    //     {
    //         _filter = Entities.With<Request_CreatePlayer>();
    //         _playerFilter = Entities.With<Player>();
    //     }
    //
    //     public World World { get; set; }
    //
    //     public void OnUpdate(float deltaTime)
    //     {
    //         foreach (var entity in _filter) {
    //             foreach (var playerEntity in _playerFilter) {
    //                 // var dashEntity = World.CreateEntity();
    //                 // ref var player = ref playerEntity.GetComponent<Player>();
    //                 //
    //                 // ref var dashAbility = ref dashEntity.AddComponent<PlayerDashAbility>();
    //                 // dashAbility.Player = playerEntity;
    //                 // dashAbility.Force = player.Instance.DashForce;
    //                 //
    //                 // ref var timer = ref dashEntity.AddComponent<IncreasingTimer>();
    //                 // timer.Duration = player.Instance.DashCooldown;
    //                 //
    //                 // dashEntity.AddComponent<TimerCompleted>();
    //                 // dashEntity.AddComponent<PlayerInput_Dash_WasPressed>();
    //
    //             }
    //         }
    //     }
    // }
}