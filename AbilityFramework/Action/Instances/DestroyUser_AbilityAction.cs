using System;
using Ability;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct DestroyUser_AbilityAction : IAbilityAction
    {
        public void AddTo(Entity entity)
        {
            StaticStash.DestroyUser.Add(entity);
        }
    }
}
