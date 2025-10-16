using Game.Systems;

namespace Game.Features
{
    public class CompleteActionFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new CompleteActionWhenDurationEndsSystem());
        }
    }
}