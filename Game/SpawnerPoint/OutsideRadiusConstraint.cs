using System;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems
{
    [Serializable]
    public class OutsideRadiusConstraint : ISpawnerLandingPointConstraint
    {
        [HideLabel]
        public float Radius;

        public bool IsValid(Entity pointEntity,
                            Vector3 playerPosition,
                            Vector3 playerForward,
                            Stash<SpawnerLandingPoint> pointsStash)
        {
            ref var point = ref pointsStash.Get(pointEntity);
            var distanceSquared = Vector3.SqrMagnitude(playerPosition - point.Position);
            return distanceSquared > Radius * Radius;
        }
    }
}