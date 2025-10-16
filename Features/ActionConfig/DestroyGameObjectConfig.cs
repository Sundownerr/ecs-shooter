using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct DestroyGameObjectConfig
    {
        [HideLabel] public GameObject Target;
    }
}
