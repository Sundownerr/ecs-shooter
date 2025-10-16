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
    public struct DestroyGameObject_AbilityAction : IAbilityAction
    {
        [HideLabel] public GameObject Target;

        public void AddTo(Entity entity)
        {
            StaticStash.DestroyGameObject.Set(entity, new DestroyGameObject
            {
                Config = new DestroyGameObjectConfig
                {
                    Target = Target
                }
            });
        }
    }
}
