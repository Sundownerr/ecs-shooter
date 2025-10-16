using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using EcsMagic.Actions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_CheckRaycastHitSystem : ISystem
    {
        private Filter _filter;
        private Stash<CheckRaycastHitCondition> _checkRaycastHitCondition;
        private Stash<UserEntity> _userEntity;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<Reference<Transform>> _transformReference;
        private Stash<Targets> _targets;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckRaycastHitCondition, Active>();
            _checkRaycastHitCondition = World.GetStash<CheckRaycastHitCondition>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _transformReference = World.GetStash<Reference<Transform>>();
            _targets = World.GetStash<Targets>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var checkRaycast = ref _checkRaycastHitCondition.Get(entity);
                ref var user = ref _userEntity.Get(entity);
                ref var targetProvider = ref _targetsProviderEntity.Get(entity);

                Vector3 direction = GetDirectionFromConfig(checkRaycast.Direction,
                                                          user.Entity,
                                                          targetProvider.Entity);

                if (!Physics.Raycast(checkRaycast.OriginTransform.position,
                                    direction,
                                    checkRaycast.MaxDistance,
                                    checkRaycast.LayerMask))
                    continue;

                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                conditionFulfilled.Value++;
            }
        }

        private Vector3 GetDirectionFromConfig(DirectionConfig directionConfig, Entity userEntity, Entity targetProviderEntity)
        {
            switch (directionConfig.Value)
            {
                case Direction.TargetAwayFromUser:
                    {
                        ref var userTransformRef = ref _transformReference.Get(userEntity);
                        ref var targets = ref _targets.Get(targetProviderEntity);
                        var targetEntity = targets.List[0];
                        ref var targetTransformRef = ref _transformReference.Get(targetEntity);

                        var userTransform = userTransformRef.Value;
                        var targetTransform = targetTransformRef.Value;
                        return (targetTransform.position - userTransform.position).normalized;
                    }

                case Direction.TargetTowardsUser:
                    {
                        ref var userTransformRef = ref _transformReference.Get(userEntity);
                        ref var targets = ref _targets.Get(targetProviderEntity);
                        var targetEntity = targets.List[0];
                        ref var targetTransformRef = ref _transformReference.Get(targetEntity);

                        var userTransform = userTransformRef.Value;
                        var targetTransform = targetTransformRef.Value;
                        return (userTransform.position - targetTransform.position).normalized;
                    }

                case Direction.UserAwayFromTarget:
                    {
                        ref var userTransformRef = ref _transformReference.Get(userEntity);
                        ref var targets = ref _targets.Get(targetProviderEntity);
                        var targetEntity = targets.List[0];
                        ref var targetTransformRef = ref _transformReference.Get(targetEntity);

                        var userTransform = userTransformRef.Value;
                        var targetTransform = targetTransformRef.Value;
                        return (userTransform.position - targetTransform.position).normalized;
                    }

                case Direction.UserTowardsTarget:
                    {
                        ref var userTransformRef = ref _transformReference.Get(userEntity);
                        ref var targets = ref _targets.Get(targetProviderEntity);
                        var targetEntity = targets.List[0];
                        ref var targetTransformRef = ref _transformReference.Get(targetEntity);

                        var userTransform = userTransformRef.Value;
                        var targetTransform = targetTransformRef.Value;
                        return (targetTransform.position - userTransform.position).normalized;
                    }

                case Direction.CustomWorldDirection:
                    return directionConfig.CustomDirection;

                case Direction.CustomLocalDirection:
                    {
                        ref var userTransformRef = ref _transformReference.Get(userEntity);
                        var userTransform = userTransformRef.Value;
                        return userTransform.TransformDirection(directionConfig.CustomDirection);
                    }

                default:
                    return Vector3.forward;
            }
        }
    }
}
