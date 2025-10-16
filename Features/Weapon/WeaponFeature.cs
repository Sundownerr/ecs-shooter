using Game.Data;
using Game.Systems;

namespace Game.Features
{
    public class WeaponFeature : Feature
    {
        private readonly DataLocator _dataLocator;
        private readonly ServiceLocator _serviceLocator;

        public WeaponFeature(DataLocator dataLocator, ServiceLocator serviceLocator)
        {
            _dataLocator = dataLocator;
            _serviceLocator = serviceLocator;
        }

        protected override void BuildGroup()
        {
            System(new WeaponFactorySystem());
            System(new PlayerWeaponTriggerPullSystem());

            System(new UpdatePlayerHitscanWeaponSystem(_dataLocator));
            System(new WeaponShootSystem());
            System(new WeaponAmmoSystem()); // Added

            System(new GameObjectWeaponProjectileFactorySystem());
            System(new GameObjectProjectileMoveSystem());
            System(new GameObjectProjectileHitSystem(_dataLocator, _serviceLocator));

            System(new HitscanPlaceAtRayEndSystem());
            System(new HitscanShootSystem(_serviceLocator));

            System(new HybridShootSystem());

            System(new ParticleWeaponHitSystem(_serviceLocator));

            System(new PlayWeaponOnHitFeedbackSystem());

            System(new GameObjectProjectileDestroyOnHitSystem());
            System(new CrosshairOnShootSystem(_serviceLocator));

            System(new WeaponCooldownCompletionSystem()); 
            System(new ResetWeaponTimerSystem());
            System(new WeaponReloadProgressSystem()); // Added
        }
    }
}