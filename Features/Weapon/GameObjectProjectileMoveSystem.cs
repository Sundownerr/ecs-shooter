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
    public sealed class GameObjectProjectileMoveSystem : ISystem
    {
        private Filter _filter;
        private Stash<WeaponProjectile> _weaponProjectile;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<WeaponProjectile>();
            _weaponProjectile = World.GetStash<WeaponProjectile>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var projectile = ref _weaponProjectile.Get(entity);

                var direction = projectile.InitialDirection;
                direction.x += projectile.X.Evaluate(projectile.TravelTime);
                direction.y += projectile.Y.Evaluate(projectile.TravelTime);
                direction.z += projectile.Z.Evaluate(projectile.TravelTime);

                projectile.TravelTime += deltaTime;
                projectile.Projectile.localRotation = Quaternion.LookRotation(direction);
                projectile.Projectile.localPosition += projectile.Projectile.forward *
                                                       projectile.Speed.Evaluate(projectile.TravelTime) * deltaTime;
            }
        }
    }
}
