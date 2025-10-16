using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ability.Utilities;
using Sirenix.OdinInspector;

namespace Game.Systems
{
    public static class SpawnerConstraintNames
    {
        private static Dictionary<int, Dictionary<int, Type>> _nameToType;
        public static Dictionary<int, Dictionary<int, Type>> NameToType => _nameToType ??= CreateNameToTypes();

        private static Dictionary<int, Dictionary<int, Type>> CreateNameToTypes()
        {
            _nameToType = SpawnerConstraintToTypeMap.Create().ToDictionary(x => x.Item1, x => x.Item2);
            return _nameToType;
        }

        public static IEnumerable NamesInGroup(int group)
        {
            var dropdownItems = new ValueDropdownList<int>();

            foreach (var key in NameToType[group].Keys)
                dropdownItems.Add(SpawnerConstraintId.ToName[key], key);

            return dropdownItems;
        }

        public static IEnumerable Groups()
        {
            var dropdownItems = new ValueDropdownList<int>();

            foreach (var key in NameToType.Keys)
                dropdownItems.Add(SpawnerConstraintId.ToName[key], key);

            return dropdownItems;
        }
    }
}
