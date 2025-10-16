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
    public sealed class CreateEnemyOnTimerCompletedSystem : ISystem
    {
        private Filter _currentLevelFilter;
        private Stash<EnemiesAlive> _enemiesAliveStash;
        private Stash<EnemySpawner> _enemySpawnerStash;
        private Filter _filter;
        private Stash<IncreasingTimer> _increasingTimerStash;
        private Stash<Level> _levelStash;
        private Stash<Request_CreateNpc> _requestCreateNpc;

        private Stash<TimerCompleted> _timerCompletedStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<EnemySpawner, TimerCompleted>();
            _currentLevelFilter = Entities.With<CurrentLevel>();

            _timerCompletedStash = World.GetStash<TimerCompleted>();
            _increasingTimerStash = World.GetStash<IncreasingTimer>();
            _enemiesAliveStash = World.GetStash<EnemiesAlive>();
            _enemySpawnerStash = World.GetStash<EnemySpawner>();
            _levelStash = World.GetStash<Level>();
            _requestCreateNpc = World.GetStash<Request_CreateNpc>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                _timerCompletedStash.Remove(entity);
                
                ref var timer = ref _increasingTimerStash.Get(entity);
                timer.Elapsed = 0f;

                var currentLevelEntity = _currentLevelFilter.First();

                ref var enemiesAlive = ref _enemiesAliveStash.Get(currentLevelEntity);
                ref var enemySpawner = ref _enemySpawnerStash.Get(entity);

                if (enemiesAlive.All[enemySpawner.Config.ID].Count >= enemySpawner.Config.MaxAmount)
                    continue;

                if (!enemySpawner.Config.SpawnOnStart)
                    continue;

                ref var level = ref _levelStash.Get(_currentLevelFilter.First());

                for(var i = 0; i < enemySpawner.Config.SpawnBatch; i++) {
                    var randomIndex = Random.Range(0, level.Instance.EnemySpawnPoints.All.Length);

                    _requestCreateNpc.CreateRequest(new Request_CreateNpc {
                        Config = enemySpawner.Config,
                        Position = level.Instance.EnemySpawnPoints.All[randomIndex].position,
                        Rotation = level.Instance.EnemySpawnPoints.All[randomIndex].rotation,
                    });
                }
            }
        }
    }
}