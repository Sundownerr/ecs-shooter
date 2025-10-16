using EcsMagic.PlayerComponenets;
using Game.Features;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateWeaponUISystem : ISystem
    {
        private readonly PlayerUIProvider _playerUI;
        private Filter _playerFilter;
        private Stash<Player> _playerStash;
        private Stash<Reloading> _reloadingStash;
        private Stash<WeaponAmmo> _weaponAmmo;

        public UpdateWeaponUISystem(ServiceLocator serviceLocator)
        {
            _playerUI = serviceLocator.Get<UiService>().PlayerUi();
        }

        public World World { get; set; }

        public void Dispose() { }

        public void OnAwake()
        {
            _playerFilter = World.Filter.With<Player>().Build();
            _playerStash = World.GetStash<Player>();
            _weaponAmmo = World.GetStash<WeaponAmmo>();
            _reloadingStash = World.GetStash<Reloading>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_playerFilter.IsEmpty())
                return;

            ref var player = ref _playerStash.Get(_playerFilter.First());
            var activeWeaponProvider = player.ActiveWeapon;

            if (activeWeaponProvider != null) {
                var activeWeaponEntity = activeWeaponProvider.Entity;

                if (_weaponAmmo.Has(activeWeaponEntity)) {
                    ref var ammo = ref _weaponAmmo.Get(activeWeaponEntity);
                    _playerUI.AmmoText.text = $"{ammo.CurrentClipAmmo} / {ammo.MaxClipAmmo}";
                    _playerUI.AmmoText.gameObject.SetActive(true);

                    var isReloading = _reloadingStash.Has(activeWeaponEntity);
                    _playerUI.ReloadingIndicator.SetActive(isReloading);
                }
                else {
                    _playerUI.AmmoText.gameObject.SetActive(false);
                    _playerUI.ReloadingIndicator.SetActive(false);
                }
            }
            else {
                _playerUI.AmmoText.gameObject.SetActive(false);
                _playerUI.ReloadingIndicator.SetActive(false);
            }
        }
    }
}