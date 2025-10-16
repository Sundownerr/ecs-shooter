using Game.Systems;

namespace Game.Features
{
    public class AbilityStatesFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new UseConditions_To_StartCooldownSystem());

            System(new AbilityCooldownSystem());
            System(new StartCooldown_To_UsingSystem());

#if UNITY_EDITOR
            System(new Debug_AbilityLogSystem());
#endif

        }
    }
}