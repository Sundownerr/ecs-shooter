using System.Collections.Generic;

namespace Ability.Identifications
{
    public static class CooldownId
    {
        public const int Cooldown = 0;
        public const int Recharge = 1;
        public const int Standard = 100;

        public static readonly Dictionary<int, string> ToName = new()
        {
            { Cooldown, "Cooldown" },
            { Recharge, "Recharge" },
            { Standard, "Standard" }
        };
    }
}
