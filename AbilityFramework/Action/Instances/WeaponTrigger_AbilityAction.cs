using System;
using Ability;
using EcsMagic.Actions;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct WeaponTrigger_AbilityAction : IAbilityAction
    {
       
        [HideLabel] public WeaponTriggerConfig.Action TriggerAction;
        [HideLabel] public WeaponProvider Weapon;

        public void AddTo(Entity entity)
        {
            StaticStash.WeaponTriggerAction.Set(entity, new WeaponTriggerAction
            {
                Config = new WeaponTriggerConfig
                {
                    TriggerAction = TriggerAction,
                    Weapon = Weapon
                }
            });
        }
    }
}
