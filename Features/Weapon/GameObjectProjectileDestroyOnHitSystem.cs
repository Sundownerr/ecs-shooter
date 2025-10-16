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
    public sealed class GameObjectProjectileDestroyOnHitSystem : ISystem
    {
        private Filter _filter;
        private Stash<WeaponProjectile> _weaponProjectile;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<WeaponProjectile, WillBeDestroyed>();
            _weaponProjectile = World.GetStash<WeaponProjectile>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var projectile = ref _weaponProjectile.Get(entity);
                Object.Destroy(projectile.Projectile.gameObject);
                World.RemoveEntity(entity);
            }
        }
    }
}
