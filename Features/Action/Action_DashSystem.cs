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
    public sealed class Action_DashSystem : ISystem
    {
        private Stash<AbilityDash> _abilityDash;
        private Stash<Active> _active;

        private Filter _filter;
        private Stash<MoveDirection> _moveDirection;
        private Stash<Reference<Rigidbody>> _rigidbodyReference;
        private Stash<Targets> _targets;
        private Stash<TargetsProviderEntity> _targetsProviderEntity;

        // Stashes for component access
        private Stash<UserEntity> _userEntity;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityDash, Active>();

            // Initialize stashes
            _userEntity = World.GetStash<UserEntity>();
            _abilityDash = World.GetStash<AbilityDash>();
            _targetsProviderEntity = World.GetStash<TargetsProviderEntity>();
            _targets = World.GetStash<Targets>();
            _moveDirection = World.GetStash<MoveDirection>();
            _rigidbodyReference = World.GetStash<Reference<Rigidbody>>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var user = ref _userEntity.Get(entity);
                ref var dash = ref _abilityDash.Get(entity);

                Entity targetEntity = default;

                switch (dash.TargetType) {
                    case TargetType.Self:
                        targetEntity = user.Entity;
                        break;
                    case TargetType.Target:
                    case TargetType.Other:
                        ref var targetsProvider = ref _targetsProviderEntity.Get(entity);

                        if (_targets.Has(targetsProvider.Entity)) {
                            ref var targets = ref _targets.Get(targetsProvider.Entity);
                            if (targets.List is {Count: > 0,})
                                targetEntity = targets.List[0];
                        }

                        break;
                }

                if (!World.IsDisposed(targetEntity))
                    if (_moveDirection.Has(targetEntity) && _rigidbodyReference.Has(targetEntity)) {
                        ref var moveDirection = ref _moveDirection.Get(targetEntity);
                        ref Reference<Rigidbody> rigidbody = ref _rigidbodyReference.Get(targetEntity);

                        var direction = moveDirection.Value.normalized;
                        rigidbody.Value.AddForce(direction * dash.Force, dash.ForceMode);
                    }

                _active.Remove(entity);
            }
        }
    }
}