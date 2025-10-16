using Game.Systems;

namespace Game.Features
{
    public class PlayerScoreFeature : Feature
    {
        private readonly ServiceLocator _serviceLocator;

        public PlayerScoreFeature(ServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        protected override void BuildGroup()
        {
            System(new AddScoreFromDeadEntitySystem(_serviceLocator));
            System(new ApplyAccumulatedScoreSystem(_serviceLocator));
        }
    }
}