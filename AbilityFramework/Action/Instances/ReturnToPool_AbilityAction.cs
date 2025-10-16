using System;
using Ability;
using Game;
using Game.AbilityComponents;
using EcsMagic.Actions;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct ReturnToPool_AbilityAction : IAbilityAction
    {
        [HideLabel] public GameObject Target;

        public void AddTo(Entity entity)
        {
            StaticStash.ReturnToPool.Set(entity, new ReturnToPool
            {
                Config = new ReturnToPoolConfig
                {
                    Target = Target
                }
            });
        }
    }
}
