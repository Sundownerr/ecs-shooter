using EcsMagic.CommonComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateIsGroundedSystem : ISystem
    {
        private Filter _filter;
        private Stash<Reference<Transform>> _transform;
        private Stash<IsGrounded> _isGrounded;
        private Stash<IsGroundedSettings> _isGroundedSettings;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Reference<Transform>, IsGrounded, IsGroundedSettings>();
            _transform = World.GetStash<Reference<Transform>>();
            _isGrounded = World.GetStash<IsGrounded>();
            _isGroundedSettings = World.GetStash<IsGroundedSettings>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref Reference<Transform> transform = ref _transform.Get(entity);
                ref var isGrounded = ref _isGrounded.Get(entity);
                ref var settings = ref _isGroundedSettings.Get(entity);

                // set sphere position, with offset
                var position = transform.Value.position;
                position.y -= settings.Offset;

                isGrounded.Value = Physics.CheckSphere(position, settings.Radius, settings.GroundMask,
                    QueryTriggerInteraction.Ignore);
            }
        }
    }
}
