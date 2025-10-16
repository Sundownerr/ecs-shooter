using System;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.Providers;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateAllInAoeTargetsSystem : ISystem
    {
        private readonly Collider[] _results = new Collider[2000];
        private Stash<AbilityCustomData> _abilityCustomDataStash;
        private Stash<AllInAOE> _allInAOEStash;
        private Filter _filter;
        private Stash<ParentEntity> _parentEntityStash;
        private Stash<TargetsProviderEntity> _targetsProviderEntityStash;
        private Stash<Targets> _targetsStash;
        private Stash<TargetWorldPosition> _targetWorldPositionStash;
        private Stash<Reference<Transform>> _transformStash;
        private Stash<UserEntity> _userEntityStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AllInAOE, Active>();

            _userEntityStash = World.GetStash<UserEntity>();
            _allInAOEStash = World.GetStash<AllInAOE>();
            _transformStash = World.GetStash<Reference<Transform>>();
            _targetWorldPositionStash = World.GetStash<TargetWorldPosition>();
            _parentEntityStash = World.GetStash<ParentEntity>();
            _abilityCustomDataStash = World.GetStash<AbilityCustomData>();
            _targetsProviderEntityStash = World.GetStash<TargetsProviderEntity>();
            _targetsStash = World.GetStash<Targets>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var allInAOE = ref _allInAOEStash.Get(entity);
                var config = allInAOE.Config;

                Vector3 position;

                switch (config.Center) {
                    case Position.UserTransform: {
                        ref var user = ref _userEntityStash.Get(entity);
                        ref Reference<Transform> userTransform = ref _transformStash.Get(user.Entity);
                        position = userTransform.Value.position;
                        // Debug.Log($"UPDATE {userTransform.Value.name} {position}");
                    }
                        break;

                    case Position.TargetTransform: {
                        ref var user = ref _userEntityStash.Get(entity);
                        ref var targetWorldPosition = ref _targetWorldPositionStash.Get(user.Entity);
                        position = targetWorldPosition.Value;
                    }
                        break;

                    case Position.CustomTransform:
                        position = config.CustomCenter.position;
                        // Debug.Log($"UPDATE {config.CustomCenter.name}");
                        break;

                    case Position.CustomPosition:
                        ref var parent = ref _parentEntityStash.Get(entity);
                        ref var customData = ref _abilityCustomDataStash.Get(parent.Entity);
                        position = customData.CustomPosition;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var hits = Physics.OverlapSphereNonAlloc(position, config.Range, _results, config.LayerMask);

                ref var targetsProvider = ref _targetsProviderEntityStash.Get(entity);
                ref var targets = ref _targetsStash.Get(targetsProvider.Entity);

                targets.List.Clear();

                for(var i = hits - 1; i >= 0; i--) {
                    if (_results[i].TryGetComponent<EntityProvider>(out var provider))
                        targets.List.Add(provider.Entity);
                }
            }
        }
    }
}