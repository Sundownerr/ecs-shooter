using System;

namespace Game
{
    public enum ModifierType { Flat = 0, Percentage = 1, }
    
    [Serializable] 
    public readonly struct StatModifier : IEquatable<StatModifier>
    {
        public readonly float Value;
        public readonly ModifierType Type;
        public readonly string Id;

        public StatModifier(float value, ModifierType type, string id)
        {
            Value = value;
            Type = type;
            Id = id;
        }

        public override bool Equals(object obj) =>
            obj is StatModifier modifier &&
            Value.Equals(modifier.Value) &&
            Type == modifier.Type && 
            Id == modifier.Id;

        public override int GetHashCode() =>
            HashCode.Combine(Value, Type, Id);

        public bool Equals(StatModifier other) =>
            Value.Equals(other.Value) && Type == other.Type;

        public static bool operator ==(StatModifier left, StatModifier right) =>
            left.Equals(right);

        public static bool operator !=(StatModifier left, StatModifier right) =>
            !left.Equals(right);

        public static StatModifier Flat(float value, string id) => new(value, ModifierType.Flat, id);

        public static StatModifier Percent(float value, string id) => new(value, ModifierType.Percentage, id);
    }
}