using System;
using Game;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct WeaponTrigger_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        public WeaponTriggerCondition WeaponTriggerCondition;

        [HideLabel]
        public WeaponProvider WeaponInstance;

        public void AddTo(Entity entity)
        {
            switch (WeaponTriggerCondition) {
                case WeaponTriggerCondition.Pulled:
                    StaticStash.CheckWeaponTriggerPulled.Set(entity, new CheckWeaponTriggerPulled {
                        Weapon = WeaponInstance.Entity,
                    });
                    break;

                case WeaponTriggerCondition.Released:
                    StaticStash.CheckWeaponTriggerReleased.Set(entity, new CheckWeaponTriggerReleased {
                        Weapon = WeaponInstance.Entity,
                    });
                    break;
            }
        }
    }
}