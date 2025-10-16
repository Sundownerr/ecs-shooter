using Game.Systems;

namespace Game.Features
{
    public class AbilityUsingToInitialStateFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new Using_To_InitialSystem());
        }
    }
}