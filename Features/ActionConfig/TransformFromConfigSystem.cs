using System;
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
    public sealed class TransformFromConfigSystem : ISystem
    {
        private Filter _filter;
        private Stash<TransformFromConfig> _transformFromConfig;
        private Stash<Active> _active;
        private Stash<UserEntity> _userEntity;
        private Stash<Reference<Transform>> _referenceTransform;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<Targets> _targets;
        private Stash<CustomTargetingTransform> _customTargetingTransform;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<TransformFromConfig, Active>();

            _transformFromConfig = World.GetStash<TransformFromConfig>();
            _active = World.GetStash<Active>();
            _userEntity = World.GetStash<UserEntity>();
            _referenceTransform = World.GetStash<Reference<Transform>>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _customTargetingTransform = World.GetStash<CustomTargetingTransform>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var fromConfig = ref _transformFromConfig.Get(entity);

                switch (fromConfig.Config.TargetTransform)
                {
                    case TargetTransform.UserTransform:
                        ref var user = ref _userEntity.Get(entity);
                        fromConfig.Transform = _referenceTransform.Get(user.Entity).Value;
                        break;

                    case TargetTransform.TargetTransform:
                        ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                        ref var targets = ref _targets.Get(targetsProvider.Entity);
                        fromConfig.Transform = _referenceTransform.Get(targets.List[0]).Value;

                        if (_customTargetingTransform.Has(targets.List[0]))
                            fromConfig.Transform = _customTargetingTransform.Get(targets.List[0]).Value;
                        break;

                    case TargetTransform.OtherTransform:
                        fromConfig.Transform = fromConfig.Config.CustomTargetTransform;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
