using System;
using Game;
using Sirenix.OdinInspector;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct WeaponTriggerConfig
    {
        public enum Action { Pull = 0, Release = 1, }

        [HideLabel] public Action TriggerAction;
        [HideLabel] public WeaponProvider Weapon;
    }
}