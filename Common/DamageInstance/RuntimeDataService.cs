using Game.Data;
using UnityEngine;

namespace Game
{
    public class RuntimeDataService
    {
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;
        private readonly GameConfig _gameConfig;

        public RuntimeDataService(RuntimeData runtimeData, StaticData staticData, GameConfig gameConfig)
        {
            _runtimeData = runtimeData;
            _staticData = staticData;
            _gameConfig = gameConfig;
        }

        public void InitializeData()
        {
            _runtimeData.StaticParticles = Object.Instantiate(_gameConfig.StaticParticlesPrefab, 
                    _staticData.DdolWrapper.transform)
                .GetComponent<StaticParticles>();

            _runtimeData.StaticParticles.Initialize();
        }
    }
}