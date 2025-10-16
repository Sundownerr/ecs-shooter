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
    public struct SetGameObjectActive_AbilityAction : IAbilityAction
    {
        [GUIColor(nameof(GetBoolColor))]
        [HideLabel]
        [HorizontalGroup("0")]
        public bool Active;

        [HideLabel]
        [HorizontalGroup("0")]
        public GameObject Target;

        public void AddTo(Entity entity) =>
            StaticStash.SetGameObjectActive.Set(entity, new SetGameObjectActive {
                Config = new SetGameObjectActiveConfig {
                    Active = Active,
                    Target = Target,
                },
            });

        private Color GetBoolColor() => Active ? Color.green : Color.red;
    }
}