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
    public sealed class DeactivateCompletedForwardConditionsSystem : ISystem
    {
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<ForwardConditions> _forwardConditions;
        private Stash<ForwardConditionsToMeet> _forwardConditionsToMeet;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ForwardConditionsToMeet, ForwardConditions>();

            _forwardConditionsToMeet = World.GetStash<ForwardConditionsToMeet>();
            _forwardConditions = World.GetStash<ForwardConditions>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var conditionsToMeet = ref _forwardConditionsToMeet.Get(entity);

                if (conditionsToMeet.Remaining > 0)
                    continue;

                ref var conditions = ref _forwardConditions.Get(entity);
                foreach (var condition in conditions.List)
                    _active.Remove(condition);
            }
        }
    }
}