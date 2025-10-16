using Game.Systems;

namespace Game.Features
{
    public class DamageFeature : Feature
    {
        private readonly ServiceLocator _serviceLocator;

        public DamageFeature(ServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        protected override void BuildGroup()
        {
            System(new RemoveDamagedMarkerSystem());
            System(new ApplyDamageSystem(_serviceLocator));
        }
    }
}