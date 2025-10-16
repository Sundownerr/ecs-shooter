using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct LimitedInAOEConfig
    {
        public int MaxTargets;
        [LabelText("In Range")]
        public float Range;
        [HideLabel]
        public Position Center;

        [ShowIf(nameof(IsCustomCenter))]
        public Transform CustomCenter;
        public LayerMask LayerMask;

        private bool IsCustomCenter() => Center == Position.CustomTransform;
    }
}