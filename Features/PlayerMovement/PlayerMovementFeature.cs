using Game.Systems;

namespace Game.Features
{
    public class PlayerMovementFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new ApplyMoveSpeedToMovement());
            System(new MovePlayerSystem());
        }
    }
}