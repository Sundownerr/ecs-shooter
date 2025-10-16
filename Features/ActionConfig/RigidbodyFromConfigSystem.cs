using System;
using EcsMagic.CommonComponents;
using EcsMagic.PlayerComponenets;
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
    public sealed class RigidbodyFromConfigSystem : ISystem
    {
        private Filter _filter;
        private Stash<RigidbodyFromConfig> _rigidbodyFromConfig;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<RigidbodyFromConfig, Active>();

            _rigidbodyFromConfig = World.GetStash<RigidbodyFromConfig>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var fromConfig = ref _rigidbodyFromConfig.Get(entity);

                switch (fromConfig.Config.TargetRigidbody) {
                    case TargetRigidbody.UserRigidbody: {
                        ref var user = ref _userEntity.Get(entity);
                        ref Reference<Rigidbody> userRigidbody = ref user.Entity.GetComponent<Reference<Rigidbody>>();
                        fromConfig.Rigidbody = userRigidbody.Value;
                    }
                        break;

                    case TargetRigidbody.TargetRigidbody: {
                        ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                        ref var targets = ref _targets.Get(targetsProvider.Entity);
                        ref Reference<Rigidbody> userRigidbody =
                            ref targets.List[0].GetComponent<Reference<Rigidbody>>();

                        fromConfig.Rigidbody = userRigidbody.Value;
                    }
                        break;

                    case TargetRigidbody.OtherRigidbody:
                        fromConfig.Rigidbody = fromConfig.Config.CustomTargetRigidbody;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}