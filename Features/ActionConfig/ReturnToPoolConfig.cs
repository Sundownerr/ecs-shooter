using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct ReturnToPoolConfig
    {
        [HideLabel] public GameObject Target;
    }
}
