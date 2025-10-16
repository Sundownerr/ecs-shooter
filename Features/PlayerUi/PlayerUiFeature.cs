using Game.Systems;

namespace Game.Features
{
    public class PlayerUiFeature : Feature
    {
        private readonly ServiceLocator _serviceLocator;

        public PlayerUiFeature(ServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        protected override void BuildGroup()
        {
            System(new PlayerDeathScreenSystem(_serviceLocator));

            System(new PlayerUiHealthUpdateFeature(_serviceLocator));
            System(new PlayerUiManaUpdateFeature(_serviceLocator));
            System(new PlayerUiUpdateYellowCubesSystem(_serviceLocator));
            System(new PlayerUiVelocityUpdateSystem(_serviceLocator));
            System(new CrosshairHitAnimationSystem(_serviceLocator));
            System(new CrosshairKillAnimationSystem(_serviceLocator));
            System(new UpdateWeaponUISystem(_serviceLocator)); // Added Weapon UI Update
        }
    }
}