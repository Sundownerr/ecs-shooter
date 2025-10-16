using Game.Components;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LoadLevelSceneSystem : ISystem
    {
        private readonly LevelEntry[] _levelsScenes;
        private readonly RuntimeData _runtimeData;
        private Stash<Event_StartedLoadingScene> _eventStartedLoadingScene;
        private Filter _filter;
        private Stash<LoadingSceneHandle> _loadingSceneHandle;
        private Stash<Request_CreateLevel> _requestCreateLevel;
        private readonly AddressablesService _addressablesService;

        public LoadLevelSceneSystem(DataLocator dataLocator,ServiceLocator serviceLocator)
        {
            _levelsScenes = dataLocator.Get<GameConfig>().Levels;
            _runtimeData = dataLocator.Get<RuntimeData>();
            _addressablesService = serviceLocator.Get<AddressablesService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Request_CreateLevel>();
            _loadingSceneHandle = World.GetStash<LoadingSceneHandle>();
            _requestCreateLevel = World.GetStash<Request_CreateLevel>();
            _eventStartedLoadingScene = World.GetStash<Event_StartedLoadingScene>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var request = ref _requestCreateLevel.Get(entity);

                AsyncOperationHandle<SceneInstance> handle = _addressablesService.LoadSceneAsync(
                    _levelsScenes[request.Index].Scene);
                _loadingSceneHandle.Set(World.CreateEntity(), new LoadingSceneHandle {Handle = handle,});
                _eventStartedLoadingScene.CreateEvent();

                _runtimeData.CurrentLevelIndex = request.Index;
                _runtimeData.CurrentLevelHandle = handle;

                entity.CompleteRequest();
            }
        }
    }
}