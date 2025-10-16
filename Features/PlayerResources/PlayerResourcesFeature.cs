using Game.Systems;

namespace Game.Features
{
    public class PlayerResourcesFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new PlayerResourcesSystem());
            System(new PlayerManaRegenSystem());
        }
    }
}
