using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using SDW.EcsMagic.Triggers;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_TriggerStaySystem : ISystem
    {
        private Filter _filter;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<TriggerConditionStay, NotifyTriggerStay, Active>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                conditionFulfilled.Value++;
            }
        }
    }
}
