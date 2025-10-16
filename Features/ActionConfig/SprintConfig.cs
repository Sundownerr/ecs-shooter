using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct SprintConfig
    {
        [HideLabel] public TargetType TargetType;
    }
}
