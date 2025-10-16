using System;
using Ability;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct Sprint_AbilityAction : IAbilityAction
    {
        [HideLabel] public TargetType TargetType;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilitySprint.Set(entity, new AbilitySprint
            {
                TargetType = TargetType
            });
        }
    }
}
