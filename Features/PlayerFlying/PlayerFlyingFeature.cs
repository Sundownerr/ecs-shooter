using Game.Systems;

namespace Game.Features
{
    public class PlayerFlyingFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new PlayerFlyingSystem());
            System(new PlayerFlyingGravitySystem());
        }
    }
}