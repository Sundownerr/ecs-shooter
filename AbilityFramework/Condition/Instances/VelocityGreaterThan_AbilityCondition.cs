using System;
using EcsMagic.Actions;
using Game;
using Game.Components;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct VelocityGreaterThan_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        public float Threshold;
        [HideLabel]
        public RigidbodyTargetConfig RigidbodyConfig;

        public void AddTo(Entity entity)
        {
            StaticStash.VelocityGreaterThan.Set(entity, new VelocityGreaterThan {Value = Threshold,});
            StaticStash.RigidbodyFromConfig.Set(entity, new RigidbodyFromConfig {Config = RigidbodyConfig,});
        }
    }
}