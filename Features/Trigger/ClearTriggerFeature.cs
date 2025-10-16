using Game.Features;

namespace SDW.EcsMagic.Triggers
{
    public class ClearTriggerFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new ClearTriggerExitSystem());
            System(new ClearTriggerNotificationsSystem());
        }
    }
}