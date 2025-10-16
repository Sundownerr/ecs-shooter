using EcsMagic.PlayerComponenets;
using Game.Components;
using Game.Data;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerLevelChangeSystem : ISystem
    {
        private readonly StaticData _staticData;
        private Filter _filter;
        private Stash<HybridParticleGameObjectShooting> _hybridParticleGameObjectShooting;
        private Stash<Player> _player;

        private Stash<Trigger_ReactOn_LevelChanged> _triggeredReactOnWorldChanged;
        private Stash<WeaponsList> _weaponsList;

        public PlayerLevelChangeSystem(DataLocator dataLocator)
        {
            _staticData = dataLocator.Get<StaticData>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Player, ReactOn_LevelChanged, Trigger_ReactOn_LevelChanged>();
            _triggeredReactOnWorldChanged = World.GetStash<Trigger_ReactOn_LevelChanged>();
            _player = World.GetStash<Player>();
            _weaponsList = World.GetStash<WeaponsList>();
            _hybridParticleGameObjectShooting = World.GetStash<HybridParticleGameObjectShooting>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                _triggeredReactOnWorldChanged.Remove(entity);

                ref var player = ref _player.Get(entity);

                player.Instance.transform.SetParent(_staticData.DdolWrapper.transform);

                if (!_weaponsList.Has(entity))
                    continue;

                ref var playerWeapons = ref _weaponsList.Get(entity);

                foreach (var weaponEntity in playerWeapons.List) {
                    if (_hybridParticleGameObjectShooting.Has(weaponEntity)) {
                        ref var shooting = ref _hybridParticleGameObjectShooting.Get(weaponEntity);

                        shooting.ParticleSystem.Clear(true);
                        shooting.ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                        foreach (var (_, projectile) in shooting.Projectiles)
                            Object.Destroy(projectile.gameObject);

                        shooting.Projectiles.Clear();
                        shooting.ParticleAlive.Clear();
                        shooting.DeadParticle.Clear();
                    }
                }
            }
        }
    }
}