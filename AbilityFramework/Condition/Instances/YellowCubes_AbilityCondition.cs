using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability
{
    [Serializable]
    public struct YellowCubes_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        [HorizontalGroup("YellowCubesConfig", MaxWidth = 0.35f)]
        public YellowCubesComparisonType ComparisonType;

        [HorizontalGroup("YellowCubesConfig", MaxWidth = 0.35f)]
        [HideLabel]
        public float Value;

        [LabelText("%")]
        [LabelWidth(10)]
        [HorizontalGroup("YellowCubesConfig")]
        [ToggleLeft]
        public bool UsePercent; // If true, Value is a percentage of total yellow cubes

        public void AddTo(Entity entity)
        {
            StaticStash.YellowCubesCondition.Set(entity, new GatheredYellowCubesCondition
            {
                Value = Value,
                ComparisonType = ComparisonType,
                UsePercent = UsePercent
            });
        }
    }
}
