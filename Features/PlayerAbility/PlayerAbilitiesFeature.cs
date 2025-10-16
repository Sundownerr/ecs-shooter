using Game.Systems;

namespace Game.Features
{
    public class PlayerAbilitiesFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new PlayerJumpSystem());
            // System(new PlayerDashSystem());
            // System(new SlowMotion());
        }
    }
}