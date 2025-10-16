using System;
using EcsMagic.Actions;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ability
{
    [Serializable]
    public struct CheckRaycastHit_AbilityCondition : IAbilityCondition
    {
        public Transform OriginTransform;
        public DirectionConfig Direction;
        public float MaxDistance;
        public LayerMask LayerMask;

        public void AddTo(Entity entity)
        {
            StaticStash.CheckRaycastHitCondition.Set(entity, new CheckRaycastHitCondition
            {
                OriginTransform = OriginTransform,
                Direction = Direction,
                MaxDistance = MaxDistance,
                LayerMask = LayerMask
            });
        }
    }
}
