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
    public sealed class Check_AbilityActivatedSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<ParentEntity> _parent;
        private Stash<AbilityActivatedFromScript> _activatedFromScript;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityActivated, Active>();

            // Initialize stashes
            _parent = World.GetStash<ParentEntity>();
            _activatedFromScript = World.GetStash<AbilityActivatedFromScript>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                //parent usually is ability provider entity
                ref var parent = ref _parent.Get(entity);

                ref var activatedFromScript = ref _activatedFromScript.Get(parent.Entity);

                if (activatedFromScript.Value)
                {
                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                }
            }
        }
    }
}
