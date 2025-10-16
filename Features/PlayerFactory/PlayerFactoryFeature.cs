using Game.Data;
using Game.Systems;

namespace Game.Features
{
    public class PlayerFactoryFeature : Feature
    {
        private readonly DataLocator _dataLocator;

        public PlayerFactoryFeature(DataLocator dataLocator)
        {
            _dataLocator = dataLocator;
        }

        protected override void BuildGroup()
        {
            System(new PlayerFactorySystem(_dataLocator));
            System(new AddPlayerWeaponSystem(_dataLocator));
        }
    }
}