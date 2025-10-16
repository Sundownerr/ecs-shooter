using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct SetGameObjectActiveConfig
    {
        [GUIColor(nameof(GetBoolColor))]
        [HideLabel] [HorizontalGroup("0")]
        public bool Active;
        [HideLabel] [HorizontalGroup("0")]
        public GameObject Target;

        private Color GetBoolColor() => Active ? Color.green : Color.red;
    }
}