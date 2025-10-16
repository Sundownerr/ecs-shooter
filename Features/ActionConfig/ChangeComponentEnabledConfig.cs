using System;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct ChangeComponentEnabledConfig
    {
        [HorizontalGroup("0")]
        [HideLabel]
        public Component TargetComponent;

        [HorizontalGroup("0")]
        [HideLabel]
        public bool Enabled;

        public void AddTo(Entity entity)
        {
            ref var changeComponentEnabled = ref entity.AddComponent<ChangeComponentEnabled>();
            changeComponentEnabled.Config = this;
        }
    }
}