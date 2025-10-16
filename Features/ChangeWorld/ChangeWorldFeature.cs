using Game.Systems;

namespace Game.Features
{
    public class ChangeWorldFeature : Feature
    {
        private readonly DataLocator _dataLocator;

        public ChangeWorldFeature(DataLocator dataLocator)
        {
            _dataLocator = dataLocator;
        }

        protected override void BuildGroup()
        {
            System(new ChangeWorldForwardSystem(_dataLocator));
            System(new PlayerLevelChangeSystem(_dataLocator));
        }
    }
}