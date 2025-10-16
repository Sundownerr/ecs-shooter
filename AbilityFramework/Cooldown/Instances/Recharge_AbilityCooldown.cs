using System;
using Game;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct Recharge_AbilityCooldown : IAbilityCooldown
    {
        public int MaxCharges;
        [SuffixLabel("sec", Overlay = true)]
        public float RechargeTime;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityRecharge.Set(entity, new AbilityRecharge
            {
                MaxCharges = MaxCharges,
                RechargeTime = RechargeTime,
            });
            StaticStash.IncreasingTimer.Set(entity, new IncreasingTimer
            {
                Duration = RechargeTime,
            });
            StaticStash.TimerCompleted.Add(entity);
        }
    }
}
