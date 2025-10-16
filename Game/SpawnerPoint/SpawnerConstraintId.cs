using System.Collections.Generic;

namespace Game.Systems
{
    public static class SpawnerConstraintId
    {
        
        public const int InsideRadius = 1;
        public const int OutsideRadius = 2;
        public const int PlayerFacing = 3;
        
        public const int Radius = 100;
        public const int Direction = 101;

        public static readonly Dictionary<int, string> ToName = new()
        {
            { InsideRadius, "Inside Radius" },
            { OutsideRadius, "Outside Radius" },
            { PlayerFacing, "Player Facing" },

            { Radius, "Radius" },
            { Direction, "Direction" }
        };
    }
}
