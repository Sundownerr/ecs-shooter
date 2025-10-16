using System;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct RotateTransformConfig
    {
        public float Duration;
        [HideLabel] public TransformConfig TransformConfig;
        [HideLabel] public RotationConfig RotationConfig;

        public void AddTo(Entity entity)
        {
            entity.SetComponent(new RotateTransform {Config = this,});
            entity.SetComponent(new Duration {Max = Duration,});
            RotationConfig.AddTo(entity);
            entity.SetComponent(new TransformFromConfig {Config = TransformConfig,});
        }
    }
}