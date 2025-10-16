using Game.Components;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WeaponAmmoSystem : ISystem
    {
        private Filter _firedWeaponFilter;
        private Stash<Reloading> _reloadingStash;
        private Stash<WeaponAmmo> _weaponAmmoStash;
        private Stash<Weapon> _weaponStash;

        public World World { get; set; }

        public void Dispose() { }

        public void OnAwake()
        {
            _firedWeaponFilter = World.Filter
                .With<WeaponAmmo>()
                .With<WeaponTriggerPulled>()
                .With<TimerCompleted>()
                .Without<Reloading>()
                .Build();

            _weaponStash = World.GetStash<Weapon>();
            _weaponAmmoStash = World.GetStash<WeaponAmmo>();
            _reloadingStash = World.GetStash<Reloading>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _firedWeaponFilter) {
                ref var weapon = ref _weaponStash.Get(entity);

                if (weapon.CanShoot)
                    continue;

                ref var ammo = ref _weaponAmmoStash.Get(entity);
                ammo.CurrentClipAmmo--;

                if (ammo.CurrentClipAmmo > 0)
                    continue;

                if (!_reloadingStash.Has(entity) && ammo.ReloadTimeSeconds > 0) {
                    ref var reloading = ref _reloadingStash.Add(entity);
                    reloading.RemainingTime = ammo.ReloadTimeSeconds;

                    foreach (var abilityProvider in weapon.Instance.OnReloadStart)
                        abilityProvider.Activate();
                }
            }
        }
    }
}