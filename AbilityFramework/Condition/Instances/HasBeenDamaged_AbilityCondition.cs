using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability
{
    [Serializable]
    public struct HasBeenDamaged_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        [HorizontalGroup("HasBeenDamagedConfig")]
        public DamageCheckType CheckType;

        [HorizontalGroup("HasBeenDamagedConfig")]
        [HideLabel]
        [ShowIf(nameof(NeedsValue))]
        public float Value; // Amount or percentage depending on CheckType

        [HideLabel]
        public TargetType TargetType;

        private bool NeedsValue() => CheckType != DamageCheckType.Any;

        public void AddTo(Entity entity)
        {
            StaticStash.HasBeenDamagedCondition.Set(entity, new HasBeenDamagedCondition
            {
                CheckType = CheckType,
                Value = Value,
                TargetType = TargetType
            });
        }
    }
}
