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
    public class AbilityConditionWrapper
    {
        [SerializeReference]
        [HideReferenceObjectPicker]
        [InlineProperty]
        [HideLabel]
        [BoxGroup("Groups/Condition2/2", ShowLabel = false)]
        [HorizontalGroup("Groups/Condition2")]
        [PropertyOrder(999)]
        public IAbilityCondition Condition = CreateDefault();

        private static IAbilityCondition CreateDefault()
        {
#if UNITY_EDITOR
            return (IAbilityCondition) Activator.CreateInstance(NameToType().First().Value.First().Value);
#endif
            return null;
        }

        public static AbilityConditionWrapper OneShotCondition() =>
            new() {Condition = new AbilityActivated_AbilityCondition(),};

        public Entity CreateEntity(World world,
                                   AbilityConditionFor purpose,
                                   Entity parentEntity,
                                   Entity userEntity,
                                   Entity targetProviderEntity,
                                   int index)
        {
            var conditionEntity = world.CreateEntity();

            StaticStash.ParentEntity.Set(conditionEntity, new ParentEntity {Entity = parentEntity,});
            StaticStash.UserEntity.Set(conditionEntity, new UserEntity {Entity = userEntity,});
            StaticStash.TargetsProviderEntity.Set(conditionEntity,
                new TargetsProviderEntity {Entity = targetProviderEntity,});
            StaticStash.ConditionFulfilled.Add(conditionEntity);

            if (purpose is AbilityConditionFor.CancelingAbility) {
                StaticStash.CancellingCondition.Add(conditionEntity);

                ref var parts = ref StaticStash.CancelConditions.Get(parentEntity);
                parts.List[index] = conditionEntity;
            }
            else {
                StaticStash.ForwardCondition.Add(conditionEntity);

                ref var parts = ref StaticStash.ForwardConditions.Get(parentEntity);
                parts.List[index] = conditionEntity;
            }

            Condition.AddTo(conditionEntity);

            return conditionEntity;
        }

#if UNITY_EDITOR
        [OnValueChanged(nameof(ChangeNames))]
        [ValueDropdown(nameof(Groups))]
        [HideLabel]
        [BoxGroup("Groups", ShowLabel = false)]
        [HorizontalGroup("Groups/Condition2", MinWidth = 0.3f)]
        [VerticalGroup("Groups/Condition2/1")]
        public int Group = SetDefaultGroup();

        private static int SetDefaultGroup() => NameToType().First().Key;

        [OnValueChanged(nameof(ChangeCondition))]
        [ValueDropdown(nameof(Names))]
        [HideLabel]
        [VerticalGroup("Groups/Condition2/1")]
        [ShowIf(nameof(NameShouldBeVisible))]
        public int Name = SetDefaultName();

        private static int SetDefaultName() => NameToType().First().Value.First().Key;

        private static Dictionary<int, Dictionary<int, Type>> NameToType() => AbilityConditionNames.NameToType;
        private IEnumerable Names() => AbilityConditionNames.NamesInGroup(Group);
        private IEnumerable Groups() => AbilityConditionNames.Groups();

        private bool NameShouldBeVisible() => NameToType().ContainsKey(Group) &&
                                              NameToType()[Group].Count > 1;

        private void ChangeCondition()
        {
            if (!NameToType().ContainsKey(Group) ||
                !NameToType()[Group].ContainsKey(Name)) {
                Condition = null;
                return;
            }

            var newInstance = Activator.CreateInstance(NameToType()[Group][Name]);

            if (Condition != null && Condition.GetType() == newInstance.GetType())
                return;

            Condition = (IAbilityCondition) newInstance;
        }

        private void ChangeNames()
        {
            // if selected name is not in the list (e.g. it was from another group)
            if (!NameToType()[Group].ContainsKey(Name)) {
                foreach (var name in Names()) {
                    Name = ((ValueDropdownItem<int>) name).Value;
                    break;
                }

                ChangeCondition();
                return;
            }

            ChangeCondition();
        }

#endif
    }
}