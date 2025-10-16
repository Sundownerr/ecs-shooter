using Game.Systems;

namespace Game.Features
{
    public class LerpMovementFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new LerpMovementOvershootSystem());
        }
    }
}