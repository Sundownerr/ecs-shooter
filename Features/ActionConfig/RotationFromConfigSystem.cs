using System;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RotationFromConfigSystem : ISystem
    {
        private Stash<AbilityCustomData> _abilityCustomData;

        private Stash<CustomTargetingTransform> _customTargetingTransform;
        private Filter _filter;
        private Stash<ParentEntity> _parentEntity;
        private Stash<Reference<Transform>> _referenceTransform;
        private Stash<RotationFromConfig> _rotationFromConfig;
        private Stash<TargetEntity> _targetEntity;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<RotationFromConfig, Active>();

            _rotationFromConfig = World.GetStash<RotationFromConfig>();
            _userEntity = World.GetStash<UserEntity>();
            _referenceTransform = World.GetStash<Reference<Transform>>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _worldPosition = World.GetStash<WorldPosition>();
            _parentEntity = World.GetStash<ParentEntity>();
            _abilityCustomData = World.GetStash<AbilityCustomData>();
            _customTargetingTransform = World.GetStash<CustomTargetingTransform>();
            _targetEntity = World.GetStash<TargetEntity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var user = ref _userEntity.Get(entity);
                ref var rotationFromConfig = ref _rotationFromConfig.Get(entity);

                if (rotationFromConfig.Config.Value is Rotation.TargetTransform or Rotation.TowardsTarget) {
                    ref var targetEntity = ref _targetEntity.Get(entity);

                    if (!targetEntity.EntitySet) {
                        ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                        ref var targets = ref _targets.Get(targetsProvider.Entity);

                        foreach (var targetListEntity in targets.List) {
                            if (targetListEntity.IsNullOrDisposed())
                                continue;

                            targetEntity.Entity = targetListEntity;
                            targetEntity.EntitySet = true;
                            break;
                        }
                    }
                }

                switch (rotationFromConfig.Config.Value) {
                    case Rotation.UserTransform: {
                        ref Reference<Transform> userTransform = ref _referenceTransform.Get(user.Entity);
                        rotationFromConfig.Rotation = userTransform.Value.rotation;
                    }
                        break;

                    case Rotation.TargetTransform: {
                        ref var targetEntity = ref _targetEntity.Get(entity);

                        if (targetEntity.Entity.IsNullOrDisposed())
                            break;

                        if (_customTargetingTransform.Has(targetEntity.Entity)) {
                            ref var customTargetingTransform = ref _customTargetingTransform.Get(targetEntity.Entity);
                            rotationFromConfig.Rotation = customTargetingTransform.Value.rotation;
                        }
                        else {
                            ref Reference<Transform> targetTransform = ref _referenceTransform.Get(targetEntity.Entity);
                            rotationFromConfig.Rotation = targetTransform.Value.rotation;
                        }
                    }
                        break;

                    case Rotation.CustomTransform:
                        rotationFromConfig.Rotation = rotationFromConfig.Config.CustomTransform.rotation;
                        break;

                    case Rotation.CustomRotation:
                        ref var parent = ref _parentEntity.Get(entity);
                        ref var customData = ref _abilityCustomData.Get(parent.Entity);
                        rotationFromConfig.Rotation = customData.CustomRotation == Quaternion.identity
                            ? Quaternion.Euler(customData.CustomDirection)
                            : customData.CustomRotation;
                        break;

                    case Rotation.TowardsTarget: {
                        {
                            ref var targetEntity = ref _targetEntity.Get(entity);

                            if (targetEntity.Entity.IsNullOrDisposed())
                                break;

                            ref Reference<Transform> userTransform = ref _referenceTransform.Get(user.Entity);

                            if (_customTargetingTransform.Has(targetEntity.Entity)) {
                                ref var customTargetingTransform =
                                    ref _customTargetingTransform.Get(targetEntity.Entity);
                                rotationFromConfig.Rotation = Quaternion.LookRotation(
                                    customTargetingTransform.Value.position -
                                    userTransform.Value.position);
                            }
                            else {
                                ref var targetPosition = ref _worldPosition.Get(targetEntity.Entity);
                                rotationFromConfig.Rotation = Quaternion.LookRotation(targetPosition.Value -
                                    (float3) userTransform.Value.position);
                            }
                        }
                    }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}