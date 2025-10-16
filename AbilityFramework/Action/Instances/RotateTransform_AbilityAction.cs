using System;
using Ability;
using Game;
using Game.AbilityComponents;
using EcsMagic.Actions;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct RotateTransform_AbilityAction : IAbilityAction
    {
        public float Duration;
        [HideLabel] public TransformConfig TransformConfig;
        [HideLabel] public RotationConfig RotationConfig;

        public void AddTo(Entity entity)
        {
            StaticStash.RotateTransform.Set(entity, new RotateTransform
            {
                Config = new RotateTransformConfig
                {
                    Duration = Duration,
                    TransformConfig = TransformConfig,
                    RotationConfig = RotationConfig
                }
            });

            StaticStash.Duration.Set(entity, new Duration { Max = Duration });
            RotationConfig.AddTo(entity);
            StaticStash.TransformFromConfig.Set(entity, new TransformFromConfig { Config = TransformConfig });
        }
    }
}
