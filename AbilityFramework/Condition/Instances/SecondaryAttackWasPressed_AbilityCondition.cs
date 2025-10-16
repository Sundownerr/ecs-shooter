using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct SecondaryAttackWasPressed_AbilityCondition : IAbilityCondition
    {
        // This condition has no parameters

        public void AddTo(Entity entity)
        {
            StaticStash.CheckSecondaryAttackWasPressed.Add(entity);
        }
    }
}
