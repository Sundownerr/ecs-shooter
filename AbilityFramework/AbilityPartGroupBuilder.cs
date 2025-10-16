using System;
using System.Collections.Generic;

namespace Ability.Utilities
{
    public static class AbilityActionToTypeMapBuilder
    {
        public static (int, Dictionary<int, Type>) Group(this int group) => (group, new Dictionary<int, Type>());

        public static (int, Dictionary<int, Type>) With<T>(this (int id, Dictionary<int, Type> actions) group,
                                                           int actionId)
        {
            group.actions.Add(actionId, typeof(T));
            return (group.id, group.actions);
        }
    }
}