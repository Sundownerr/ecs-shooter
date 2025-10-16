using System;
using System.Collections.Generic;
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
    public sealed class Action_RigidbodyActionAddForceSystem : ISystem
    {
        private readonly List<Rigidbody> _targets = new();
        private Stash<Active> _active;
        private Filter _filter;
        private Stash<RigidbodyActionAddForce> _rigidbodyActionAddForce;

        // Stashes for component access
        private Stash<UserEntity> _userEntity;
        private Stash<WorldPosition> _worldPosition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<RigidbodyActionAddForce, Active>();

            // Initialize stashes
            _userEntity = World.GetStash<UserEntity>();
            _rigidbodyActionAddForce = World.GetStash<RigidbodyActionAddForce>();
            _worldPosition = World.GetStash<WorldPosition>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var user = ref _userEntity.Get(entity);
                ref var rigidbodyAction = ref _rigidbodyActionAddForce.Get(entity);

                rigidbodyAction.Config.AddRigidbodyTargets(entity, _targets);

                foreach (var target in _targets) {
                    Vector3 direction;

                    switch (rigidbodyAction.Config.Direction) {
                        case Direction.TargetAwayFromUser: {
                            ref var userPosition = ref _worldPosition.Get(user.Entity);
                            direction = (float3) target.position - userPosition.Value;
                        }
                            break;

                        case Direction.TargetTowardsUser: {
                            ref var userPosition = ref _worldPosition.Get(user.Entity);
                            direction = userPosition.Value - (float3) target.position;
                        }
                            break;

                        case Direction.CustomWorldDirection:
                        case Direction.CustomLocalDirection:
                            direction = rigidbodyAction.Config.CustomDirection;
                            break;

                        case Direction.UserAwayFromTarget: {
                            ref var userPosition = ref _worldPosition.Get(user.Entity);
                            direction = userPosition.Value - (float3) target.position;
                        }
                            break;
                        case Direction.UserTowardsTarget: {
                            ref var userPosition = ref _worldPosition.Get(user.Entity);
                            direction = (float3) target.position - userPosition.Value;
                            break;
                        }
                        case Direction.TransformForward:
                            Debug.LogError("Missing implementation for TransformForward");
                            direction = Vector3.zero;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    direction.Normalize();
                    direction *= rigidbodyAction.Config.Force;

                    var worldSpace = rigidbodyAction.Config.Direction.WorldSpace();

                    if (worldSpace)
                        target.AddForce(direction, rigidbodyAction.Config.ForceMode);
                    else
                        target.AddRelativeForce(direction, rigidbodyAction.Config.ForceMode);
                }

                _active.Remove(entity);
            }
        }
    }
}