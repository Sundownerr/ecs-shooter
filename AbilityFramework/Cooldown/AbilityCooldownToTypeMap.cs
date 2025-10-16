using System;
using System.Collections.Generic;
using Ability.Identifications;
using Ability.Utilities;

namespace Ability
{
    public static class AbilityCooldownToTypeMap
    {
        public static List<(int, Dictionary<int, Type>)> Create() => new()
        {
            CooldownId.Cooldown.Group()
                .With<Cooldown_AbilityCooldown>(CooldownId.Standard),

            CooldownId.Recharge.Group()
                .With<Recharge_AbilityCooldown>(CooldownId.Standard),
        };
    }
}
