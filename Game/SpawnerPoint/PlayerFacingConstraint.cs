using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Systems
{
    [Serializable]
    public class PlayerFacingConstraint : ISpawnerLandingPointConstraint
    {
        public float AngleThreshold;

        public bool IsValid(Entity pointEntity,
                            Vector3 playerPosition,
                            Vector3 playerForward,
                            Stash<SpawnerLandingPoint> pointsStash)
        {
            var cosAngleThreshold = Mathf.Cos(AngleThreshold * Mathf.Deg2Rad);
            ref var point = ref pointsStash.Get(pointEntity);
            var directionToPoint = (point.Position - playerPosition).normalized;

            var dot = Vector3.Dot(playerForward, directionToPoint);
            return dot >= cosAngleThreshold;
        }
    }
}