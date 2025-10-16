using System;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.AbilityComponents
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct GatheredYellowCubesCondition : IComponent
    {
        public float Value;
        public YellowCubesComparisonType ComparisonType;
        public bool UsePercent; // If true, Value is a percentage of total yellow cubes
    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TotalYellowCubesCondition : IComponent
    {
        public float Value;
        public YellowCubesComparisonType ComparisonType;
        public bool UsePercent; // If true, Value is a percentage
    }

    public enum YellowCubesComparisonType
    {
        LessThan = 0,
        MoreThan = 1,
    }
}
