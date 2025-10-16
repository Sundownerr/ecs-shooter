using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct RigidbodyConfig
    {
        [HideLabel] public TargetType TargetType;
        [HideLabel]
        [ShowIf(nameof(IsOtherTarget))]
        public Rigidbody OtherTarget;
        [HorizontalGroup("F")]
        [HideLabel] public RigidbodyActionType ActionType;

        [HideLabel]
        [ShowIf(nameof(IsAddForce))]
        public Direction Direction;

        [HorizontalGroup("F")]
        [ShowIf(nameof(IsAnyAddForce))]
        [HideLabel] public float Force;

        [HorizontalGroup("F")]
        [ShowIf(nameof(IsAnyAddForce))]
        [HideLabel] public ForceMode ForceMode;

        [ShowIf(nameof(IsCustomDirection))]
        public Vector3 CustomDirection;

        [HorizontalGroup("F")]
        [ShowIf(nameof(IsSetKinematic))]
        [HideLabel]
        public bool Kinematic;

        private bool IsSetKinematic() => ActionType == RigidbodyActionType.SetKinematic;

        private bool IsAnyAddForce() =>
            ActionType is RigidbodyActionType.AddForce or RigidbodyActionType.AddExplosionForce;

        private bool IsAddForce() => ActionType == RigidbodyActionType.AddForce;
        private bool IsOtherTarget() => TargetType == TargetType.Other;

        private bool IsCustomDirection() => IsAddForce() &&
                                            Direction is Direction.CustomWorldDirection
                                                         or Direction.CustomLocalDirection;
    }
}