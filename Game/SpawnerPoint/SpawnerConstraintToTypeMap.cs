using System;
using System.Collections.Generic;
using Ability.Utilities;

namespace Game.Systems
{
    public static class SpawnerConstraintToTypeMap
    {
        public static List<(int, Dictionary<int, Type>)> Create() => new()
        {
            SpawnerConstraintId.Radius.Group()
                .With<InsideRadiusConstraint>(SpawnerConstraintId.InsideRadius)
                .With<OutsideRadiusConstraint>(SpawnerConstraintId.OutsideRadius),

            SpawnerConstraintId.PlayerFacing.Group()
                .With<PlayerFacingConstraint>(SpawnerConstraintId.PlayerFacing)
        };
    }
}
