using Game.Components;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ChangeWorldForwardSystem : ISystem
    {
        private readonly RuntimeData _runtimeData;
        private readonly GameConfig _gameConfig;
        private Filter _filter;

        public ChangeWorldForwardSystem(DataLocator dataLocator)
        {
            _runtimeData = dataLocator.Get<RuntimeData>();
            _gameConfig = dataLocator.Get<GameConfig>();
        }

        public void Dispose() { }

        public void OnAwake() =>
            _filter = Entities.With<Request_ChangeWorldForward>();

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                var levelIndex = _runtimeData.CurrentLevelIndex + 1;

                if (levelIndex <= _gameConfig.Levels.Length)
                    LevelsEcsUtility.NotifyLoadingNewLevel(levelIndex);

                entity.CompleteRequest();
            }
        }
    }
}