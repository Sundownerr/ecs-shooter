using EcsMagic.PlayerComponenets;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlacePlayerOnNewLevelSystem : ISystem
    {
        private Filter _currentLevelFilter;
        private Filter _filter;
        private Filter _playerFilter;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_CompletedLoadingScene>();
            _currentLevelFilter = Entities.With<CurrentLevel>();
            _playerFilter = Entities.With<Player>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                if (_playerFilter.IsEmpty())
                    continue;

                ref var player = ref _playerFilter.First().GetComponent<Player>();
                ref var currentLevel = ref _currentLevelFilter.First().GetComponent<Level>();

                player.Instance.transform.SetParent(currentLevel.Instance.transform);
            }
        }
    }
}