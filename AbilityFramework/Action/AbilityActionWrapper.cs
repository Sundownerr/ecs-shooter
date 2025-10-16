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
    public class AbilityActionWrapper
    {
        [SerializeReference]
        [HideReferenceObjectPicker]
        [InlineProperty]
        [HideLabel]
        [BoxGroup("Groups/Action2/2", ShowLabel = false)]
        [HorizontalGroup("Groups/Action2")]
        [PropertyOrder(999)]
        public IAbilityAction Action = CreateDefault();

        private static IAbilityAction CreateDefault()
        {
#if UNITY_EDITOR
            return (IAbilityAction) Activator.CreateInstance(NameToType().First().Value.First().Value);
#endif
            return null;
        }

        public Entity CreateEntity(World world, Entity parentEntity, Entity userEntity, Entity targetProviderEntity)
        {
            var actionEntity = world.CreateEntity();
            StaticStash.ParentEntity.Set(actionEntity, new ParentEntity {Entity = parentEntity,});
            StaticStash.UserEntity.Set(actionEntity, new UserEntity {Entity = userEntity,});
            StaticStash.TargetsProviderEntity.Set(actionEntity, new TargetsProviderEntity {
                Entity = targetProviderEntity,
            });

            Action.AddTo(actionEntity);

            return actionEntity;
        }

#if UNITY_EDITOR

        [OnValueChanged(nameof(ChangeNames))]
        [ValueDropdown(nameof(Groups))]
        [HideLabel]
        [BoxGroup("Groups", ShowLabel = false)]
        [HorizontalGroup("Groups/Action2", MaxWidth = 0.3f, MinWidth = 0.3f)]
        [VerticalGroup("Groups/Action2/1")]
        public int Group = SetDefaultGroup();

        private static int SetDefaultGroup() => NameToType().First().Key;

        [OnValueChanged(nameof(ChangeAction))]
        [ValueDropdown(nameof(Names))]
        [HideLabel]
        [VerticalGroup("Groups/Action2/1")]
        [ShowIf(nameof(NameShouldBeVisible))]
        public int Name = SetDefaultName();

        private static int SetDefaultName() => NameToType().First().Value.First().Key;

        private static Dictionary<int, Dictionary<int, Type>> NameToType() => AbilityActionNames.NameToType;
        private IEnumerable Names() => AbilityActionNames.NamesInGroup(Group);
        private IEnumerable Groups() => AbilityActionNames.Groups();
        private bool NameShouldBeVisible() => NameToType()[Group].Count > 1;

        private void ChangeAction()
        {
            if (!NameToType().ContainsKey(Group) ||
                !NameToType()[Group].ContainsKey(Name)) {
                Action = null;
                return;
            }

            var newInstance = Activator.CreateInstance(NameToType()[Group][Name]);

            if (Action != null && Action.GetType() == newInstance.GetType())
                return;

            Action = (IAbilityAction) newInstance;
        }

        private void ChangeNames()
        {
            // if selected name is not in the list (e.g. it was from another group)
            if (!NameToType()[Group].ContainsKey(Name)) {
                foreach (var name in Names()) {
                    Name = ((ValueDropdownItem<int>) name).Value;
                    break;
                }

                ChangeAction();
                return;
            }

            ChangeAction();
        }
#endif
    }
}