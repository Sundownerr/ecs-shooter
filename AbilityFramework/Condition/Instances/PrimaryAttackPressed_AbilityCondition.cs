using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct PrimaryAttackPressed_AbilityCondition : IAbilityCondition
    {
        // This condition has no parameters

        public void AddTo(Entity entity)
        {
            StaticStash.CheckPrimaryAttackPressed.Add(entity);
        }
    }
}
