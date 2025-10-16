using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.AddressableAssets;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UnloadOldLevelSystem : ISystem
    {
        private Filter _currentLevelFilter;
        private Filter _filter;

        private readonly RuntimeData _runtimeData;

        public UnloadOldLevelSystem(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_StartedLoadingScene>();
            _currentLevelFilter = Entities.With<CurrentLevel>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                foreach (var currentLevelEntity in _currentLevelFilter) {
                    ref var currentLevel = ref currentLevelEntity.GetComponent<Level>();
                    
                    foreach (var enemySpawner in currentLevel.EnemySpawners)
                        World.RemoveEntity(enemySpawner);

                    World.RemoveEntity(currentLevelEntity);
                    ProviderActivatorManager.NotifyLevelUnloaded();
                }
            }
        }
    }
}