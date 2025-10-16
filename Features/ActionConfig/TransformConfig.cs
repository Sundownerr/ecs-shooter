using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct TransformConfig
    {
        [HideLabel] public TargetTransform TargetTransform;
        [HideLabel] [ShowIf(nameof(IsCustomTargetTransform))]
        public Transform CustomTargetTransform;
        private bool IsCustomTargetTransform() => TargetTransform == TargetTransform.OtherTransform;
    }
}