using System;
using Ability;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct AddFloatStatModifier_AbilityAction : IAbilityAction
    {
        [ValueDropdown("@Game.Stat.GetStatNames()")]
        [HideLabel]
        [HorizontalGroup("Value")]
        public int StatId;

        [HideLabel]
        [HorizontalGroup("Value")]
        public float Value;

        [HideLabel]
        [HorizontalGroup("Value")]
        public ModifierType ModifierType;

        [HideLabel]
        [HorizontalGroup("Target", MaxWidth = 0.2f)]
        public TargetType TargetType;
        
        [HorizontalGroup("Target")]
        [LabelText("ID:")]
        [LabelWidth(18)]
        public string ModifierId;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityAddFloatStatModifier.Set(entity, new AbilityAddFloatStatModifier
            {
                StatId = StatId,
                Value = Value,
                ModifierType = ModifierType,
                TargetType = TargetType,
                ModifierId = ModifierId
            });
        }
    }
}
