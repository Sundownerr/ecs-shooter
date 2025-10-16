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
    public sealed class Check_HasBeenDamagedSystem : ISystem
    {
        private Stash<Active> _active;
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Stash<DamageApplied> _damageApplied;
        private Filter _filter;
        private Stash<HasBeenDamaged> _hasBeenDamaged;
        private Stash<HasBeenDamagedCondition> _hasBeenDamagedCondition;
        private Stash<MaxHealth> _maxHealth;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<HasBeenDamagedCondition, Active>();

            // Initialize stashes
            _hasBeenDamagedCondition = World.GetStash<HasBeenDamagedCondition>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
            _hasBeenDamaged = World.GetStash<HasBeenDamaged>();
            _damageApplied = World.GetStash<DamageApplied>();
            _maxHealth = World.GetStash<MaxHealth>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var condition = ref _hasBeenDamagedCondition.Get(entity);
                ref var user = ref _userEntity.Get(entity);
                ref var targetProvider = ref _targetsProviderEntity.Get(entity);

                // Determine target entity based on TargetType
                Entity targetEntity;

                switch (condition.TargetType) {
                    case TargetType.Self:
                        targetEntity = user.Entity;
                        break;
                    case TargetType.Target:
                        if (!_targets.Has(targetProvider.Entity))
                            continue;

                        ref var targets = ref _targets.Get(targetProvider.Entity);
                        if (targets.List.Count == 0)
                            continue;

                        targetEntity = targets.List[0];
                        break;
                    default:
                        continue;
                }

                // Check if the target has been damaged
                var conditionMet = false;

                if (_hasBeenDamaged.Has(targetEntity))
                    switch (condition.CheckType) {
                        case DamageCheckType.Any:
                            conditionMet = true;
                            break;

                        case DamageCheckType.ByAmount:
                            if (_damageApplied.Has(targetEntity)) {
                                ref var damageApplied = ref _damageApplied.Get(targetEntity);
                                conditionMet = damageApplied.Total >= condition.Value;
                            }

                            break;

                        case DamageCheckType.ByPercent:
                            if (_damageApplied.Has(targetEntity) && _maxHealth.Has(targetEntity)) {
                                ref var damageApplied = ref _damageApplied.Get(targetEntity);
                                ref var maxHealth = ref _maxHealth.Get(targetEntity);

                                var damagePercent = damageApplied.Total * 100f / maxHealth.Value;
                                conditionMet = damagePercent >= condition.Value;
                            }

                            break;
                    }

                if (conditionMet) {
                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                }
            }
        }
    }
}