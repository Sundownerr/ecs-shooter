using System;
using EcsMagic.Actions;
using Game;
using Game.Components;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace Ability
{
    [Serializable]
    public struct VelocityLowerThan_AbilityCondition : IAbilityCondition
    {
        [HideLabel]
        public float Threshold;

        [HideLabel]
        public RigidbodyTargetConfig RigidbodyConfig;

        public void AddTo(Entity entity)
        {
            StaticStash.VelocityLowerThan.Set(entity, new VelocityLowerThan {Value = Threshold,});
            StaticStash.RigidbodyFromConfig.Set(entity, new RigidbodyFromConfig {Config = RigidbodyConfig,});
        }
    }
}