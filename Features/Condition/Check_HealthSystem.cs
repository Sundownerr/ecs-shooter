using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_HealthSystem : ISystem
    {
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Filter _filter;
        private Stash<Health> _health;
        private Stash<HealthCondition> _healthCondition;
        private Stash<MaxHealth> _maxHealth;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<HealthCondition, Active>();
            _health = World.GetStash<Health>();
            _maxHealth = World.GetStash<MaxHealth>();
            _healthCondition = World.GetStash<HealthCondition>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var healthCondition = ref _healthCondition.Get(entity);
                ref var user = ref _userEntity.Get(entity);
                ref var targetProvider = ref _targetsProviderEntity.Get(entity);

                Entity targetEntity;

                switch (healthCondition.TargetType) {
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

                ref var health = ref _health.Get(targetEntity);
                var compareValue = healthCondition.Value;

                if (healthCondition.UsePercent && _maxHealth.Has(targetEntity)) {
                    ref var maxHealth = ref _maxHealth.Get(targetEntity);
                    if (maxHealth.Value > 0)
                        compareValue = maxHealth.Value * (healthCondition.Value / 100f);
                }

                var conditionMet = false;

                switch (healthCondition.ComparisonType) {
                    case HealthComparisonType.Equal:
                        conditionMet = Mathf.Approximately(health.Value, compareValue);
                        break;
                    case HealthComparisonType.LessThan:
                        conditionMet = health.Value <= compareValue;
                        break;
                    case HealthComparisonType.GreaterThan:
                        conditionMet = health.Value >= compareValue;
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