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
    public struct ChangeComponentEnabled_AbilityAction : IAbilityAction
    {
        [HorizontalGroup("0")]
        [HideLabel]
        public Component TargetComponent;

        [HorizontalGroup("0")]
        [HideLabel]
        public bool Enabled;

        public void AddTo(Entity entity)
        {
            StaticStash.ChangeComponentEnabled.Set(entity, new ChangeComponentEnabled
            {
                Config = new ChangeComponentEnabledConfig
                {
                    TargetComponent = TargetComponent,
                    Enabled = Enabled
                }
            });
        }
    }
}
