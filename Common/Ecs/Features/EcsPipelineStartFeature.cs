using Game.Systems;

namespace Game.Features
{
    public class EcsPipelineStartFeature : Feature
    {
        protected override void BuildGroup() =>
            Initializer(new InitializeEcsExtentionsSystem());
    }
}