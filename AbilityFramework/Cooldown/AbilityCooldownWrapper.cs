using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ability.Utilities;
using EcsMagic.CommonComponents;
using Game;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability
{
    [Serializable]
    public class AbilityCooldownWrapper
    {
        [SerializeReference]
        [HideReferenceObjectPicker]
        [InlineProperty]
        [HideLabel]
        [BoxGroup("Groups/Cooldown2/2", ShowLabel = false)]
        [HorizontalGroup("Groups/Cooldown2")]
        [PropertyOrder(999)]
        public IAbilityCooldown Cooldown = CreateDefault();

        private static IAbilityCooldown CreateDefault()
        {
#if UNITY_EDITOR
            return (IAbilityCooldown) Activator.CreateInstance(NameToType().First().Value.First().Value);
#endif
            return null;
        }

        public Entity CreateEntity(World world, Entity parentEntity)
        {
            var cooldownEntity = world.CreateEntity();
            StaticStash.ParentEntity.Set(cooldownEntity, new ParentEntity {Entity = parentEntity,});

            Cooldown.AddTo(cooldownEntity);

            return cooldownEntity;
        }

#if UNITY_EDITOR
        [OnValueChanged(nameof(ChangeNames))]
        [ValueDropdown(nameof(Groups))]
        [HideLabel]
        [BoxGroup("Groups", ShowLabel = false)]
        [HorizontalGroup("Groups/Cooldown2", MaxWidth = 0.3f, MinWidth = 0.3f)]
        [VerticalGroup("Groups/Cooldown2/1")]
        public int Group = SetDefaultGroup();

        private static int SetDefaultGroup() => NameToType().First().Key;

        [OnValueChanged(nameof(ChangeCooldown))]
        [ValueDropdown(nameof(Names))]
        [HideLabel]
        [VerticalGroup("Groups/Cooldown2/1")]
        [ShowIf(nameof(NameShouldBeVisible))]
        public int Name = SetDefaultName();

        private static int SetDefaultName() => NameToType().First().Value.First().Key;

        private static Dictionary<int, Dictionary<int, Type>> NameToType() => AbilityCooldownNames.NameToType;
        private IEnumerable Names() => AbilityCooldownNames.NamesInGroup(Group);
        private IEnumerable Groups() => AbilityCooldownNames.Groups();
        private bool NameShouldBeVisible() => NameToType()[Group].Count > 1;

        private void ChangeCooldown()
        {
            if (!NameToType().ContainsKey(Group) ||
                !NameToType()[Group].ContainsKey(Name)) {
                Cooldown = null;
                return;
            }

            var newInstance = Activator.CreateInstance(NameToType()[Group][Name]);

            if (Cooldown != null && Cooldown.GetType() == newInstance.GetType())
                return;

            Cooldown = (IAbilityCooldown) newInstance;
        }

        private void ChangeNames()
        {
            // if selected name is not in the list (e.g. it was from another group)
            if (!NameToType()[Group].ContainsKey(Name)) {
                foreach (var name in Names()) {
                    Name = ((ValueDropdownItem<int>) name).Value;
                    break;
                }

                ChangeCooldown();
                return;
            }

            ChangeCooldown();
        }

#endif
    }
}