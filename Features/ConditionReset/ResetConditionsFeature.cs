using Game.Systems;

namespace Game.Features
{
    public class ResetConditionsFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new ResetForwardConditionsToMeetSystem());
            System(new ResetCancelConditionsToMeetSystem());
            
            System(new ResetConditionFulfilledSystem());
        }
    }
}