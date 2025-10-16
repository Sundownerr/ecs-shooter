using System;
using Ability;
using EcsMagic.Actions;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;
using Direction = Game.Direction;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct Rigidbody_AbilityAction : IAbilityAction
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

        public void AddTo(Entity entity)
        {
            var config = new RigidbodyConfig {
                TargetType = TargetType,
                OtherTarget = OtherTarget,
                ActionType = ActionType,
                Direction = Direction,
                Force = Force,
                ForceMode = ForceMode,
                CustomDirection = CustomDirection,
                Kinematic = Kinematic,
            };

            switch (ActionType) {
                case RigidbodyActionType.AddForce or RigidbodyActionType.AddExplosionForce:
                    StaticStash.RigidbodyActionAddForce.Set(entity, new RigidbodyActionAddForce {Config = config,});
                    break;

                case RigidbodyActionType.SetKinematic:
                    StaticStash.RigidbodyActionSetKinematic.Set(entity,
                        new RigidbodyActionSetKinematic {Config = config,});
                    break;
            }
        }

        private bool IsSetKinematic() => ActionType == RigidbodyActionType.SetKinematic;

        private bool IsAnyAddForce() =>
            ActionType is RigidbodyActionType.AddForce or RigidbodyActionType.AddExplosionForce;

        private bool IsAddForce() => ActionType == RigidbodyActionType.AddForce;
        private bool IsOtherTarget() => TargetType == TargetType.Other;

        private bool IsCustomDirection() =>
            IsAddForce() && Direction is Direction.CustomWorldDirection or Direction.CustomLocalDirection;
    }
}