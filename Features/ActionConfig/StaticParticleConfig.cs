using System;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EcsMagic.Actions
{
    [Serializable]
    public struct StaticParticleConfig
    {
        [HideLabel] [HorizontalGroup("1")] public string ParticleId;
        [HideLabel] [HorizontalGroup("1")] public PositionConfig Position;

        public void AddTo(Entity entity)
        {
            ref var playStaticParticle = ref entity.AddComponent<AbilityPlayStaticParticle>();
            playStaticParticle.ParticleId = ParticleId;

            Position.AddTo(entity);
        }
    }
}
