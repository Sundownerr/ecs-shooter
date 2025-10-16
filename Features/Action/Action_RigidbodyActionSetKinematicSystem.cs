using System.Collections.Generic;
using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_RigidbodyActionSetKinematicSystem : ISystem
    {
        private readonly List<Rigidbody> _targets = new();
        private Filter _filter;

        // Stashes for component access
        private Stash<RigidbodyActionSetKinematic> _rigidbodyActionSetKinematic;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<RigidbodyActionSetKinematic, Active>();

            // Initialize stashes
            _rigidbodyActionSetKinematic = World.GetStash<RigidbodyActionSetKinematic>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var rigidbodyAction = ref _rigidbodyActionSetKinematic.Get(entity);
                rigidbodyAction.Config.AddRigidbodyTargets(entity, _targets);

                foreach (var rigidbody in _targets)
                    rigidbody.isKinematic = rigidbodyAction.Config.Kinematic;

                _active.Remove(entity);
            }
        }
    }
}
