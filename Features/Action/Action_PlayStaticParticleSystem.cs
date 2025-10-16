using System;
using EcsMagic.CommonComponents;
using Game;
using Game.AbilityComponents;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_PlayStaticParticleSystem : ISystem
    {
        private Filter _filter;
        private readonly RuntimeData _runtimeData;
        private Stash<AbilityPlayStaticParticle> _playStaticParticle;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<Active> _active;

        public Action_PlayStaticParticleSystem(DataLocator dataLocator)
        {
            _runtimeData = dataLocator.Get<RuntimeData>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<AbilityPlayStaticParticle, Active>();

            // Initialize stashes
            _playStaticParticle = World.GetStash<AbilityPlayStaticParticle>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var playStaticParticle = ref _playStaticParticle.Get(entity);
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);

                // Get the particle emitter from StaticParticles
                var particleEmitter = _runtimeData.StaticParticles.WithId(playStaticParticle.ParticleId);

                // Emit the particle at the specified position
                particleEmitter.EmitAt(positionFromConfig.Position);

                // Remove the Active component to mark this action as completed
                _active.Remove(entity);
            }
        }
    }
}
