using Game.Components;
using Game.Features;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUiUpdateYellowCubesSystem : ISystem
    {
        private readonly UiService _uiService;
        private Filter _filter;
        private Filter _playerResourcesFilter;

        public PlayerUiUpdateYellowCubesSystem(ServiceLocator serviceLocator)
        {
            _uiService = serviceLocator.Get<UiService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Request_UpdatePlayerResourcesUi>();
            _playerResourcesFilter = Entities.With<PlayerResources>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var playerResources = ref _playerResourcesFilter.First().GetComponent<PlayerResources>();

                _uiService.UpdateYellowCubesCount(playerResources.YellowCubes);

                entity.CompleteRequest();
            }
        }
    }
}