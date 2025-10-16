using System;
using Ability;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct Dash_AbilityAction : IAbilityAction
    {
        [HideLabel] public float Force;
        [HideLabel] public ForceMode ForceMode;
        [HideLabel] public TargetType TargetType;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityDash.Set(entity, new AbilityDash
            {
                Force = Force,
                ForceMode = ForceMode,
                TargetType = TargetType
            });
        }
    }
}
