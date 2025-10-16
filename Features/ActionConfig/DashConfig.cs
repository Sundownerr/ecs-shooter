using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct DashConfig
    {
        [HideLabel] public TargetType TargetType;
        [HorizontalGroup("F")]
        [HideLabel] public float Force;
        [HorizontalGroup("F")]
        [HideLabel] public ForceMode ForceMode;
    }
}
