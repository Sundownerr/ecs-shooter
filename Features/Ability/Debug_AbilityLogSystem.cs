using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Debug_AbilityLogSystem : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<AbilityState_Using>().With<Debug_AbilityLog>().Without<Cancelled>().Build();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var debugAbilityLog = ref entity.GetComponent<Debug_AbilityLog>();
                Debug.Log(debugAbilityLog.AbilityProvider.name, debugAbilityLog.AbilityProvider);
            }
        }
    }
}