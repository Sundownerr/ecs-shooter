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
    public sealed class UpdatePlayerMoveDirectionSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_MoveDirection> _playerInputMoveDirectionStash;
        private Stash<MoveDirection> _moveDirectionStash;
        private Stash<Player> _playerStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Player, MoveDirection, PlayerInput_MoveDirection>();
            _playerInputMoveDirectionStash = World.GetStash<PlayerInput_MoveDirection>();
            _moveDirectionStash = World.GetStash<MoveDirection>();
            _playerStash = World.GetStash<Player>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var input = ref _playerInputMoveDirectionStash.Get(entity);
                ref var moveDirection = ref _moveDirectionStash.Get(entity);
                ref var player = ref _playerStash.Get(entity);

                moveDirection.Value.x = 0;
                moveDirection.Value.z = 0;

                if (input.Value.x != 0 || input.Value.y != 0)
                {
                    var cameraRight = player.Instance.CinemachineCameraTarget.right;
                    var forward = new Vector3(-cameraRight.z, 0, cameraRight.x);

                    moveDirection.Value = forward * input.Value.y + cameraRight * input.Value.x;
                    moveDirection.Value.Normalize();
                    moveDirection.Value.y = 0;
                }
            }
        }
    }
}
