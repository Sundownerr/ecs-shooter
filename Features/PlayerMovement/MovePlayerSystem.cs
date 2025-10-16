using EcsMagic.CommonComponents;
using EcsMagic.PlayerComponenets;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MovePlayerSystem : IFixedSystem
    {
        private Filter _filter;
        private Stash<PlayerInput_MoveDirection> _playerInputMoveDirectionStash;
        private Stash<MoveDirection> _moveDirectionStash;
        private Stash<VerticalVelocity> _verticalVelocityStash;
        private Stash<DamageDirection> _damageDirectionStash;
        private Stash<Reference<Rigidbody>> _rigidbodyStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Player, MoveDirection>();
            _playerInputMoveDirectionStash = World.GetStash<PlayerInput_MoveDirection>();
            _moveDirectionStash = World.GetStash<MoveDirection>();
            _verticalVelocityStash = World.GetStash<VerticalVelocity>();
            _damageDirectionStash = World.GetStash<DamageDirection>();
            _rigidbodyStash = World.GetStash<Reference<Rigidbody>>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var moveDirection = ref _moveDirectionStash.Get(entity);
                ref var verticalVelocity = ref _verticalVelocityStash.Get(entity);
                ref var damageDirection = ref _damageDirectionStash.Get(entity);
                ref Reference<Rigidbody> rigidbody = ref _rigidbodyStash.Get(entity);
                
                // Debug.Log($"M: {moveDirection.Value} | V: {verticalVelocity.Value} | D: {damageDirection.Value}");

                rigidbody.Value.AddForce(moveDirection.Value +
                                         Vector3.up * verticalVelocity.Value +
                                         (Vector3)damageDirection.Value
                    , ForceMode.Acceleration);

                damageDirection.Value = Vector3.zero;
            }
        }
    }
}
