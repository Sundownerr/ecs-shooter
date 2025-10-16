using Game.Systems;

namespace Game.Features
{
    public class AbilityTargetFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new UpdateAllInAoeTargetsSystem());
            System(new UpdateUserTargetSystem());
        }
    }
}