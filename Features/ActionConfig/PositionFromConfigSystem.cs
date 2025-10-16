using System;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.AI;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PositionFromConfigSystem : ISystem
    {
        private Stash<AbilityCustomData> _abilityCustomData;
        private Stash<CustomTargetingTransform> _customTargetingTransform;
        private Filter _filter;
        private Stash<ParentEntity> _parentEntity;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<TargetEntity> _targetEntity;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<PositionFromConfig, Active>();

            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _userEntity = World.GetStash<UserEntity>();
            _worldPosition = World.GetStash<WorldPosition>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _customTargetingTransform = World.GetStash<CustomTargetingTransform>();
            _parentEntity = World.GetStash<ParentEntity>();
            _abilityCustomData = World.GetStash<AbilityCustomData>();
            _targetEntity = World.GetStash<TargetEntity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);

                switch (positionFromConfig.Config.Value) {
                    case Position.UserTransform: {
                        ref var user = ref _userEntity.Get(entity);
                        ref var userPosition = ref _worldPosition.Get(user.Entity);
                        positionFromConfig.Position = userPosition.Value;
                    }
                        break;

                    case Position.TargetTransform:
                        ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                        ref var targets = ref _targets.Get(targetsProvider.Entity);
                        ref var targetEntity = ref _targetEntity.Get(entity);

                        if (!targetEntity.EntitySet)
                            foreach (var targetListEntity in targets.List) {
                                if (World.IsDisposed(targetListEntity))
                                    continue;

                                targetEntity.Entity = targetListEntity;
                                targetEntity.EntitySet = true;
                                break;
                            }

                        if (!World.IsDisposed(targetEntity.Entity)) {
                            if (_customTargetingTransform.Has(targetEntity.Entity)) {
                                ref var customTargetingTransform =
                                    ref _customTargetingTransform.Get(targetEntity.Entity);
                                positionFromConfig.Position = customTargetingTransform.Value.position;
                            }
                            else {
                                ref var worldPosition = ref _worldPosition.Get(targetEntity.Entity);
                                positionFromConfig.Position = worldPosition.Value;
                            }
                        }

                        break;

                    case Position.CustomTransform:
                        positionFromConfig.Position = positionFromConfig.Config.CustomTransform.position;
                        break;

                    case Position.CustomPosition:
                        ref var parent = ref _parentEntity.Get(entity);
                        ref var customData = ref _abilityCustomData.Get(parent.Entity);
                        positionFromConfig.Position = customData.CustomPosition;
                        break;

                    case Position.NearestNavMeshPoint: {
                        ref var user = ref _userEntity.Get(entity);
                        ref var userPosition = ref _worldPosition.Get(user.Entity);
                        var positionFound = false;
                        var acc = 0;

                        while (!positionFound && acc < 10) {
                            positionFound =
                                NavMesh.SamplePosition(userPosition.Value, out var hit, 15, NavMesh.AllAreas);
                            positionFromConfig.Position = hit.position;
                            acc++;
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