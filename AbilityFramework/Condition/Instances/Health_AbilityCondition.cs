using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct Health_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        [HorizontalGroup("HealthConditionConfig")]
        public HealthComparisonType ComparisonType;

        [HorizontalGroup("HealthConditionConfig")]
        [HideLabel]
        public float Value;

        [HideLabel]
        [HorizontalGroup("Target", MaxWidth = 0.2f)]
        public TargetType TargetType;

        [LabelText("%")]
        [LabelWidth(10)]
        [HorizontalGroup("HealthConditionConfig")]
        [ToggleLeft]
        public bool UsePercent; // If true, Value is a percentage of max health

        public void AddTo(Entity entity) =>
            StaticStash.HealthCondition.Set(entity, new HealthCondition {
                ComparisonType = ComparisonType,
                Value = Value,
                TargetType = TargetType,
                UsePercent = UsePercent,
            });
    }
}