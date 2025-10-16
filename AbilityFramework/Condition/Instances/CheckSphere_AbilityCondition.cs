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
    public struct CheckSphere_AbilityCondition : IAbilityCondition
    {
        public float Radius;
        public Transform CenterTransform;
        public LayerMask LayerMask;

        public void AddTo(Entity entity)
        {
            StaticStash.CheckSphereCondition.Set(entity, new CheckSphereCondition
            {
                Radius = Radius,
                CenterTransform = CenterTransform,
                LayerMask = LayerMask
            });
        }
    }
}
