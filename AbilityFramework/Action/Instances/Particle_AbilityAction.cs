using System;
using Ability;
using EcsMagic.Actions;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct Particle_AbilityAction : IAbilityAction
    {
      
        [HideLabel]
        [HorizontalGroup("1", MaxWidth = 0.3f)]
        public ParticleConfig.ActionType Action;
        [HideLabel] [HorizontalGroup("1")] public ParticleSystem Target;

        public void AddTo(Entity entity)
        {
            StaticStash.ParticleAction.Set(entity, new ParticleAction
            {
                Config = new ParticleConfig
                {
                    Action = Action,
                    Target = Target
                }
            });
        }
    }
}
