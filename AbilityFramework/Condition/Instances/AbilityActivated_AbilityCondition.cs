using System;
using Game;
using Scellecs.Morpeh;

namespace Ability
{
    [Serializable]
    public struct AbilityActivated_AbilityCondition : IAbilityCondition
    {
        public void AddTo(Entity entity) =>
            StaticStash.AbilityActivated.Add(entity);
    }
}