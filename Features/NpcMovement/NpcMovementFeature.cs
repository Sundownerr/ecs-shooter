using Game.Systems;

namespace Game.Features
{
    public class NpcMovementFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new UpdateNpcPositionSystem());
            System(new NpcNavmeshMovementSystem());
            System(new NpcFlyingMovementSystem());
        }
    }
}