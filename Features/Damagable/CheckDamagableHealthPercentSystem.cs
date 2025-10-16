using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // public sealed class CheckDamagableHealthPercentSystem : ISystem
    // {
    //     private Filter _filter;
    //
    //     public void Dispose() { }
    //
    //     public void OnAwake() => _filter = Entities.With<DamageInstancesList>();
    //
    //     public World World { get; set; }
    //
    //     public void OnUpdate(float deltaTime)
    //     {
    //         // foreach (var entity in _filter) {
    //         //     ref var damageInstance = ref entity.GetComponent<DamageInstancesList>();
    //         //     
    //         //     if (!damageInstance.Target.Has<Damagable>())
    //         //         continue;
    //         //
    //         //     var damagableEntity = damageInstance.Target;
    //         //     
    //         //     ref var health = ref damagableEntity.GetComponent<Health>();
    //         //     ref var maxHealth = ref damagableEntity.GetComponent<MaxHealth>();
    //         //     
    //         //     var currentHealthPercent = (float) health.Value / maxHealth.Value;
    //         //
    //         //     ref var damagable = ref damagableEntity.GetComponent<Damagable>();
    //         //
    //         //     for(var i = damagable.Stage; i < damagable.Instance.HealthPercentActions.Count; i++) {
    //         //         var action = damagable.Instance.HealthPercentActions[i];
    //         //
    //         //         if (action.TriggerPercent > currentHealthPercent) {
    //         //             damagable.Stage = i + 1;
    //         //             action.Ability.Activate();
    //         //             
    //         //             Debug.Log( $"Health percent action {action.TriggerPercent} activated");
    //         //         }
    //         //     }
    //         // }
    //     }
    // }
}