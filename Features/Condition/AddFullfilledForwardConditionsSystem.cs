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
    public sealed class AddFullfilledForwardConditionsSystem : ISystem
    {
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Filter _filter;
        private Stash<ForwardConditionsToMeet> _forwardConditionsToMeet;
        private Stash<ParentEntity> _parentEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ConditionFulfilled, ForwardCondition>();

            _parentEntity = World.GetStash<ParentEntity>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
            _forwardConditionsToMeet = World.GetStash<ForwardConditionsToMeet>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var parent = ref _parentEntity.Get(entity);
                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                ref var conditionsToMeet = ref _forwardConditionsToMeet.Get(parent.Entity);
                
                conditionsToMeet.Remaining -= conditionFulfilled.Value;
            }
        }
    }
}