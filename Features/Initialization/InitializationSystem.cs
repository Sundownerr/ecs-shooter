using Game.Components;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitializationSystem : IInitializer
    {
        private readonly GameConfig _gameConfig;
        private readonly DamageInstanceService _damageInstanceService;

        public InitializationSystem(DataLocator dataLocator, ServiceLocator serviceLocator)
        {
            _gameConfig = dataLocator.Get<GameConfig>();
            _damageInstanceService = serviceLocator.Get<DamageInstanceService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _damageInstanceService.Initialize(World);
            
            Stash<PlayerResources> playerResourcesStash = World.GetStash<PlayerResources>();
            Stash<Request_CreateLevel> requestCreateLevel = World.GetStash<Request_CreateLevel>();

            playerResourcesStash.Set(World.CreateEntity(), new PlayerResources {YellowCubes = 0,});

            if (!_gameConfig.StartFromMenu)
                requestCreateLevel.CreateRequest(new Request_CreateLevel {Index = 0,});
        }

        public World World { get; set; }
    }
}