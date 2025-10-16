using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class StartCooldown_To_UsingSystem : ISystem
    {
        private Stash<Active> _active;
        private Stash<CancelConditions> _cancelConditions;
        private Filter _filter;
        private Stash<InstantActions> _instantActions;

        // Stashes for component access
        private Stash<PartsToComplete> _partsToComplete;
        private Stash<AbilityState_StartCooldown> _startCooldown;
        private Stash<UsageProgress> _usageProgress;
        private Stash<AbilityState_Using> _using;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<AbilityState_StartCooldown>().Without<Cancelled>().Build();

            // Initialize stashes
            _partsToComplete = World.GetStash<PartsToComplete>();
            _instantActions = World.GetStash<InstantActions>();
            _usageProgress = World.GetStash<UsageProgress>();
            _cancelConditions = World.GetStash<CancelConditions>();
            _startCooldown = World.GetStash<AbilityState_StartCooldown>();
            _using = World.GetStash<AbilityState_Using>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var partsToComplete = ref _partsToComplete.Get(entity);

                if (partsToComplete.Value > 0)
                    continue;

                _startCooldown.Remove(entity);
                _using.Add(entity);

                if (_instantActions.Has(entity)) {
                    ref var parts = ref _instantActions.Get(entity);

                    foreach (var instantUseAction in parts.List)
                        _active.Add(instantUseAction);
                }
                else {
                    partsToComplete.Value = 1;

                    ref var usageProgress = ref _usageProgress.Get(entity);
                    _active.Add(usageProgress.Entity);

                    if (_cancelConditions.Has(entity)) {
                        ref var cancelConditions = ref _cancelConditions.Get(entity);

                        foreach (var condition in cancelConditions.List)
                            _active.Add(condition);
                    }
                }

                // Debug.Log("Start Cooldown -> Using");
            }
        }
    }
}