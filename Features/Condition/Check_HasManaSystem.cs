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
    public sealed class Check_HasManaSystem : ISystem
    {
        private Filter _filter;
        private Stash<FloatStats> _floatStats;
        private Stash<ManaCondition> _manaCondition;
        private Stash<UserEntity> _userEntity;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<Targets> _targets;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ManaCondition, Active>();
            _floatStats = World.GetStash<FloatStats>();
            _manaCondition = World.GetStash<ManaCondition>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var manaCondition = ref _manaCondition.Get(entity);
                ref var user = ref _userEntity.Get(entity);
                ref var targetProvider = ref _targetsProviderEntity.Get(entity);

                // Determine the target entity based on TargetType
                Entity targetEntity;
                switch (manaCondition.TargetType)
                {
                    case TargetType.Self:
                        targetEntity = user.Entity;
                        break;
                    case TargetType.Target:
                        // If there are no targets, condition is not fulfilled
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

                // Check if the target entity has FloatStats component
                if (!_floatStats.Has(targetEntity))
                    continue;

                // Get the FloatStats component and check mana based on comparison type
                ref var stats = ref _floatStats.Get(targetEntity);
                float currentMana = stats.Value.ValueOf(Stat.Mana);
                bool conditionMet = false;

                switch (manaCondition.ComparisonType)
                {
                    case ManaComparisonType.LessThan:
                        conditionMet = currentMana < manaCondition.Value;
                        break;
                    case ManaComparisonType.MoreThan:
                        conditionMet = currentMana > manaCondition.Value;
                        break;
                }

                if (conditionMet)
                {
                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                }
            }
        }
    }
}
