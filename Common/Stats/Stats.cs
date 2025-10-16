using System;
using System.Collections.Generic;

namespace Game
{
    public class Stats<T>
    {
        public Dictionary<T, List<StatModifier>> Modifier;
        public Dictionary<T, float> Value;
        public Dictionary<T, float> ValueWithModifiers;

        public Stats()
        {
            Modifier = new Dictionary<T, List<StatModifier>>();
            Value = new Dictionary<T, float>();
            ValueWithModifiers = new Dictionary<T, float>();
        }

        public float BaseValueOf(T stat) => Value[stat];
        public float ValueOf(T stat) => ValueWithModifiers[stat];

        public Stats<T> AddNew(T stat, float value)
        {
            Value.Add(stat, value);
            Modifier.Add(stat, new List<StatModifier>());
            ValueWithModifiers.Add(stat, value);
            return this;
        }

        public Stats<T> Increase(T stat, float value)
        {
            Value[stat] += value;
            Recalculate(stat);
            return this;
        }

        public Stats<T> Decrease(T stat, float value)
        {
            Value[stat] -= value;
            Recalculate(stat);
            return this;
        }

        public Stats<T> Set(T stat, float value)
        {
            Value[stat] = value;
            Recalculate(stat);
            return this;
        }

        public void AddModifier(T stat, StatModifier modifier)
        {
            Modifier[stat].Add(modifier);
            Recalculate(stat);
        }

        public void RemoveModifier(T stat, string modifierId)
        {
            for(var i = Modifier[stat].Count - 1; i >= 0; i--) {
                if (!string.Equals(Modifier[stat][i].Id, modifierId))
                    continue;

                Modifier[stat].RemoveAt(i);
                break;
            }

            Recalculate(stat);
        }

        public void RecalculateAll()
        {
            foreach (var stat in Value.Keys)
                Recalculate(stat);
        }

        protected virtual void Recalculate(T stat)
        {
            var value = Value[stat];
            var percentMultiplier = 1f;

            foreach (var modifier in Modifier[stat]) {
                switch (modifier.Type) {
                    case ModifierType.Flat:
                        value += modifier.Value;
                        break;

                    case ModifierType.Percentage:
                        percentMultiplier += modifier.Value;
                        break;
                }
            }

            ValueWithModifiers[stat] = value * percentMultiplier;
        }

        public void RemoveAllStatModifiers(T stat)
        {
            Modifier[stat].Clear();
            Recalculate(stat);
        }

        public void RemoveAllModifiers()
        {
            foreach ((var stat, List<StatModifier> modifiers) in Modifier) {
                modifiers.Clear();
                Recalculate(stat);
            }
        }

        public void RemoveAllModifiersWithId(string modifierId)
        {
            foreach ((var stat, List<StatModifier> modifiers) in Modifier) {
                for(var i = modifiers.Count - 1; i >= 0; i--) {
                    if (!string.Equals(modifiers[i].Id, modifierId))
                        continue;

                    modifiers.RemoveAt(i);
                }

                Recalculate(stat);
            }
        }

        public void ClearCopyModifiers(Stats<T> other)
        {
            foreach (var stat in other.Modifier.Keys) {
                Modifier[stat].Clear();

                foreach (var modifier in other.Modifier[stat])
                    Modifier[stat].Add(modifier);

                Recalculate(stat);
            }
        }

        public Stats<T> ClearCopyModifier(Stats<T> other, T stat)
        {
            Modifier[stat].Clear();

            foreach (var modifier in other.Modifier[stat])
                Modifier[stat].Add(modifier);

            Recalculate(stat);

            return this;
        }

        public void AddModifiersFrom(Stats<T> other)
        {
            foreach (var stat in other.Modifier.Keys) {
                foreach (var modifier in other.Modifier[stat])
                    AddModifier(stat, modifier);
            }
        }
    }
}