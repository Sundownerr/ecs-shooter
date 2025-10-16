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
    public sealed class PlayerLookDirectionSystem : ISystem
    {
        private Filter _filter;
        private Stash<PlayerInput_LookDirection> _playerInputLookDirectionStash;
        private Stash<LookDirection> _lookDirectionStash;
        private Stash<Player> _playerStash;
        private Stash<PlayerConfig> _playerConfigStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Player, LookDirection, PlayerInput_LookDirection>();
            _playerInputLookDirectionStash = World.GetStash<PlayerInput_LookDirection>();
            _lookDirectionStash = World.GetStash<LookDirection>();
            _playerStash = World.GetStash<Player>();
            _playerConfigStash = World.GetStash<PlayerConfig>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var input = ref _playerInputLookDirectionStash.Get(entity);
                ref var lookDirection = ref _lookDirectionStash.Get(entity);
                ref var player = ref _playerStash.Get(entity);
                ref var playerConfig = ref _playerConfigStash.Get(entity);

                if (math.lengthsq(input.Value) < 0.01f)
                    continue;

                //Don't multiply mouse input by Time.deltaTime, on gamepad you should multiply by deltatime
                var deltaTimeMultiplier = 1.0f;

                lookDirection.Value.x += input.Value.x * playerConfig.LookRotationSpeed * deltaTimeMultiplier;
                lookDirection.Value.y += input.Value.y * playerConfig.LookRotationSpeed * deltaTimeMultiplier;

                lookDirection.Value.y = ClampAngle(lookDirection.Value.y, -89, 89);

                player.Instance.CinemachineCameraTarget.localRotation = Quaternion.Euler(
                    lookDirection.Value.y,
                    lookDirection.Value.x, 0.0f);

                float ClampAngle(float lfAngle, float lfMin, float lfMax)
                {
                    if (lfAngle < -360f)
                        lfAngle += 360f;

                    if (lfAngle > 360f)
                        lfAngle -= 360f;

                    return Mathf.Clamp(lfAngle, lfMin, lfMax);
                }
            }
        }
    }
}
