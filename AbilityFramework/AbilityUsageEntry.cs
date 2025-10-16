using System;
using System.Collections.Generic;
using Ability;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AbilityUsageEntry
    {
        public enum Use
        {
            [LabelText("sec")]
            Time = 0,
            [LabelText("%")]
            Percent = 1,
        }

        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [HideLabel] [HorizontalGroup("0")] [PropertyRange(0, nameof(GetMaxValue))]
        public float Value;

        [PropertySpace(SpaceBefore = 10, SpaceAfter = 0)]
        [HideLabel] [HorizontalGroup("0", MaxWidth = 0.1f)]
        public Use Type;

        [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
        [LabelText(" ", SdfIconType.LightningChargeFill)]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        public List<AbilityActionWrapper> NewAbilityActions;

        [HideInInspector]
        public float MaxTimeValue = 999f;

        private float GetMaxValue() => Type == Use.Time ? MaxTimeValue : 1f;

        public void AddToAbility(ref UsageParts parts,
                                 World world,
                                 Entity parentEntity,
                                 Entity userEntity,
                                 Entity targetProviderEntity)
        {
            foreach (var abilityActionWrapper in NewAbilityActions) {
                parts.UsagePartActions[this].Add(
                    abilityActionWrapper.CreateEntity(world, parentEntity, userEntity, targetProviderEntity));
            }
        }
    }
}