using Game.Systems;

namespace Game.Features
{
    public class WorldPositionFeature : Feature
    {
        protected override void BuildGroup() =>
            System(new UpdateTransformWorldPositionSystem());
    }
}