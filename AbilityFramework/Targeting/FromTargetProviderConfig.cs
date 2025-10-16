using System;
using Sirenix.OdinInspector;

namespace Game
{
    [Serializable]
    public struct FromTargetProviderConfig
    {
        [HideLabel]
        public TargetProvider TargetProvider;
    }
}