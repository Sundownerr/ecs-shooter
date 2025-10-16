using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_AbilityHaveTargetSystem : ISystem
    {
        private Stash<AbilityHaveTarget> _abilityHaveTarget;
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Filter _filter;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityHaveTarget, Active>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
            _abilityHaveTarget = World.GetStash<AbilityHaveTarget>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var abilityHaveTarget = ref _abilityHaveTarget.Get(entity);
                ref var targetsProvider = ref _targetsProviderEntity.Get(entity);

                // FIXIT: need to get rid of this check
                if (World.IsDisposed(targetsProvider.Entity))
                    continue;

                ref var targets = ref _targets.Get(targetsProvider.Entity);

                var conditionMet = abilityHaveTarget.CheckType switch {
                                       TargetCheckType.None => targets.List.Count == 0,
                                       TargetCheckType.AtLeastOne => targets.List.Count >= 1,
                                       TargetCheckType.AtLeastAmount => targets.List.Count >= abilityHaveTarget.Amount,
                                       _ => false,
                                   };

                if (!conditionMet)
                    continue;
                
                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                conditionFulfilled.Value++;
            }
        }
    }
}