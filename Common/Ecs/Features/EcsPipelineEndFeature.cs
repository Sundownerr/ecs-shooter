using Game.Systems;

namespace Game.Features
{
    public class EcsPipelineEndFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new CleanupEcsSystem());
            System(new RemoveOneFrameEntitiesSystem());
        }
    }
}