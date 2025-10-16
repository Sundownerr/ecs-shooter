using System;
using Game;
using Sirenix.OdinInspector;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct DamageConfig
    {
        [HideLabel] public int Value;
        [HideLabel] public TargetType TargetType;
    }
}