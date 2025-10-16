using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.StateMachineComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddFullfilledCancelConditionsSystem : ISystem
    {
        private Filter _filter;
        private Stash<ParentEntity> _parentEntity;
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Stash<CancelConditionsToMeet> _cancelConditionsToMeet;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ConditionFulfilled, CancellingCondition>();
            _parentEntity = World.GetStash<ParentEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
            _cancelConditionsToMeet = World.GetStash<CancelConditionsToMeet>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var parent = ref _parentEntity.Get(entity);
                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);

                ref var conditionsToMeet = ref _cancelConditionsToMeet.Get(parent.Entity);
                conditionsToMeet.Remaining -= conditionFulfilled.Value;
            }
        }
    }
}
