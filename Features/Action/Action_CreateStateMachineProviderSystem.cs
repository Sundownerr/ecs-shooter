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
    public sealed class Action_CreateStateMachineProviderSystem : ISystem
    {
        private Stash<Active> _active;

        // Stashes for component access
        private Stash<CreateStateMachineProvider> _createStateMachineProvider;
        private Filter _filter;
        private Stash<MarkToDestroyWhenLevelChanged> _markToDestroyWhenLevelChanged;
        private Stash<ParentEntity> _parentEntity;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<RotationFromConfig> _rotationFromConfig;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;
        private Stash<Reference<Transform>> _transformReference;
        private Stash<WorldPosition> _worldPosition;
        private Stash<EntityCreatedDuringGameplay> _entityCreatedDuringGameplay;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CreateStateMachineProvider, Active>();

            _createStateMachineProvider = World.GetStash<CreateStateMachineProvider>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _rotationFromConfig = World.GetStash<RotationFromConfig>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _transformReference = World.GetStash<Reference<Transform>>();
            _worldPosition = World.GetStash<WorldPosition>();
            _active = World.GetStash<Active>();
            _parentEntity = World.GetStash<ParentEntity>();
            _markToDestroyWhenLevelChanged = World.GetStash<MarkToDestroyWhenLevelChanged>();
            _entityCreatedDuringGameplay = World.GetStash<EntityCreatedDuringGameplay>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var action = ref _createStateMachineProvider.Get(entity);
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);
                ref var rotationFromConfig = ref _rotationFromConfig.Get(entity);

                StateMachineProvider instance;

                if (action.UsePooling)
                    instance = GameObjectPool.Get(action.Prefab.gameObject, positionFromConfig.Position,
                        rotationFromConfig.Rotation).GetComponent<StateMachineProvider>();
                else
                    instance = Object.Instantiate(action.Prefab, positionFromConfig.Position,
                        rotationFromConfig.Rotation);

                if (instance.Created) {
                    instance.ChangeToInitialState();
                }
                else {
                    var stateMachineUser = World.CreateEntity();
                    _transformReference.Set(stateMachineUser, new Reference<Transform> {Value = instance.transform,});
                    _worldPosition.Set(stateMachineUser, new WorldPosition {Value = instance.transform.position,});
                    _targets.Set(stateMachineUser, new Targets {List = new List<Entity>(),});

                    var stateMachineInstance = instance.Create(stateMachineUser, World);
                    
                    if (!action.DontDestroyWhenWorldChanges) {
                        _markToDestroyWhenLevelChanged.Add(stateMachineUser);
                        _markToDestroyWhenLevelChanged.Add(stateMachineInstance);
                        _entityCreatedDuringGameplay.Add(stateMachineUser);
                    }
                }

                if (action.PassUserTargets) {
                    ref var stateMachineUser = ref _parentEntity.Get(instance.Entity);

                    ref var targetsProvider = ref _targetsProviderEntity.Get(entity);
                    ref var targets = ref _targets.Get(targetsProvider.Entity);
                    ref var instanceTargets = ref _targets.Get(stateMachineUser.Entity);

                    if (!action.PassTargetsOnlyOnce) {
                        instanceTargets.List = targets.List;
                    }
                    else {
                        instanceTargets.List ??= new List<Entity>();

                        foreach (var target in targets.List)
                            instanceTargets.List.Add(target);
                    }
                }

                _active.Remove(entity);
            }
        }
    }
}