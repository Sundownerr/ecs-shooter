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

namespace Ability.Targeting
{
    [Serializable]
    public class AbilityTargetWrapper
    {
        [SerializeReference]
        [HideReferenceObjectPicker]
        [InlineProperty]
        [HideLabel]
        [BoxGroup("Groups/Target2/2", ShowLabel = false)]
        [HorizontalGroup("Groups/Target2")]
        [PropertyOrder(999)]
        public IAbilityTarget Target = CreateDefault();

        private static IAbilityTarget CreateDefault()
        {
#if UNITY_EDITOR
            return (IAbilityTarget) Activator.CreateInstance(NameToType().First().Value.First().Value);
#endif
            return null;
        }

        public Entity CreateEntity(World world, Entity parentEntity, Entity userEntity, Entity targetProviderEntity)
        {
            var targetEntity = world.CreateEntity();
            StaticStash.ParentEntity.Set(targetEntity, new ParentEntity {Entity = parentEntity,});
            StaticStash.UserEntity.Set(targetEntity, new UserEntity {Entity = userEntity,});
            StaticStash.TargetsProviderEntity.Set(targetEntity, new TargetsProviderEntity {
                Entity = targetProviderEntity,
            });
            StaticStash.Active.Add(targetEntity);

            Target.AddTo(targetEntity);

            return targetEntity;
        }

#if UNITY_EDITOR
        [OnValueChanged(nameof(ChangeNames))]
        [ValueDropdown(nameof(Groups))]
        [HideLabel]
        [BoxGroup("Groups", ShowLabel = false)]
        [HorizontalGroup("Groups/Target2", MaxWidth = 0.3f, MinWidth = 0.3f)]
        [VerticalGroup("Groups/Target2/1")]
        public int Group = SetDefaultGroup();

        private static int SetDefaultGroup() => NameToType().First().Key;

        [OnValueChanged(nameof(ChangeTarget))]
        [ValueDropdown(nameof(Names))]
        [HideLabel]
        [VerticalGroup("Groups/Target2/1")]
        [ShowIf(nameof(NameShouldBeVisible))]
        public int Name = SetDefaultName();

        private static int SetDefaultName() => NameToType().First().Value.First().Key;

        private static Dictionary<int, Dictionary<int, Type>> NameToType() => AbilityTargetNames.NameToType;
        private IEnumerable Names() => AbilityTargetNames.NamesInGroup(Group);
        private IEnumerable Groups() => AbilityTargetNames.Groups();
        private bool NameShouldBeVisible() => NameToType()[Group].Count > 1;

        private void ChangeTarget()
        {
            if (!NameToType().ContainsKey(Group) ||
                !NameToType()[Group].ContainsKey(Name)) {
                Target = null;
                return;
            }

            var newInstance = Activator.CreateInstance(NameToType()[Group][Name]);

            if (Target != null && Target.GetType() == newInstance.GetType())
                return;

            Target = (IAbilityTarget) newInstance;
        }

        private void ChangeNames()
        {
            // if selected name is not in the list (e.g. it was from another group)
            if (!NameToType()[Group].ContainsKey(Name)) {
                foreach (var name in Names()) {
                    Name = ((ValueDropdownItem<int>) name).Value;
                    break;
                }
                ChangeTarget();
                return;
            }

            ChangeTarget();
        }
#endif
    }
}