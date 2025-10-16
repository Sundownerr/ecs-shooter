using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Using_To_InitialSystem : ISystem
    {
        private Stash<AbilityState_Initial> _abilityStateInitial;
        private Stash<AbilityState_Using> _abilityStateUsing;
        private Stash<Active> _active;
        private Stash<CancelConditions> _cancelConditions;
        private Filter _filter;

        private Stash<PartsToComplete> _partsToComplete;
        private Stash<ShouldResetDurations> _shouldResetDurations;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<AbilityState_Using>().Without<Cancelled>().Build();

            _partsToComplete = World.GetStash<PartsToComplete>();
            _abilityStateUsing = World.GetStash<AbilityState_Using>();
            _abilityStateInitial = World.GetStash<AbilityState_Initial>();
            _cancelConditions = World.GetStash<CancelConditions>();
            _active = World.GetStash<Active>();
            _shouldResetDurations = World.GetStash<ShouldResetDurations>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var partsToComplete = ref _partsToComplete.Get(entity);

                if (partsToComplete.Value > 0)
                    continue;

                _abilityStateUsing.Remove(entity);
                _abilityStateInitial.Add(entity);

                _shouldResetDurations.Add(entity);

                if (_cancelConditions.Has(entity)) {
                    ref var cancelConditions = ref _cancelConditions.Get(entity);

                    foreach (var condition in cancelConditions.List)
                        _active.Remove(condition);
                }
                //
                // Debug.Log("Using -> Initial ");
            }
        }
    }
}