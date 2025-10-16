using EcsMagic.NpcComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NpcFactorySystem : ISystem
    {
        private Filter _currentLevelFilter;
        private Stash<EnemiesAlive> _enemiesAlive;
        private Stash<Event_NpcCreated> _eventNpcCreated;
        private Filter _filter;
        private Stash<Level> _level;
        private Stash<Request_CreateNpc> _requestCreateNpc;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Request_CreateNpc>().Build();
            _currentLevelFilter = World.Filter.With<CurrentLevel>().Build();

            _requestCreateNpc = World.GetStash<Request_CreateNpc>();
            _eventNpcCreated = World.GetStash<Event_NpcCreated>();
            _enemiesAlive = World.GetStash<EnemiesAlive>();
            _level = World.GetStash<Level>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var request = ref _requestCreateNpc.Get(entity);

                var currentLevelEntity = _currentLevelFilter.First();
                ref var currentLevel = ref _level.Get(currentLevelEntity);

                var npcInstance = Object.Instantiate(request.Config.Prefab,
                    request.Position,
                    request.Rotation,
                    currentLevel.Instance.transform);

                var npcEntity = npcInstance.Initialize(World);

                ref var enemiesAlive = ref _enemiesAlive.Get(_currentLevelFilter.First());
                enemiesAlive.All[request.Config.ID].Add(npcInstance);

                _eventNpcCreated.CreateEvent(new Event_NpcCreated {
                    Entity = npcEntity,
                    Instance = npcInstance,
                    Config = request.Config,
                });

                entity.CompleteRequest();
            }
        }
    }
}