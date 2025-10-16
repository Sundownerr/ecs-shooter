using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability
{
    [Serializable]
    public struct TotalYellowCubes_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        [HorizontalGroup("TotalYellowCubesConfig", MaxWidth = 0.35f)]
        public YellowCubesComparisonType ComparisonType;

        [HorizontalGroup("TotalYellowCubesConfig", MaxWidth = 0.35f)]
        [HideLabel]
        public float Value;

        [LabelText("%")]
        [LabelWidth(10)]
        [HorizontalGroup("TotalYellowCubesConfig")]
        [ToggleLeft]
        public bool UsePercent; // If true, Value is a percentage

        public void AddTo(Entity entity)
        {
            StaticStash.TotalYellowCubesCondition.Set(entity, new TotalYellowCubesCondition
            {
                Value = Value,
                ComparisonType = ComparisonType,
                UsePercent = UsePercent
            });
        }
    }
}