using System;
using Game;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Direction = Game.AbilityComponents.Direction;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct MoveTransformConfig
    {
        public enum MoveType
        {
            Warp = 0, Lerp = 1, Direction = 2,
        }

        [HideLabel] public TransformConfig TransformConfig;
        [HideIf(nameof(HidePositionConfig))]
        [HideLabel] public PositionConfig PositionConfig;
        [HorizontalGroup("type", MaxWidth = 0.3f)]
        [HideLabel] public MoveType Type;
        [HorizontalGroup("type")]
        [ShowIf(nameof(IsDirection))] [HideLabel] [InlineProperty]
        public DirectionConfig Direction;
        [ShowIf(nameof(IsLerp))] public bool FollowTarget;
        [ShowIf(nameof(IsDirection))] public float Speed;
        [ShowIf(nameof(IsLerp))] public float DurationDistanceModifier;
        [ShowIf(nameof(IsLerp))] public float YDistanceModifier;
        [ShowIf(nameof(HaveDuration))] public UnityMinMaxCurve Duration;
        [ShowIf(nameof(IsLerp))] public float OvershootTime;
        [ShowIf(nameof(HaveCurve))] [LabelWidth(10)]
        public UnityMinMaxCurve XOffset;
        [ShowIf(nameof(HaveCurve))] [LabelWidth(10)]
        public UnityMinMaxCurve YOffset;
        [ShowIf(nameof(HaveCurve))] [LabelWidth(10)]
        public UnityMinMaxCurve ZOffset;

        private bool HidePositionConfig() => Type is MoveType.Direction &&
                                             Direction.Value is Game.Direction.TransformForward
                                                                or Game.Direction.CustomLocalDirection
                                                                or Game.Direction.CustomWorldDirection;

        private bool IsDirection() => Type == MoveType.Direction;

        private bool IsLerp() => Type == MoveType.Lerp;
        private bool HaveCurve() => Type is MoveType.Lerp or MoveType.Direction;
        private bool HaveDuration() => Type is MoveType.Direction or MoveType.Lerp;
        private bool HaveInitialPosition() => Type is MoveType.Lerp;
        private bool HaveDistanceToTarget() => Type is MoveType.Direction or MoveType.Lerp;

        public void AddTo(Entity entity)
        {
            // if you leave FollowTarget enabled and then switch to Direction mode, we need to disable FollowTarget
            if (Type is MoveType.Direction && FollowTarget)
                FollowTarget = false;

            entity.SetComponent(new MoveTransform {Config = this,});
            PositionConfig.AddTo(entity);

            StaticStash.TransformFromConfig.Set(entity, new TransformFromConfig {Config = TransformConfig,});

            if (HaveDuration()) {
                StaticStash.Duration.Add(entity);
                StaticStash.Direction.Add(entity);

                if (IsLerp()) {
                    StaticStash.DoNotDeactivateWhenDurationEnds.Add(entity);
                    StaticStash.LerpingMovementOvershootTimer.Set(entity, new LerpMovementOvershootTimer() {
                        Max = OvershootTime,
                    });
                }
            }

            if (HaveInitialPosition())
                StaticStash.InitialPosition.Add(entity);

            if (HaveDistanceToTarget())
                StaticStash.DistanceToTarget.Add(entity);

            if (!FollowTarget)
                StaticStash.InitialTargetPosition.Add(entity);
        }
    }
}