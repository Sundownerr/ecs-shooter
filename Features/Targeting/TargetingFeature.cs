using Game.Systems;

namespace Game.Features
{
    public class TargetingFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new UpdateTargetWorldPositionSystem());
            System(new UpdateDistanceToTargetSystem());
        }
    }
}