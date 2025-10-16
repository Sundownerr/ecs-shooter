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
    public struct Damage_AbilityAction : IAbilityAction
    {
        [HideLabel] public int Value;
        [HorizontalGroup("Target", MaxWidth = 0.2f)]
        [HideLabel] public TargetType TargetType;

        public void AddTo(Entity entity) =>
            StaticStash.ApplyDamage.Set(entity, new ApplyDamage {
                Config = new DamageConfig {
                    Value = Value,
                    TargetType = TargetType,
                },
            });
    }
}