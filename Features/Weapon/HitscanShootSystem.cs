using Game.Components;
using Game.Data;
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
    public sealed class HitscanShootSystem : ISystem
    {
        private readonly DamageInstanceService _damageInstanceService;
        private Stash<Event_WeaponHit> _eventRegisterWeaponHit;
        private Filter _filter;
        private Stash<HitscanShooting> _hitscanShooting;
        private Stash<Weapon> _weapon;
       
        public HitscanShootSystem(ServiceLocator serviceLocator)
        {
            _damageInstanceService = serviceLocator.Get<DamageInstanceService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<HitscanShooting, TimerCompleted, WeaponTriggerPulled>();
            _weapon = World.GetStash<Weapon>();
            _hitscanShooting = World.GetStash<HitscanShooting>();
            _eventRegisterWeaponHit = World.GetStash<Event_WeaponHit>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var weapon = ref _weapon.Get(entity);
                ref var hitscanShooting = ref _hitscanShooting.Get(entity);

                // Debug.DrawRay(rayOrigin, rayDirection* 10000, Color.red, 0.1f);

                if (!Physics.SphereCast(hitscanShooting.RayOrigin,
                    hitscanShooting.RayRadius,
                    hitscanShooting.RayDirection, out var hit,
                    hitscanShooting.MaxDistance, weapon.Instance.HitLayerMask)) {
                    _eventRegisterWeaponHit.CreateEvent(new Event_WeaponHit {
                        Position = hitscanShooting.RayOrigin +
                                   hitscanShooting.RayDirection * hitscanShooting.MaxDistance,
                        WeaponInstance = weapon.Instance,
                    });
                    continue;
                }

                _eventRegisterWeaponHit.CreateEvent(new Event_WeaponHit {
                    Position = hit.point,
                    WeaponInstance = weapon.Instance,
                });

                if (!hit.transform.TryGetComponent<EntityProvider>(out var entityProvider))
                    continue;

                _damageInstanceService.AddDamageInstance(weapon.User, entityProvider.Entity, weapon.Instance.Damage);
            }
        }
    }
}