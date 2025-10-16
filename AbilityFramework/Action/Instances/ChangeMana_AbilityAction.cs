using System;
using Ability;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct ChangeMana_AbilityAction : IAbilityAction
    {
        [HideLabel] public int ManaChange;
        [HideLabel] public TargetType TargetType;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityChangeMana.Set(entity, new AbilityChangeMana
            {
                ManaChange = ManaChange,
                TargetType = TargetType
            });
        }
    }
}
