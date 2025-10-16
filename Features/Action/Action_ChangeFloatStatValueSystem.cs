using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_ChangeFloatStatValueSystem : ISystem
    {
        // Stashes for component access
        private Stash<AbilityChangeFloatStatValue> _abilityChangeFloatStatValue;
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<FloatStats> _floatStats;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityChangeFloatStatValue, Active>();

            // Initialize stashes
            _abilityChangeFloatStatValue = World.GetStash<AbilityChangeFloatStatValue>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _floatStats = World.GetStash<FloatStats>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var changeFloatStat = ref _abilityChangeFloatStatValue.Get(entity);
                ref var userEntity = ref _userEntity.Get(entity);
                ref var targetsProvider = ref _targetsProviderEntity.Get(entity);

                Entity targetEntity = default;

                // Determine target based on TargetType
                switch (changeFloatStat.TargetType) {
                    case TargetType.Self:
                        targetEntity = userEntity.Entity;
                        break;
                    case TargetType.Target:
                    case TargetType.Other:
                        if (_targets.Has(targetsProvider.Entity)) {
                            ref var targets = ref _targets.Get(targetsProvider.Entity);
                            if (targets.List is {Count: > 0,})
                                targetEntity = targets.List[0]; // Use the first target
                        }

                        break;
                }

                // Modify the FloatStats component
                if (!World.IsDisposed(targetEntity) && _floatStats.Has(targetEntity)) {
                    ref var floatStats = ref _floatStats.Get(targetEntity);
                    floatStats.Value.Set(changeFloatStat.StatId, changeFloatStat.Value);
                }

                _active.Remove(entity);
            }
        }
    }
}