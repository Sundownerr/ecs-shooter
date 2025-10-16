using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct Mana_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        [HorizontalGroup("ManaConditionConfig")]
        public ManaComparisonType ComparisonType;

        [HorizontalGroup("ManaConditionConfig")]
        [HideLabel]
        public int Value;

        [HideLabel]
        [HorizontalGroup("Target", MaxWidth = 0.2f)]
        public TargetType TargetType;

        public void AddTo(Entity entity) =>
            StaticStash.ManaCondition.Set(entity, new ManaCondition {
                ComparisonType = ComparisonType,
                Value = Value,
                TargetType = TargetType,
            });
    }
}