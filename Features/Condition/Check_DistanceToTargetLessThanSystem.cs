using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_DistanceToTargetLessThanSystem : ISystem
    {
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Stash<DistanceToTargetLessThan> _distanceToTargetLessThan;
        private Filter _filter;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<DistanceToTargetLessThan, Active>();

            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _userEntity = World.GetStash<UserEntity>();
            _worldPosition = World.GetStash<WorldPosition>();
            _distanceToTargetLessThan = World.GetStash<DistanceToTargetLessThan>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var targetsProvider = ref _targetsProviderEntity.Get(entity);

                // FIXIT: need to get rid of this check
                if (World.IsDisposed(targetsProvider.Entity))
                    continue;

                ref var targets = ref _targets.Get(targetsProvider.Entity);

                if (targets.List.Count == 0 || World.IsDisposed(targets.List[0]))
                    continue;

                ref var user = ref _userEntity.Get(entity);
                ref var userPosition = ref _worldPosition.Get(user.Entity);

                foreach (var targetEntity in targets.List) {
                    if (World.IsDisposed(targetEntity))
                        continue;

                    ref var targetPosition = ref _worldPosition.Get(targetEntity);

                    var distanceToTarget = Vector3.Distance(userPosition.Value, targetPosition.Value);
                    ref var condition = ref _distanceToTargetLessThan.Get(entity);

                    if (distanceToTarget < condition.Value) {
                        ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                        conditionFulfilled.Value++;
                        break;
                    }
                }
            }
        }
    }
}