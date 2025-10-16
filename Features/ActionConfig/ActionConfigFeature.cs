using Game.Systems;

namespace Game.Features
{
    public class ActionConfigFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new PositionFromConfigSystem());
            System(new RotationFromConfigSystem());
            System(new TransformFromConfigSystem());
            System(new RigidbodyFromConfigSystem());
            System(new GameObjectFromConfigSystem());
        }
    }
}
