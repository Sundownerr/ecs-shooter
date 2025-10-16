using System.Collections.Generic;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CreateEnemySpawnersOnLevelSystem : ISystem
    {
        private Filter _currentLevelFilter;
        private Stash<EnemiesAlive> _enemiesAlive;
        private Stash<EnemySpawner> _enemySpawner;
        private Filter _filter;
        private Stash<Level> _level;
        private Stash<IncreasingTimer> _timer;
        private Stash<MarkToDestroyWhenLevelChanged> _markToDestroyWhenLevelChanged;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_CompletedLoadingScene>();
            _currentLevelFilter = Entities.With<CurrentLevel>();
            _level = World.GetStash<Level>();
            _enemiesAlive = World.GetStash<EnemiesAlive>();
            _timer = World.GetStash<IncreasingTimer>();
            _enemySpawner = World.GetStash<EnemySpawner>();
            _markToDestroyWhenLevelChanged = World.GetStash<MarkToDestroyWhenLevelChanged>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                foreach (var levelEntity in _currentLevelFilter) {
                    ref var level = ref _level.Get(levelEntity);

                    var aliveEnemiesDict = new Dictionary<int, List<NpcProvider>>();
                    _enemiesAlive.Set(levelEntity, new EnemiesAlive {All = aliveEnemiesDict,});

                    foreach (var enemyConfig in level.Instance.Enemies) {
                        var spawnerEntity = World.CreateEntity();

                        _timer.Set(spawnerEntity, new IncreasingTimer {Duration = enemyConfig.SpawnInterval,});
                        _enemySpawner.Set(spawnerEntity, new EnemySpawner {Config = enemyConfig,});
                        _markToDestroyWhenLevelChanged.Add(spawnerEntity);

                        level.EnemySpawners.Add(spawnerEntity);
                        aliveEnemiesDict.Add(enemyConfig.ID, new List<NpcProvider>());
                    }
                }
            }
        }
    }
}