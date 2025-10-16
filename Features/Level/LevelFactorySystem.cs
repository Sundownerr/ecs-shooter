using System.Collections.Generic;
using System.Linq;
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
    public sealed class LevelFactorySystem : ISystem
    {
        private readonly RuntimeData _runtimeData;
        private Stash<CurrentLevel> _currentLevel;
        private Filter _filter;
        private Stash<Level> _level;
        private Stash<Event_CompletedLoadingScene> _loadingSceneCompleted;
        private Filter _playerFilter;
        private Stash<Request_UpdatePlayerResourcesUi> _requestUpdatePlayerResourcesUi;
        private Stash<Request_CreatePlayer> _requestCreatePlayer;
        private Stash<MarkToDestroyWhenLevelChanged> _markToDestroyWhenLevelChanged;

        public LevelFactorySystem(DataLocator dataLocator)
        {
            _runtimeData = dataLocator.Get<RuntimeData>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_CompletedLoadingScene>();
            _playerFilter = Entities.With<Player>();
            
            _currentLevel = World.GetStash<CurrentLevel>();
            _level = World.GetStash<Level>();
            _loadingSceneCompleted = World.GetStash<Event_CompletedLoadingScene>();
            _requestUpdatePlayerResourcesUi = World.GetStash<Request_UpdatePlayerResourcesUi>();
            _requestCreatePlayer = World.GetStash<Request_CreatePlayer>();
            _markToDestroyWhenLevelChanged = World.GetStash<MarkToDestroyWhenLevelChanged>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                Debug.Log("- Loading Scene Completed");

                GameObjectPool.ClearPool();
                
                GameObjectPool.CreateFirstPool();

                ref var evt = ref _loadingSceneCompleted.Get(entity);
                var levelInstance = evt.Handle.Result.Scene.GetRootGameObjects()
                    .First(x => x.GetComponent<LevelScope>() != null)
                    .GetComponent<LevelScope>();

                var levelEntity = World.CreateEntity();

                _currentLevel.Add(levelEntity);
                _markToDestroyWhenLevelChanged.Add(levelEntity);
                _level.Set(levelEntity, new Level {
                    Instance = levelInstance,
                    EnemySpawners = new List<Entity>(),
                });


                _runtimeData.Camera = Camera.main;

                if (_playerFilter.IsEmpty())
                    _requestCreatePlayer.CreateRequest();

                ProviderActivatorManager.NotifyLevelLoaded();

                _requestUpdatePlayerResourcesUi.CreateRequest();
            }
        }
    }
}