using System;
using Ability;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct ChangeFloatStat_AbilityAction : IAbilityAction
    {
        [HideLabel]
        [ValueDropdown("@Game.Stat.GetStatNames()")]
        [HorizontalGroup("Value", MaxWidth = 0.3f)]
        public int StatId;

        [HideLabel]
        [HorizontalGroup("Value")]
        public float Value;

        [HideLabel]
        public TargetType TargetType;

        public void AddTo(Entity entity) =>
            StaticStash.AbilityChangeFloatStatValue.Set(entity, new AbilityChangeFloatStatValue {
                StatId = StatId,
                Value = Value,
                TargetType = TargetType,
            });
    }
}