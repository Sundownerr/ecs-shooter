using System;
using EcsMagic.Abilities;
using EcsMagic.Actions;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_PlayParticle : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<ParticleAction> _particleAction;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ParticleAction, Active>();

            // Initialize stashes
            _particleAction = World.GetStash<ParticleAction>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var playParticle = ref _particleAction.Get(entity);

                switch (playParticle.Config.Action)
                {
                    case ParticleConfig.ActionType.Play:
                        playParticle.Config.Target.Play(true);
                        break;

                    case ParticleConfig.ActionType.Stop:
                        playParticle.Config.Target.Stop(true);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _active.Remove(entity);
            }
        }
    }
}
