using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerManaRegenSystem : ISystem
    {
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake() => _filter = Entities.With<FloatStats, AccumulatedMana>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var floatStats = ref entity.GetComponent<FloatStats>();
                ref var accumulatedMana = ref entity.GetComponent<AccumulatedMana>();

                var currentMana = floatStats.Value.ValueOf(Stat.Mana);
                var maxMana = floatStats.Value.ValueOf(Stat.MaxMana);

                if (currentMana >= maxMana)
                    continue;
                
                var regenRate = floatStats.Value.ValueOf(Stat.ManaRegen);

                // Calculate how much mana to add this frame
                // This ensures we add exactly RegenPerSecond mana over the course of a second
                var manaToAddThisFrame = regenRate * deltaTime;

                // Add to accumulated mana (which tracks fractional mana)
                accumulatedMana.Value += manaToAddThisFrame;

                // If we have at least 1 point of mana accumulated, add it to the player's mana
                if (accumulatedMana.Value >= 1.0f) {
                    var manaToAdd = Mathf.Floor(accumulatedMana.Value);

                    // Add mana without rounding
                    var newMana = currentMana + manaToAdd;
                    newMana = Mathf.Min(newMana, maxMana);

                    floatStats.Value.Set(Stat.Mana, newMana);
                    accumulatedMana.Value -= manaToAdd;
                }
            }
        }
    }
}