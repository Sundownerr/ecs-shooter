using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct ParticleConfig
    {
        public enum ActionType { Play = 0, Stop = 1, }

        [HideLabel] [HorizontalGroup("1", MaxWidth = 0.3f)]
        public ActionType Action;
        [HideLabel] [HorizontalGroup("1")] public ParticleSystem Target;
    }
}