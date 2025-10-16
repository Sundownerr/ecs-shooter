using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public enum AbilityTimingType { Instant = 0, Channel = 2, }

    [Serializable]
    public class AbilityTiming
    {
        [HorizontalGroup("0", MaxWidth = 0.3f)]
        [HideLabel]
        public AbilityTimingType Type;

        [HorizontalGroup("0", PaddingLeft = 0.4f, PaddingRight = 0.05f)]
        [HideLabel]
        [ShowIf(nameof(IsChannel))]
        public ChannelConfig Channel;

        private bool IsChannel() => Type == AbilityTimingType.Channel;

        [Serializable]
        public class ChannelConfig
        {
            [SuffixLabel("sec", Overlay = true)]
            [Min(0.01f)]
            [LabelWidth(60)]
            public float Duration;
        }
    }
}