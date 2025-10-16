using EcsMagic.PlayerComponenets;
using Game.Data;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerWeaponChangeSystem : ISystem
    {
        private Stash<ActiveWeapon> _activeWeapon;
        private Stash<CanChangeWeapons> _canChangeWeapons;
        private Stash<Player> _player;
        private Filter _playerFilter;
        private Stash<PlayerInput_Weapon1_WasPressed> _weapon1Pressed;
        private Filter _weapon1PressedFilter;
        private Stash<PlayerInput_Weapon2_WasPressed> _weapon2Pressed;
        private Filter _weapon2PressedFilter;
        private Stash<PlayerInput_Weapon3_WasPressed> _weapon3Pressed;
        private Filter _weapon3PressedFilter;
        private Stash<PlayerInput_Weapon4_WasPressed> _weapon4Pressed;
        private Filter _weapon4PressedFilter;
        private Stash<WeaponChangeTimer> _weaponChangeTimer;
        private Filter _weaponChangeTimerFilter;

        public void Dispose() { }

        public void OnAwake()
        {
            _playerFilter = Entities.With<Player, CanChangeWeapons>();
            _weapon1PressedFilter = Entities.With<Player, PlayerInput_Weapon1_WasPressed>();
            _weapon2PressedFilter = Entities.With<Player, PlayerInput_Weapon2_WasPressed>();
            _weapon3PressedFilter = Entities.With<Player, PlayerInput_Weapon3_WasPressed>();
            _weapon4PressedFilter = Entities.With<Player, PlayerInput_Weapon4_WasPressed>();
            _weaponChangeTimerFilter = World.Filter.With<WeaponChangeTimer>().Without<CanChangeWeapons>().Build();

            _player = World.GetStash<Player>();
            _weapon1Pressed = World.GetStash<PlayerInput_Weapon1_WasPressed>();
            _weapon2Pressed = World.GetStash<PlayerInput_Weapon2_WasPressed>();
            _weapon3Pressed = World.GetStash<PlayerInput_Weapon3_WasPressed>();
            _weapon4Pressed = World.GetStash<PlayerInput_Weapon4_WasPressed>();
            _canChangeWeapons = World.GetStash<CanChangeWeapons>();
            _weaponChangeTimer = World.GetStash<WeaponChangeTimer>();
            _activeWeapon = World.GetStash<ActiveWeapon>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _weaponChangeTimerFilter) {
                ref var weaponChangeTimer = ref _weaponChangeTimer.Get(entity);

                weaponChangeTimer.RemainingTime -= deltaTime;

                if (weaponChangeTimer.RemainingTime <= 0)
                    _canChangeWeapons.Add(entity);
            }

            foreach (var playerEntity in _playerFilter) {
                ref var player = ref _player.Get(playerEntity);

                if (player.ActiveWeapon == null) {
                    ChangeWeapon(ref player, playerEntity, PlayerInputKey.Weapon1);
                    continue;
                }

                foreach (var entity1 in _weapon1PressedFilter) {
                    ref var weapon1Pressed = ref _weapon1Pressed.Get(entity1);

                    if (weapon1Pressed.Value)
                        ChangeWeapon(ref player, playerEntity, PlayerInputKey.Weapon1);
                }

                foreach (var entity2 in _weapon2PressedFilter) {
                    ref var weapon2Pressed = ref _weapon2Pressed.Get(entity2);

                    if (weapon2Pressed.Value)
                        ChangeWeapon(ref player, playerEntity, PlayerInputKey.Weapon2);
                }

                foreach (var entity3 in _weapon3PressedFilter) {
                    ref var weapon3Pressed = ref _weapon3Pressed.Get(entity3);

                    if (weapon3Pressed.Value)
                        ChangeWeapon(ref player, playerEntity, PlayerInputKey.Weapon3);
                }

                foreach (var entity4 in _weapon4PressedFilter) {
                    ref var weapon4Pressed = ref _weapon4Pressed.Get(entity4);

                    if (weapon4Pressed.Value)
                        ChangeWeapon(ref player, playerEntity, PlayerInputKey.Weapon4);
                }
            }
        }

        private void ChangeWeapon(ref Player player, Entity playerEntity, PlayerInputKey key)
        {
            if (player.ActiveWeapon == player.WeaponsOnKeys[key])
                return;

            var weaponChangeTime = player.WeaponsOnKeys[key].DrawDuration;

            if (player.ActiveWeapon != null) {
                weaponChangeTime += player.ActiveWeapon.HideDuration;
                _activeWeapon.Remove(player.ActiveWeapon.Entity);
                player.ActiveWeapon.HideWeapon();
            }

            player.ActiveWeapon = player.WeaponsOnKeys[key];
            player.ActiveWeapon.gameObject.SetActive(true);
            player.ActiveWeapon.DrawWeapon();
            _activeWeapon.Add(player.ActiveWeapon.Entity);

            _canChangeWeapons.Remove(playerEntity);

            ref var weaponChangeTimer = ref _weaponChangeTimer.Get(playerEntity);
            weaponChangeTimer.RemainingTime = weaponChangeTime;
        }
    }
}