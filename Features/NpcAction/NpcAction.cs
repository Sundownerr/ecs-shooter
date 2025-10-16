using System;
using System.Collections.Generic;
using EcsMagic.Actions;
using EcsMagic.CommonComponents;
using Game.NpcComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Game
{
    [Serializable]
    public class NpcAction
    {
        public enum Type
        {
            StopMove = 0,
            Move = 1,
            StopFollowTarget = 3,
            Retreat = 4,
            Idle = 5,
            LookForTarget = 6,
        }

        [HorizontalGroup("0", MaxWidth = 0.35f)]
        [HideLabel] public Type ActionType;

        [HorizontalGroup("0")]
        [HideLabel] [ShowIf(nameof(IsMove))]
        public MoveConfig MoveConfigValue;

        [HorizontalGroup("0")]
        [HideLabel] [ShowIf(nameof(IsRetreat))]
        public RetreatConfig RetreatConfigValue;

        private bool IsRetreat() => ActionType == Type.Retreat;

        private bool IsMove() => ActionType == Type.Move;

        public void AddTo(Entity userEntity, World world, List<Entity> list)
        {
            var entity = world.CreateEntity();

            ref var user = ref entity.AddComponent<UserEntity>();
            user.Entity = userEntity;

            list.Add(entity);

            switch (ActionType) {
                case Type.StopMove:
                    break;

                case Type.Move:
                    MoveConfigValue.AddTo(entity);
                    break;

                case Type.StopFollowTarget:
                    entity.AddComponent<NpcAction_StopFollowTarget>();
                    break;

                case Type.Retreat:
                    RetreatConfigValue.AddTo(entity);
                    break;

                case Type.Idle:
                    entity.AddComponent<NpcAction_Idle>();
                    break;

                case Type.LookForTarget:
                    entity.AddComponent<NpcAction_LookForTarget>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum NpcMoveType { ToPoint = 0, InDirection = 1, }

    [Serializable]
    // looks like this: T  U---O
    // T - target
    // U - user
    // --- - min distance
    // O - random point in radius
    public struct RetreatConfig
    {
        public float MinDistance;
        public float Radius;

        public void AddTo(Entity entity)
        {
            ref var retreat = ref entity.AddComponent<NpcAction_Retreat>();
            retreat.MinDistance = MinDistance;
            retreat.Radius = Radius;
        }
    }

    [Serializable]
    public struct MoveConfig
    {
        [HideLabel]
        public NpcMoveType Type;

        [HideLabel] [InlineProperty] [ShowIf(nameof(IsMoveToPoint))]
        public PositionConfig Position;

        [HideLabel] [InlineProperty] [ShowIf(nameof(IsDirectionMove))]
        public DirectionConfig Direction;

        [HideLabel] [InlineProperty] [ShowIf(nameof(IsDirectionMove))]
        public DistanceConfig Distance;

        private bool IsDirectionMove() => Type is NpcMoveType.InDirection;

        private bool IsMoveToPoint() => Type is NpcMoveType.ToPoint;

        public void AddTo(Entity entity)
        {
            if (Type is not NpcMoveType.InDirection)
                return;

            switch (Direction.Value) {
                case Game.Direction.TargetAwayFromUser:
                    break;

                case Game.Direction.TargetTowardsUser:
                    break;

                case Game.Direction.UserAwayFromTarget:
                    break;

                case Game.Direction.UserTowardsTarget:
                    entity.SetComponent(new NpcAction_FollowTarget {MinDistance = Distance.Value,});
                    break;

                case Game.Direction.CustomWorldDirection:
                    break;

                case Game.Direction.CustomLocalDirection:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}