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
    public sealed class GameObjectWeaponProjectileFactorySystem : ISystem
    {
        private Filter _filter;
        private Stash<GameObjectShooting> _gameObjectShootingStash;
        private Stash<Weapon> _weaponStash;
        private Stash<WeaponProjectile> _weaponProjectileStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<GameObjectShooting, WeaponTriggerPulled, TimerCompleted>();
            _gameObjectShootingStash = World.GetStash<GameObjectShooting>();
            _weaponStash = World.GetStash<Weapon>();
            _weaponProjectileStash = World.GetStash<WeaponProjectile>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var gameObjectShooting = ref _gameObjectShootingStash.Get(entity);

                var projectileInstance = Object.Instantiate(gameObjectShooting.Config.ProjectilePrefab,
                    gameObjectShooting.Config.ShootPoint.position,
                    gameObjectShooting.Config.ShootPoint.rotation);

                var projectileEntity = World.CreateEntity();
                
                ref var projectile = ref _weaponProjectileStash.Add(projectileEntity);
                projectile.Projectile = projectileInstance.transform;

                projectile.X = gameObjectShooting.Config.X;
                projectile.Y = gameObjectShooting.Config.Y;
                projectile.Z = gameObjectShooting.Config.Z;
                projectile.Speed = gameObjectShooting.Config.Speed;
                projectile.InitialDirection = projectileInstance.transform.forward;
                projectile.Radius = gameObjectShooting.Config.Radius;

                // Debug.Log(projectile.InitialDirection);

                ref var weapon = ref _weaponStash.Get(entity);
                projectile.WeaponInstance = weapon.Instance;
            }
        }
    }
}
