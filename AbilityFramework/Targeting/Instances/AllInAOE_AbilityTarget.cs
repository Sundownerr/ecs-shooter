using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability.Targeting
{
    [Serializable]
    public struct AllInAOE_AbilityTarget : IAbilityTarget
    {
        [LabelText("Radius")]
        public float Range;

        [LabelText("From")]
        public Position Center;

        [ShowIf(nameof(IsCustomCenter))]
        public Transform CustomCenter;

        public LayerMask LayerMask;

        public void AddTo(Entity entity) =>
            StaticStash.AllInAOE.Set(entity, new AllInAOE {
                Config = new AOEConfig {
                    Range = Range,
                    Center = Center,
                    CustomCenter = CustomCenter,
                    LayerMask = LayerMask,
                },
            });

        private bool IsCustomCenter() => Center == Position.CustomTransform;
    }
}