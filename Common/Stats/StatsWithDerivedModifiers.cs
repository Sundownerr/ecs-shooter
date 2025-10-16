using System.Collections.Generic;

namespace Game
{
    public class StatsWithDerivedModifiers<T> : Stats<T>
    {
        public List<Stats<T>> DerivedStats = new();

        public StatsWithDerivedModifiers<T> AddDerived(Stats<T>stats)
        {
            DerivedStats.Add(stats);
            RecalculateAll();
            return this;
        }

        public StatsWithDerivedModifiers<T> RemoveDerived(Stats<T>stats)
        {
            DerivedStats.Remove(stats);
            RecalculateAll();

            return this;
        }

        protected override void Recalculate(T stat)
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

            foreach (var derivedStat in DerivedStats) {
                foreach (var derivedModifier in derivedStat.Modifier[stat]) {
                    switch (derivedModifier.Type) {
                        case ModifierType.Flat:
                            value += derivedModifier.Value;
                            break;

                        case ModifierType.Percentage:
                            percentMultiplier += derivedModifier.Value;
                            break;
                    }
                }
            }

            ValueWithModifiers[stat] = value * percentMultiplier;
        }
    }
}