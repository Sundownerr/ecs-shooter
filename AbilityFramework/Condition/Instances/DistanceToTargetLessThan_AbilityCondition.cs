using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct DistanceToTargetLessThan_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        public float Value;

        public void AddTo(Entity entity) =>
            StaticStash.DistanceToTargetLessThan.Set(entity, new DistanceToTargetLessThan {
                Value = Value,
            });
    }
}