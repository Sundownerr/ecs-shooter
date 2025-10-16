using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct SetAbilityActivatedConfig
    {
        [HideLabel] [HorizontalGroup("c")] [GUIColor(nameof(GetBoolColor))]
        public bool Activated;

        [HideLabel] [HorizontalGroup("c")]
        public TargetType _targetType;

        [HideLabel] [HorizontalGroup("c")]
        [ShowIf(nameof(IsOtherTarget))]
        public AbilityProvider Other;

        private Color GetBoolColor() => Activated ? Color.green : Color.red;

        private bool IsOtherTarget() => _targetType == TargetType.Other;
    }
}