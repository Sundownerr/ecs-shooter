using System.Collections.Generic;
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
    public sealed class WeaponFactorySystem : ISystem
    {
        private Stash<AbilitiesList> _abilities;
        private Filter _filter;
        private Stash<GameObjectShooting> _gameObjectShooting;
        private Stash<HitscanShooting> _hitscanShooting;
        private Stash<HybridParticleGameObjectShooting> _hybridParticleGameObjectShooting;
        private Stash<IncreasingTimer> _increasingTimer;
        private Stash<PlayerInput_PrimaryAttack_IsPressed> _playerInput_PrimaryAttack_IsPressed;
        private Stash<PlayerTag> _playerTag;

        private Stash<Request_CreateWeapon> _requestCreateWeapon;
        private Stash<Weapon> _weapon;
        private Stash<WeaponsList> _weaponsList;
        private Stash<CreateWeaponForPlayer> _createWeaponForPlayer;
        private Stash<Player> _player;
        private Filter _playerFilter;
        private Stash<WeaponAmmo> _weaponAmmo;
        private Stash<Reloading> _reloading;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Request_CreateWeapon>();

            _requestCreateWeapon = World.GetStash<Request_CreateWeapon>();
            _weapon = World.GetStash<Weapon>();
            _increasingTimer = World.GetStash<IncreasingTimer>();
            _playerInput_PrimaryAttack_IsPressed = World.GetStash<PlayerInput_PrimaryAttack_IsPressed>();
            _playerTag = World.GetStash<PlayerTag>();
            _weaponsList = World.GetStash<WeaponsList>();
            _abilities = World.GetStash<AbilitiesList>();
            _hitscanShooting = World.GetStash<HitscanShooting>();
            _gameObjectShooting = World.GetStash<GameObjectShooting>();
            _hybridParticleGameObjectShooting = World.GetStash<HybridParticleGameObjectShooting>();
            _weaponAmmo = World.GetStash<WeaponAmmo>();
            _reloading = World.GetStash<Reloading>();

            _createWeaponForPlayer = World.GetStash<CreateWeaponForPlayer>();
            _player = World.GetStash<Player>();
            _playerFilter = Entities.With<Player>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var request = ref _requestCreateWeapon.Get(entity);

                var weaponUserEntity = request.WeaponUserEntity;
                var weaponInstance = request.Instance;

                var weaponEntity = weaponInstance.Initialize(World);

                _weapon.Set(weaponEntity, new Weapon { Instance = weaponInstance, User = weaponUserEntity, CanShoot = true }); // Initialize CanShoot
                _increasingTimer.Set(weaponEntity, new IncreasingTimer { Duration = weaponInstance.ShootInterval, });

                AddWeaponToUserWeaponList(weaponUserEntity, weaponEntity);
                AddWeaponAbilities(weaponEntity, weaponInstance, weaponUserEntity);
                AddShootingComponent(weaponInstance, weaponEntity);

                if (_createWeaponForPlayer.Has(entity))
                {
                    _playerInput_PrimaryAttack_IsPressed.Add(weaponEntity);
                    _playerTag.Add(weaponEntity);

                    ref var createWeaponForPlayer = ref _createWeaponForPlayer.Get(entity);
                    ref var player = ref _player.Get(_playerFilter.First());
                    player.WeaponsOnKeys.Add(createWeaponForPlayer.Key, weaponInstance);

                    weaponInstance.gameObject.SetActive(false);
                }

                if (weaponInstance.ReloadTime > 0) {
                    _reloading.Add(weaponEntity);
                    _weaponAmmo.Set(weaponEntity, new WeaponAmmo() {
                        MaxClipAmmo = weaponInstance.MagazineSize,
                        CurrentClipAmmo = weaponInstance.MagazineSize,
                        ReloadTimeSeconds = weaponInstance.ReloadTime,
                    });
                }

                entity.CompleteRequest();
            }
        }

        private void AddShootingComponent(WeaponProvider weaponInstance, Entity weaponEntity)
        {
            switch (weaponInstance.ShootingComponent)
            {
                case HitscanShootingComponent hitscanShootingComponent:
                    _hitscanShooting.Set(weaponEntity, new HitscanShooting
                    {
                        MaxDistance = hitscanShootingComponent.MaxDistance,
                        RayRadius = hitscanShootingComponent.RayRadius,
                        PlaceAtRayEnd = hitscanShootingComponent.PlaceAtRayEnd,
                    });
                    break;

                case GameObjectShootingComponent gameObjectShootingComponent:
                    _gameObjectShooting.Set(weaponEntity, new GameObjectShooting
                    {
                        Config = gameObjectShootingComponent,
                    });
                    break;

                case HybridParticleGameObjectShootingComponent hybridParticleGameObjectShootingComponent:
                    _hybridParticleGameObjectShooting.Set(weaponEntity, new HybridParticleGameObjectShooting
                    {
                        ParticleSystem = hybridParticleGameObjectShootingComponent.ParticleSystem,
                        Prefab = hybridParticleGameObjectShootingComponent.ProjectilePrefab,

                        DeadParticle = new List<uint>(),
                        Projectiles = new Dictionary<uint, Transform>(),
                        ParticleAlive = new Dictionary<uint, bool>(),
                    });
                    break;
            }

            weaponInstance.ShootingComponent.Initialize(weaponInstance, World);
        }

        private void AddWeaponAbilities(Entity weaponEntity, WeaponProvider weaponInstance, Entity weaponUserEntity)
        {
            var weaponAbilitiesList = new List<Entity>();

            foreach (var abilityProvider in weaponInstance.OnShootAbility)
                weaponAbilitiesList.Add(abilityProvider.Create(weaponUserEntity, World));

            foreach (var abilityProvider in weaponInstance.OnHitAbility)
                weaponAbilitiesList.Add(abilityProvider.Create(weaponUserEntity, World));

            foreach (var abilityProvider in weaponInstance.OnTriggerPulledAbility)
                weaponAbilitiesList.Add(abilityProvider.Create(weaponUserEntity, World));

            foreach (var abilityProvider in weaponInstance.OnTriggerReleasedAbility)
                weaponAbilitiesList.Add(abilityProvider.Create(weaponUserEntity, World));

            foreach (var abilityProvider in weaponInstance.OnWeaponDraw)
                weaponAbilitiesList.Add(abilityProvider.Create(weaponUserEntity, World));

            foreach (var abilityProvider in weaponInstance.OnWeaponHide)
                weaponAbilitiesList.Add(abilityProvider.Create(weaponUserEntity, World));
            
            foreach (var abilityProvider in weaponInstance.OnReloadStart)
                weaponAbilitiesList.Add(abilityProvider.Create(weaponUserEntity, World));

            _abilities.Set(weaponEntity, new AbilitiesList { List = weaponAbilitiesList, });
        }

        private void AddWeaponToUserWeaponList(Entity weaponUserEntity, Entity weaponEntity)
        {
            List<Entity> weaponsList;

            if (_weaponsList.Has(weaponUserEntity))
            {
                ref var weapons = ref _weaponsList.Get(weaponUserEntity);
                weaponsList = weapons.List;
            }
            else
            {
                weaponsList = new List<Entity>();
                _weaponsList.Set(weaponUserEntity, new WeaponsList { List = weaponsList, });
            }

            weaponsList.Add(weaponEntity);
        }
    }
}