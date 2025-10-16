using Game.Systems;

namespace Game.Features
{
    public class AbilityInitialStateFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new ResetUsagePartsStateSystem());
            System(new ResetOneShotSystem());
            System(new AbilityCooldownFinishedSystem());

            System(new Initial_To_UseConditionsSystem());
        }
    }
}