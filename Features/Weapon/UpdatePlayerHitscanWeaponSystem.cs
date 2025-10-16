using EcsMagic.PlayerComponenets;
using Game.Components;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdatePlayerHitscanWeaponSystem : ISystem
    {
        private Filter _filter;
        private readonly RuntimeData _runtimeData;
        private Stash<HitscanShooting> _hitscanShooting;
        private Stash<Weapon> _weapon;
        private Stash<Player> _player;

        public UpdatePlayerHitscanWeaponSystem(DataLocator dataLocator)
        {
            _runtimeData = dataLocator.Get<RuntimeData>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter .With<HitscanShooting, PlayerTag>().Without<WillBeDestroyed>().Build();
            _hitscanShooting = World.GetStash<HitscanShooting>();
            _weapon = World.GetStash<Weapon>();
            _player = World.GetStash<Player>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                // if (_runtimeData.Camera == null)
                // {
                //     Debug.LogWarning("Camera is null");
                //     continue;
                // }
                ref var weapon = ref _weapon.Get(entity);
                ref var player = ref _player.Get(weapon.User);
                ref var hitscanShooting = ref _hitscanShooting.Get(entity);

                hitscanShooting.RayOrigin = player.Instance.CinemachineCameraTarget.position;
                hitscanShooting.RayDirection = player.Instance.CinemachineCameraTarget.forward;
            }
        }
    }
}
