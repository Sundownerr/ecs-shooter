using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct DirectionConfig
    {
        [HideLabel]
        public Direction Value;

        [HideLabel] [ShowIf(nameof(IsCustomDirection))]
        public Vector3 CustomDirection;

        [HideLabel] [ShowIf(nameof(IsCustomTransform))]
        public Transform CustomTransform;

        private bool IsCustomDirection() => Value is Direction.CustomWorldDirection or Direction.CustomLocalDirection;
        private bool IsCustomTransform() => Value is Direction.TransformForward;
    }
}