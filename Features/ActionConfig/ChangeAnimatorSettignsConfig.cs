using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct ChangeAnimatorSettignsConfig
    {
        public enum ChangeType
        {
            SetBool = 0,
            SetFloat = 1,
            SetInt = 2,
            SetTrigger = 3,
            SetAnimatorSpeed = 4,
        }

        [HideLabel] [HorizontalGroup("T")] 
        public TargetType _targetType;
        
        [HideLabel] [HorizontalGroup("T")] [ShowIf(nameof(IsOtherTarget))]
        public Animator Other;
        
        [HideLabel] [HorizontalGroup("1")] 
        public ChangeType Type;

        [HideLabel] [HorizontalGroup("1")] [ShowIf(nameof(IsBoolType))]
        [GUIColor(nameof(GetBoolColor))]
        public bool BoolValue;

        [HideLabel] [HorizontalGroup("1")] [HideIf(nameof(IsAnimatorSpeedType))]
        public string ParameterName;

        [HideLabel] [HorizontalGroup("1")] [ShowIf(nameof(IsFloatType))]
        public float FloatValue;
        
        [HideLabel] [HorizontalGroup("1")] [ShowIf(nameof(IsIntType))]
        public int IntValue;
        
        [HideLabel] [HorizontalGroup("1")] [ShowIf(nameof(IsTriggerType))]
        public bool TriggerValue;
        
        [HideLabel] [HorizontalGroup("1")] [ShowIf(nameof(IsAnimatorSpeedType))]
        public float AnimatorSpeedValue;

        private Color GetBoolColor() => BoolValue ? Color.green : Color.red;
        private bool IsOtherTarget() => _targetType == TargetType.Other;
        private bool IsBoolType() => Type == ChangeType.SetBool;
        private bool IsFloatType() => Type == ChangeType.SetFloat;
        private bool IsIntType() => Type == ChangeType.SetInt;
        private bool IsTriggerType() => Type == ChangeType.SetTrigger;
        private bool IsAnimatorSpeedType() => Type == ChangeType.SetAnimatorSpeed;
    }
}