using Game.StateMachineComponents;
using Game.Systems;

namespace Game.Features
{
    public class StateMachineFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new RemoveTransitionResetDurationsTagSystem());
            System(new RemoveEnterActionResetDurationsTagSystem());

            System(new UpdateStateMachineExitTimeSystem());

            System(new CheckTransitionConditionsSystem());
            System(new ActivateTransitionWithoutConditionSystem());
            System(new AddStateMachineExitTimeSystem());

            // System(new ChangePreviousTransitionSystem());
            System(new ChangeStateMachineStateSystem());

            System(new ActivateStateInstantActionsSystem());
            System(new ActivateStateChannelActionsSystem());
            System(new ActivateStateAbilitiesSystem());
            System(new ActivateStateNpcActionsSystem());

            System(new DeactivateStateInstantUsageSystem());

            System(new Remove<NeedsActivation>());
            System(new Remove<ChangeState>());
        }
    }
}