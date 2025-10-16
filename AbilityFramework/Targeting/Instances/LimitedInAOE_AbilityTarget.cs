using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability.Targeting
{
    [Serializable]
    public struct LimitedInAOE_AbilityTarget : IAbilityTarget
    {
        public int MaxTargets;

        [LabelText("In Range")]
        public float Range;

        [HideLabel]
        public Position Center;

        [ShowIf(nameof(IsCustomCenter))]
        public Transform CustomCenter;

        public LayerMask LayerMask;

        public void AddTo(Entity entity) =>
            StaticStash.LimitedInAOE.Set(entity, new LimitedInAOE {
                Config = new LimitedInAOEConfig {
                    MaxTargets = MaxTargets,
                    Range = Range,
                    Center = Center,
                    CustomCenter = CustomCenter,
                    LayerMask = LayerMask,
                },
            });

        private bool IsCustomCenter() => Center == Position.CustomTransform;
    }
}