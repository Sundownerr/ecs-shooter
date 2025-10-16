using Game.Systems;

namespace Game.Features
{
    public class InitializationFeature : Feature
    {
        private readonly DataLocator _dataLocator;
        private readonly ServiceLocator _serviceLocator;

        public InitializationFeature(DataLocator dataLocator, ServiceLocator serviceLocator)
        {
            _dataLocator = dataLocator;
            _serviceLocator = serviceLocator;
        }

        protected override void BuildGroup() =>
            Initializer(new InitializationSystem(_dataLocator, _serviceLocator));
    }
}