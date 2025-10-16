using Game.Systems;

namespace Game.Features
{
    public class GravityFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new UpdateIsGroundedSystem());
            System(new ApplyGravitySystem());
        }
    }
}