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
    public sealed class Action_MoveTransformSystem : ISystem
    {
        private Stash<Active> _active;
        private Stash<AbilityComponents.Direction> _direction;
        private Stash<DistanceToTarget> _distanceToTarget;
        private Stash<Duration> _duration;
        private Filter _filter;
        private Stash<InitialPosition> _initialPosition;
        private Stash<InitialTargetPosition> _initialTargetPosition;

        // Stashes for component access
        private Stash<MoveTransform> _moveTransform;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<TransformFromConfig> _transformFromConfig;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<MoveTransform, Active>();

            // Initialize stashes
            _moveTransform = World.GetStash<MoveTransform>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _transformFromConfig = World.GetStash<TransformFromConfig>();
            _initialTargetPosition = World.GetStash<InitialTargetPosition>();
            _duration = World.GetStash<Duration>();
            _distanceToTarget = World.GetStash<DistanceToTarget>();
            _initialPosition = World.GetStash<InitialPosition>();
            _direction = World.GetStash<AbilityComponents.Direction>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var moveTransform = ref _moveTransform.Get(entity);
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);
                ref var transformFromConfig = ref _transformFromConfig.Get(entity);

                Vector3 targetPosition;

                var config = moveTransform.Config;

                if (config.FollowTarget) {
                    targetPosition = positionFromConfig.Position;
                }
                else {
                    ref var initialTargetPosition = ref _initialTargetPosition.Get(entity);
                    targetPosition = initialTargetPosition.Value;
                }

                if (config.Type is MoveTransformConfig.MoveType.Warp) {
                    transformFromConfig.Transform.position = targetPosition;
                    _active.Remove(entity);
                    continue;
                }

                ref var duration = ref _duration.Get(entity);

                var arcOffset = new Vector3 {
                    x = config.XOffset.Evaluate(duration.PercentElapsed),
                    y = config.YOffset.Evaluate(duration.PercentElapsed),
                    z = config.ZOffset.Evaluate(duration.PercentElapsed),
                };

                switch (config.Type) {
                    case MoveTransformConfig.MoveType.Lerp: {
                        ref var initialPosition = ref _initialPosition.Get(entity);
                        ref var previousDirection = ref _direction.Get(entity);
                        ref var distanceToTarget = ref _distanceToTarget.Get(entity);

                        arcOffset.y *= distanceToTarget.Value * config.YDistanceModifier;

                        var nextPosition = Vector3.LerpUnclamped(initialPosition.Value, targetPosition,
                            duration.PercentElapsed) + arcOffset;

                        if (duration.PercentElapsed < 1)
                            previousDirection.Value = nextPosition - transformFromConfig.Transform.position;
                        else
                            nextPosition = transformFromConfig.Transform.position + previousDirection.Value;

                        transformFromConfig.Transform.position = nextPosition;
                        // Debug.Log($"Moving {transformFromConfig.Transform.name} to {nextPosition}");
                    }
                        break;

                    case MoveTransformConfig.MoveType.Direction: {
                        Vector3 direction;

                        
                        switch (config.Direction.Value) {
                            case Direction.TargetAwayFromUser:
                                direction = positionFromConfig.Position - transformFromConfig.Transform.position;
                                break;

                            case Direction.TargetTowardsUser:
                                direction = transformFromConfig.Transform.position - positionFromConfig.Position;
                                break;

                            case Direction.CustomWorldDirection:
                            case Direction.CustomLocalDirection:
                                direction = config.Direction.CustomDirection;
                                break;

                            case Direction.UserTowardsTarget:
                                direction = positionFromConfig.Position - transformFromConfig.Transform.position;
                                break;

                            case Direction.UserAwayFromTarget:
                                direction = transformFromConfig.Transform.position - positionFromConfig.Position;
                                break;

                            case Direction.TransformForward:
                                direction = config.Direction.CustomTransform.forward;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        direction.Normalize();
                        direction *= config.Speed * deltaTime;

                        if (config.Direction.Value is Direction.CustomLocalDirection) {
                            transformFromConfig.Transform.localPosition += direction;
                            transformFromConfig.Transform.position += arcOffset;
                        }
                        else {
                            transformFromConfig.Transform.position += direction + arcOffset;
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