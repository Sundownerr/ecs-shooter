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
    public struct HealthCondition : IComponent
    {
        public float Value;
        public TargetType TargetType;
        public HealthComparisonType ComparisonType;
        public bool UsePercent; // If true, Value is a percentage of max health
    }

    public enum HealthComparisonType
    {
        Equal = 0,
        LessThan = 1,
        GreaterThan = 2,
    }
}
