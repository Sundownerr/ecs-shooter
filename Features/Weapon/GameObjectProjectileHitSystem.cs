using Game.Components;
using Game.Providers;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GameObjectProjectileHitSystem : ISystem
    {
        private readonly Collider[] _colliders = new Collider[100];
        private readonly DamageInstanceService _damageInstanceService;

        private Stash<Event_WeaponHit> _eventRegisterWeaponHit;
        private Filter _filter;
        private Stash<Weapon> _weapon;
        private Stash<WeaponProjectile> _weaponProjectile;
        private Stash<WillBeDestroyed> _willBeDestroyed;

        public GameObjectProjectileHitSystem(DataLocator dataLocator, ServiceLocator serviceLocator)
        {
            _damageInstanceService = serviceLocator.Get<DamageInstanceService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<WeaponProjectile>();
            _willBeDestroyed = World.GetStash<WillBeDestroyed>();
            _weaponProjectile = World.GetStash<WeaponProjectile>();
            _weapon = World.GetStash<Weapon>();
            _eventRegisterWeaponHit = World.GetStash<Event_WeaponHit>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var projectile = ref _weaponProjectile.Get(entity);

                var position = projectile.Projectile.position;
                ref var weapon = ref _weapon.Get(projectile.WeaponInstance.Entity);

                if (!Physics.CheckSphere(position, projectile.Radius, weapon.Instance.HitLayerMask))
                    continue;

                _eventRegisterWeaponHit.CreateEvent(new Event_WeaponHit {
                    Position = position,
                    WeaponInstance = projectile.WeaponInstance,
                });

                _willBeDestroyed.Add(entity);

                var hits = Physics.OverlapSphereNonAlloc(position, projectile.Radius, _colliders,
                    weapon.Instance.HitLayerMask);

                if (hits <= 0)
                    continue;

                for(var i = 0; i < hits; i++) {
                    if (!_colliders[i].TryGetComponent<EntityProvider>(out var entityProvider))
                        continue;

                    _damageInstanceService.AddDamageInstance(weapon.User, entityProvider.Entity,
                        projectile.WeaponInstance.Damage);
                }
            }
        }
    }
}