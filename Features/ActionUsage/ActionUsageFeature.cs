using Game.Systems;

namespace Game.Features
{
    public class ActionUsageFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new UpdateDurationSystem());
            System(new ActivateUsagePartsSystem());
            System(new DecreasePartsToCompleteWhenDurationEndsSystem());
          
        }
    }
}