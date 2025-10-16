using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct PrimaryAttackWasPressed_AbilityCondition : IAbilityCondition
    {
        // This condition has no parameters

        public void AddTo(Entity entity)
        {
            StaticStash.CheckPrimaryAttackWasPressed.Add(entity);
        }
    }
}
