using System.Collections.Generic;
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
    public sealed class Action_CreateAbilityProviderSystem : ISystem
    {
        private Stash<Active> _active;

        // Stashes for component access
        private Stash<CreateAbilityProvider> _createAbilityProvider;
        private Filter _filter;
        private Stash<MarkToDestroyWhenLevelChanged> _markToDestroyWhenLevelChanged;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<RotationFromConfig> _rotationFromConfig;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CreateAbilityProvider, Active>();

            // Initialize stashes
            _createAbilityProvider = World.GetStash<CreateAbilityProvider>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _rotationFromConfig = World.GetStash<RotationFromConfig>();
            _userEntity = World.GetStash<UserEntity>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _active = World.GetStash<Active>();
            _markToDestroyWhenLevelChanged = World.GetStash<MarkToDestroyWhenLevelChanged>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var action = ref _createAbilityProvider.Get(entity);
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);
                ref var rotationFromConfig = ref _rotationFromConfig.Get(entity);

                AbilityProvider instance;

                if (action.UsePooling)
                    instance = GameObjectPool.Get(action.Prefab.gameObject, positionFromConfig.Position,
                        rotationFromConfig.Rotation).GetComponent<AbilityProvider>();
                else
                    instance = Object.Instantiate(action.Prefab, positionFromConfig.Position,
                        rotationFromConfig.Rotation);

                ref var user = ref _userEntity.Get(entity);

                if (instance.Created) {
                    instance.ResetState();
                }
                else {
                    var abilityEntity = instance.Create(user.Entity, World);
                    instance.Activate();
                    
                    if (!action.DontDestroyWhenWorldChanges)
                        _markToDestroyWhenLevelChanged.Add(abilityEntity);
                }

                if (action.PassUserTargets) {
                    ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                    ref var targets = ref _targets.Get(targetsProvider.Entity);
                    ref var instanceTargets = ref _targets.Get(instance.Entity);

                    if (!action.PassTargetsOnlyOnce) {
                        instanceTargets.List = targets.List;
                    }
                    else {
                        instanceTargets.List = new List<Entity>();
                        foreach (var target in targets.List)
                            instanceTargets.List.Add(target);
                    }
                }

                _active.Remove(entity);
            }
        }
    }
}