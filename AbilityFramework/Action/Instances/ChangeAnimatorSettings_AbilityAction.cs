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
    public struct ChangeAnimatorSettings_AbilityAction : IAbilityAction
    {
       
        [HideLabel] 
        [HorizontalGroup("T", MaxWidth = 0.2f)] 
        public TargetType _targetType;
        
        [HideLabel]
        [HorizontalGroup("T")]
        [ShowIf(nameof(IsOtherTarget))]
        public Animator Other;
        
        [HideLabel] 
        [HorizontalGroup("1")] 
        public ChangeAnimatorSettignsConfig.ChangeType Type;

        [HideLabel]
        [HorizontalGroup("1")]
        [ShowIf(nameof(IsBoolType))]
        [GUIColor(nameof(GetBoolColor))]
        public bool BoolValue;

        [HideLabel]
        [HorizontalGroup("1")]
        [HideIf(nameof(IsAnimatorSpeedType))]
        public string ParameterName;

        [HideLabel]
        [HorizontalGroup("1")]
        [ShowIf(nameof(IsFloatType))]
        public float FloatValue;
        [HideLabel]
        [HorizontalGroup("1")]
        [ShowIf(nameof(IsIntType))]
        public int IntValue;
        [HideLabel]
        [HorizontalGroup("1")]
        [ShowIf(nameof(IsTriggerType))]
        public bool TriggerValue;
        [HideLabel]
        [HorizontalGroup("1")]
        [ShowIf(nameof(IsAnimatorSpeedType))]
        public float AnimatorSpeedValue;

        public void AddTo(Entity entity)
        {
            StaticStash.ChangeAnimatorSettings.Set(entity, new ChangeAnimatorSettings
            {
                Config = new ChangeAnimatorSettignsConfig
                {
                    _targetType = _targetType,
                    Other = Other,
                    Type = Type,
                    BoolValue = BoolValue,
                    ParameterName = ParameterName,
                    FloatValue = FloatValue,
                    IntValue = IntValue,
                    TriggerValue = TriggerValue,
                    AnimatorSpeedValue = AnimatorSpeedValue
                }
            });
        }

        private Color GetBoolColor() => BoolValue ? Color.green : Color.red;
        private bool IsOtherTarget() => _targetType == TargetType.Other;
        private bool IsBoolType() => Type == ChangeAnimatorSettignsConfig.ChangeType.SetBool;
        private bool IsFloatType() => Type == ChangeAnimatorSettignsConfig.ChangeType.SetFloat;
        private bool IsIntType() => Type == ChangeAnimatorSettignsConfig.ChangeType.SetInt;
        private bool IsTriggerType() => Type == ChangeAnimatorSettignsConfig.ChangeType.SetTrigger;
        private bool IsAnimatorSpeedType() => Type == ChangeAnimatorSettignsConfig.ChangeType.SetAnimatorSpeed;
    }
}
