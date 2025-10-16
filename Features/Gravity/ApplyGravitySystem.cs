using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ApplyGravitySystem : ISystem
    {
        private Filter _filter;
        private Stash<VerticalVelocity> _verticalVelocity;
        private Stash<Gravity> _gravity;
        private Stash<IsGrounded> _isGrounded;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<VerticalVelocity, Gravity, IsGrounded>();
            _verticalVelocity = World.GetStash<VerticalVelocity>();
            _gravity = World.GetStash<Gravity>();
            _isGrounded = World.GetStash<IsGrounded>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var verticalVelocity = ref _verticalVelocity.Get(entity);
                ref var gravity = ref _gravity.Get(entity);
                ref var isGrounded = ref _isGrounded.Get(entity);

                if (!isGrounded.Value)
                    verticalVelocity.Value += gravity.Value;

                if (isGrounded.Value && verticalVelocity.Value < 0.0f)
                    // stop our velocity dropping infinitely when grounded
                    verticalVelocity.Value = gravity.Value;
                
                // Debug.Log($"Entity: {entity} VerticalVelocity: {verticalVelocity.Value} Gravity: {gravity.Value} IsGrounded: {isGrounded.Value}");
            }
        }
    }
}
