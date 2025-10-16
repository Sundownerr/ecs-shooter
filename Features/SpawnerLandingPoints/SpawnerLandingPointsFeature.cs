using Game.Components;
using Game.Data;
using Game.Features;
using Game.Systems;

namespace Game.Features
{
    public class SpawnerLandingPointsFeature : Feature
    {
        private DataLocator _dataLocator;
        public SpawnerLandingPointsFeature(DataLocator dataLocator)
        {
            _dataLocator = dataLocator;
        }

        protected override void BuildGroup()
        {
            // Add update system first
            System(new InitializeSpawnerLandingPointsSystem(_dataLocator));

            // Validation system - runs after update to identify valid points
            System(new Validate_SpawnerLandingPointsSystem(_dataLocator));

            // Deactivation system - runs after validation to deactivate invalid points
            System(new Deactivate_SpawnerLandingPointsSystem(_dataLocator));

            // Activation system - runs after deactivation to activate new valid points
            System(new Activate_SpawnerLandingPointsSystem(_dataLocator));

            // Add Use system (not automatically updated)
            System(new Use_SpawnerLandingPointSystem());
            System(new  RemoveSpawnerUpdateActiveSystem());
            
        }
    }
}
