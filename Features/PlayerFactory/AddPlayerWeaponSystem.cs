using EcsMagic.PlayerComponenets;
using Game.Components;
using Game.Data;
using Game.WeaponComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddPlayerWeaponSystem : ISystem
    {
        private readonly GameConfig _gameConfig;
        private Stash<CreateWeaponForPlayer> _createWeaponForPlayer;
        private Filter _filter;
        private Stash<Player> _player;
        private Filter _playerFilter;
        private Stash<PlayerTag> _playerTag;
        private Stash<Request_CreateWeapon> _requestCreateWeapon;

        public AddPlayerWeaponSystem(DataLocator dataLocator)
        {
            _gameConfig = dataLocator.Get<GameConfig>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_PlayerCreated>();
            _playerFilter = Entities.With<Player>();

            _requestCreateWeapon = World.GetStash<Request_CreateWeapon>();
            _player = World.GetStash<Player>();

            _playerTag = World.GetStash<PlayerTag>();
            _createWeaponForPlayer = World.GetStash<CreateWeaponForPlayer>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var player = ref _player.Get(_playerFilter.First());

                // if (_gameConfig.WeaponPrefab == null)
                //     continue;

                foreach (var playerWeapon in _gameConfig.PlayerWeapons) {
                    var weaponInstance = Object.Instantiate(playerWeapon.WeaponPrefab, player.Instance.WeaponParent)
                        .GetComponent<WeaponProvider>();

                    var requestEntity = _requestCreateWeapon.CreateRequest(new Request_CreateWeapon {
                        Instance = weaponInstance,
                        WeaponUserEntity = player.Instance.Entity,
                    });

                    _createWeaponForPlayer.Add(requestEntity, new CreateWeaponForPlayer {Key = playerWeapon.Key,});
                    _playerTag.Add(requestEntity);
                }
            }
        }
    }
}