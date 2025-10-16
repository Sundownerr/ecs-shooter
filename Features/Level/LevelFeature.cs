using Game.Systems;

namespace Game.Features
{
    public class LevelFeature : Feature
    {
        private readonly DataLocator _dataLocator;
        private readonly ServiceLocator _serviceLocator;

        public LevelFeature(DataLocator dataLocator, ServiceLocator serviceLocator)
        {
            _dataLocator = dataLocator;
            _serviceLocator = serviceLocator;
        }

        protected override void BuildGroup()
        {
            System(new LoadLevelSceneSystem(_dataLocator, _serviceLocator));
            // System(new UnloadOldLevelSystem(_runtimeData));
            System(new CheckLevelLoadedSystem());

            System(new LevelFactorySystem(_dataLocator));
            System(new ConstructLevelObjectsSystem());

            System(new PlacePlayerOnNewLevelSystem());

            System(new InitializeeTriggersOnNewLevelSystem());
        }
    }
}