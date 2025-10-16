using Game.Systems;

namespace Game.Features
{
    public class TimerFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new TimerSystem());
            System(new CompleteTimerSystem());
        }
    }
}