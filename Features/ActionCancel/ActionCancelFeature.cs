using Game.Systems;

namespace Game.Features
{
    public class ActionCancelFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new ChangeCancelledAbilityStateSystem());
            
            System(new DeactivateInstantActionsSystem());
            System(new DeactivateActionsInProgressSystem());
            System(new DeactivateCancelConditionsSystem());
            
            System(new ActivateCancelActionsSystem());
            
            System(new RemoveCancelledTag());
        }
    }
}