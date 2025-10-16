using EcsMagic.CommonComponents;
using EcsMagic.PlayerComponenets;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerJumpSystem : ISystem
    {
        private Filter _filter;
        private Stash<IsGrounded> _isGrounded;
        private Stash<PlayerInput_Jump_IsPressed> _jumpPressed;
        private Stash<Player> _player;
        private Stash<PlayerConfig> _playerConfig;
        private Stash<Reference<Rigidbody>> _rigidBody;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Player, PlayerConfig, PlayerInput_Jump_IsPressed>();
            _playerConfig = World.GetStash<PlayerConfig>();
            _isGrounded = World.GetStash<IsGrounded>();
            _jumpPressed = World.GetStash<PlayerInput_Jump_IsPressed>();
            _rigidBody = World.GetStash<Reference<Rigidbody>>();

            _player = World.GetStash<Player>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
              
                ref var player = ref _player.Get(entity);

                if (player.Jumped) {
                    player.Jumped = false;
                    continue;
                }

                ref var isGrounded = ref _isGrounded.Get(entity);
                ref var jump = ref _jumpPressed.Get(entity);
                
                if (!jump.Value || !isGrounded.Value)
                    continue;

                player.Jumped = true;

                ref var playerConfig = ref _playerConfig.Get(entity);
                ref Reference<Rigidbody> rigidBody = ref _rigidBody.Get(entity);

                var velocity = rigidBody.Value.velocity;
                velocity.y = 0;
                rigidBody.Value.velocity = velocity;

                rigidBody.Value.AddForce(Vector3.up * playerConfig.JumpHeight, ForceMode.Impulse);
            }
        }
    }
}