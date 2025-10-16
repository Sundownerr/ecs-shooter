using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems
{
    [Serializable]
    public class SpawnerConstraintWrapper
    {
        [SerializeReference]
        [HideReferenceObjectPicker]
        [InlineProperty]
        [HideLabel]
        [BoxGroup("Groups/Constraint2/2", ShowLabel = false)]
        [HorizontalGroup("Groups/Constraint2")]
        [PropertyOrder(999)]
        public ISpawnerLandingPointConstraint Constraint = CreateDefault();

        private static ISpawnerLandingPointConstraint CreateDefault()
        {
#if UNITY_EDITOR
            return (ISpawnerLandingPointConstraint) Activator.CreateInstance(NameToType().First().Value.First().Value);
#endif
            return null;
        }
        

#if UNITY_EDITOR
        [OnValueChanged(nameof(ChangeNames))]
        [ValueDropdown(nameof(Groups))]
        [HideLabel]
        [BoxGroup("Groups", ShowLabel = false)]
        [HorizontalGroup("Groups/Constraint2", MinWidth = 0.3f)]
        [VerticalGroup("Groups/Constraint2/1")]
        public int Group = SetDefaultGroup();

        private static int SetDefaultGroup() => NameToType().First().Key;

        [OnValueChanged(nameof(ChangeConstraint))]
        [ValueDropdown(nameof(Names))]
        [HideLabel]
        [VerticalGroup("Groups/Constraint2/1")]
        [ShowIf(nameof(NameShouldBeVisible))]
        public int Name = SetDefaultName();

        private static int SetDefaultName() => NameToType().First().Value.First().Key;

        private static Dictionary<int, Dictionary<int, Type>> NameToType() => SpawnerConstraintNames.NameToType;
        private IEnumerable Names() => SpawnerConstraintNames.NamesInGroup(Group);
        private IEnumerable Groups() => SpawnerConstraintNames.Groups();

        private bool NameShouldBeVisible() => NameToType().ContainsKey(Group) &&
                                              NameToType()[Group].Count > 1;

        private void ChangeConstraint()
        {
            if (!NameToType().ContainsKey(Group) ||
                !NameToType()[Group].ContainsKey(Name)) {
                Constraint = null;
                return;
            }

            var newInstance = Activator.CreateInstance(NameToType()[Group][Name]);

            if (Constraint != null && Constraint.GetType() == newInstance.GetType())
                return;

            Constraint = (ISpawnerLandingPointConstraint) newInstance;
        }

        private void ChangeNames()
        {
            // if selected name is not in the list (e.g. it was from another group)
            if (!NameToType()[Group].ContainsKey(Name)) {
                foreach (var name in Names()) {
                    Name = ((ValueDropdownItem<int>) name).Value;
                    break;
                }

                ChangeConstraint();
                return;
            }

            ChangeConstraint();
        }
#endif
    }
}