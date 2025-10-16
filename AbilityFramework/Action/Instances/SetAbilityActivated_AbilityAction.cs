using System;
using Ability;
using EcsMagic.Actions;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct SetAbilityActivated_AbilityAction : IAbilityAction
    {
        [HideLabel]
        [HorizontalGroup("c", MaxWidth = 0.2f, MinWidth = 0.2f)]
        public TargetType _targetType;

        [HideLabel]
        [HorizontalGroup("c")]
        [GUIColor(nameof(GetBoolColor))]
        public bool Activated;

       
        [HideLabel]
        [HorizontalGroup("c")]
        [ShowIf(nameof(IsOtherTarget))]
        public AbilityProvider Other;

        public void AddTo(Entity entity) =>
            StaticStash.SetAbilityActivated.Set(entity, new SetAbilityActivated {
                Config = new SetAbilityActivatedConfig {
                    Activated = Activated,
                    _targetType = _targetType,
                    Other = Other,
                },
            });

        private Color GetBoolColor() => Activated ? Color.green : Color.red;
        private bool IsOtherTarget() => _targetType == TargetType.Other;
    }
}