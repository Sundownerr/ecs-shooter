using System;
using System.Collections.Generic;
using Ability.Identifications;
using Ability.Targeting;
using Ability.Utilities;

namespace Ability
{
    public static class AbilityTargetToTypeMap
    {
        public static List<(int, Dictionary<int, Type>)> Create() => new()
        {
            TargetId.User.Group()
                .With<UserTarget_AbilityTarget>(TargetId.TargetFromUser),

            TargetId.Radius.Group()
                .With<AllInAOE_AbilityTarget>(TargetId.AllInRadius)
                .With<LimitedInAOE_AbilityTarget>(TargetId.LimitedInAOE),

            TargetId.Provider.Group()
                .With<FromTargetProvider_AbilityTarget>(TargetId.FromTargetProvider),

            TargetId.Script.Group()
                .With<FromScript_AbilityTarget>(TargetId.FromScript),
        };
    }
}
