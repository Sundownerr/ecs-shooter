using System;
using Ability;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct RegenerateFullHealth_AbilityAction : IAbilityAction
    {
        [HorizontalGroup("Target", MaxWidth = 0.2f)]
        [HideLabel] public TargetType TargetType;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityRegenerateFullHealth.Set(entity, new AbilityRegenerateFullHealth
            {
                TargetType = TargetType
            });
        }
    }
}
