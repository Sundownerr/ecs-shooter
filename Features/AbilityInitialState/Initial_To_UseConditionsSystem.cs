using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Initial_To_UseConditionsSystem : ISystem
    {
        private Filter _filter;
        private Stash<ShouldResetDurations> _shouldResetDurations;
        private Stash<AbilityState_Initial> _abilityState_Initial;
        private Stash<AbilityState_CheckUseConditions> _abilityState_CheckUseConditions;
        private Stash<ForwardConditions> _forwardConditions;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityState_Initial>();
            _shouldResetDurations = World.GetStash<ShouldResetDurations>();
            _abilityState_Initial = World.GetStash<AbilityState_Initial>();
            _abilityState_CheckUseConditions = World.GetStash<AbilityState_CheckUseConditions>();
            _forwardConditions = World.GetStash<ForwardConditions>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                _shouldResetDurations.Remove(entity);

                _abilityState_Initial.Remove(entity);
                _abilityState_CheckUseConditions.Add(entity);

                if (!_forwardConditions.Has(entity))
                    continue;

                ref var forwardConditions = ref _forwardConditions.Get(entity);
                foreach (var condition in forwardConditions.List)
                    _active.Add(condition);

                // Debug.Log("Initial -> CheckUseConditions");
            }
        }
    }
}
