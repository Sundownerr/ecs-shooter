using System.Collections.Generic;

namespace Ability.Identifications
{
    public static class TargetId
    {
        public const int User = 0;
        public const int Radius = 1;
        public const int Provider = 2;
        public const int Script = 3;
        public const int TargetFromUser = 4;
        public const int AllInRadius = 5;
        public const int LimitedInAOE = 6;
        public const int FromTargetProvider = 7;
        public const int FromScript = 8;

        public static readonly Dictionary<int, string> ToName = new()
        {
            { User, "From User" },
            { Radius, "Radius" },
            { Provider, "Provider" },
            { Script, "Script" },
            { TargetFromUser, "Target From User" },
            { AllInRadius, "All in Radius" },
            { LimitedInAOE, "Limited in Radius" },
            { FromTargetProvider, "From Target Provider" },
            { FromScript, "From Script" },
        };
    }
}
