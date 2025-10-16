using System;
using EcsMagic.Actions;
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
    public sealed class GameObjectFromConfigSystem : ISystem
    {
        private Filter _filter;
        private Stash<GameObjectFromConfig> _gameObjectFromConfig;
        private Stash<Active> _active;
        private Stash<UserEntity> _userEntity;
        private Stash<Reference<GameObject>> _referenceGameObject;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<Targets> _targets;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<GameObjectFromConfig, Active>();

            _gameObjectFromConfig = World.GetStash<GameObjectFromConfig>();
            _active = World.GetStash<Active>();
            _userEntity = World.GetStash<UserEntity>();
            _referenceGameObject = World.GetStash<Reference<GameObject>>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var fromConfig = ref _gameObjectFromConfig.Get(entity);

                switch (fromConfig.Config.TargetGameObject)
                {
                    case TargetGameObject.UserGameObject:
                        ref var user = ref _userEntity.Get(entity);
                        fromConfig.GameObject = _referenceGameObject.Get(user.Entity).Value;
                        break;

                    case TargetGameObject.TargetGameObject:
                        ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                        ref var targets = ref _targets.Get(targetsProvider.Entity);
                        fromConfig.GameObject = _referenceGameObject.Get(targets.List[0]).Value;
                        break;

                    case TargetGameObject.CustomGameObject:
                        fromConfig.GameObject = fromConfig.Config.CustomTargetGameObject;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
