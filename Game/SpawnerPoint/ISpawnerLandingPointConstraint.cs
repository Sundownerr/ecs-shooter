using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Systems
{
    public interface ISpawnerLandingPointConstraint
    {
        bool IsValid(Entity pointEntity,
                     Vector3 playerPosition,
                     Vector3 playerForward,
                     Stash<SpawnerLandingPoint> pointsStash);
    }

  
}
