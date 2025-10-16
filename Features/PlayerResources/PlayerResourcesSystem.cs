using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerResourcesSystem : ISystem
    {
        private Stash<Request_ChangeYellowCubes> _requestChangeYellowCubes;
        private Filter _filter;
        private Stash<Level> _level;
        private Filter _levelFilter;
        private Stash<PlayerResources> _playerResources;
        private Filter _playerResourcesFilter;
        private Stash<Request_UpdatePlayerResourcesUi> _requestUpdatePlayerResourcesUi;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Request_ChangeYellowCubes>();
            _levelFilter = Entities.With<Level>();
            _playerResourcesFilter = Entities.With<PlayerResources>();

            _playerResources = World.GetStash<PlayerResources>();
            _requestChangeYellowCubes = World.GetStash<Request_ChangeYellowCubes>();
            _requestUpdatePlayerResourcesUi = World.GetStash<Request_UpdatePlayerResourcesUi>();
            _level = World.GetStash<Level>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var yellowCubes = ref _requestChangeYellowCubes.Get(entity);
                ref var playerResources = ref _playerResources.Get(_playerResourcesFilter.First());

                playerResources.YellowCubes += yellowCubes.Amount;

                if (yellowCubes.AsGatheredOnLevel) {
                    ref var level = ref _level.Get(_levelFilter.First());
                    level.PlayerGatheredYellowCubes += yellowCubes.Amount;
                }

                _requestUpdatePlayerResourcesUi.CreateRequest();
                entity.CompleteRequest();
            }
        }
    }
}