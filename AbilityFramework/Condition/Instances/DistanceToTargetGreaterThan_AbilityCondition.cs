using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct DistanceToTargetGreaterThan_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        public float Value;

        public void AddTo(Entity entity) =>
            StaticStash.DistanceToTargetGreaterThan.Set(entity, new DistanceToTargetGreaterThan {
                Value = Value,
            });
    }
}