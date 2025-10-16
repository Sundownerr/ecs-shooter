using System;
using Ability;
using Game.AbilityComponents;
using EcsMagic.Actions;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using Game;

namespace EcsMagic.Abilities
{
    [Serializable]
    public struct StaticParticle_AbilityAction : IAbilityAction
    {
        [HideLabel]  public string ParticleId;
        [HideLabel]  public PositionConfig Position;

        public void AddTo(Entity entity)
        {
            StaticStash.AbilityPlayStaticParticle.Set(entity, new AbilityPlayStaticParticle
            {
                ParticleId = ParticleId
            });

            Position.AddTo(entity);
        }
    }
}
