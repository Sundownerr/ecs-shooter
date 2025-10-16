using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HitscanPlaceAtRayEndSystem : ISystem
    {
        private Filter _filter;
        private Stash<HitscanShooting> _hitscanShooting;
        private Stash<Weapon> _weapon;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Weapon, HitscanShooting>();
            _hitscanShooting = World.GetStash<HitscanShooting>();
            _weapon = World.GetStash<Weapon>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var hitscanShooting = ref _hitscanShooting.Get(entity);
                ref var weapon = ref _weapon.Get(entity);

                if (!Physics.SphereCast(hitscanShooting.RayOrigin,
                    hitscanShooting.RayRadius,
                    hitscanShooting.RayDirection, out var hit,
                    hitscanShooting.MaxDistance, weapon.Instance.HitLayerMask)) {
                    hitscanShooting.PlaceAtRayEnd.position = hitscanShooting.RayOrigin +
                                                             hitscanShooting.RayDirection * hitscanShooting.MaxDistance;

                    // Debug.Log($"Hitscan Endpoint: {hitscanShooting.PlaceAtRayEnd.position}");
                    continue;
                }

                Debug.DrawLine(hitscanShooting.RayOrigin, hit.point, Color.red);
                hitscanShooting.PlaceAtRayEnd.position = hit.point;
                // Debug.Log($"Hitscan Endpoint: {hitscanShooting.PlaceAtRayEnd.position}");
            }
        }
    }
}