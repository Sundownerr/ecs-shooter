using System;
using Game;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct Cooldown_AbilityCooldown : IAbilityCooldown
    {
        [SuffixLabel("sec", Overlay = true)]
        public float Duration;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityCooldown.Add(entity);
            StaticStash.IncreasingTimer.Set(entity, new IncreasingTimer
            {
                Duration = Duration,
            });
            StaticStash.TimerCompleted.Add(entity);
        }
    }
}
