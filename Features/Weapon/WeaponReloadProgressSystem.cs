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
    public sealed class WeaponReloadProgressSystem : ISystem
    {
        private Filter _reloadingFilter;
        private Stash<Reloading> _reloadingStash;
        private Stash<TimerCompleted> _timerCompletedStash; 
        private Stash<WeaponAmmo> _weaponAmmoStash;
        private Stash<Weapon> _weaponStash;

        public World World { get; set; }

        public void Dispose() { }

        public void OnAwake()
        {
            _reloadingFilter = World.Filter
                .With<Weapon>()
                .With<WeaponAmmo>()
                .With<Reloading>()
                .Build();

            _weaponStash = World.GetStash<Weapon>();
            _weaponAmmoStash = World.GetStash<WeaponAmmo>();
            _reloadingStash = World.GetStash<Reloading>();
            _timerCompletedStash = World.GetStash<TimerCompleted>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _reloadingFilter) {
                ref var reloading = ref _reloadingStash.Get(entity);
                reloading.RemainingTime -= deltaTime;

                if (reloading.RemainingTime > 0f)
                    continue;

                ref var ammo = ref _weaponAmmoStash.Get(entity);
                ammo.CurrentClipAmmo = ammo.MaxClipAmmo;

                _reloadingStash.Remove(entity);

                ref var weapon = ref _weaponStash.Get(entity);

                if (_timerCompletedStash.Has(entity))
                    weapon.CanShoot = true;

                // Debug.Log(
                //     $"Reload complete. Ammo: {ammo.CurrentClipAmmo}/{ammo.MaxClipAmmo}. CanShoot: {weapon.CanShoot}");
            }
        }
    }
}