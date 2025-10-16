using Game.NpcComponents;
using Game.Systems;

namespace Game.Features
{
    public class NpcActionFeature : Feature
    {
        protected override void BuildGroup()
        {
            System(new NpcAction_FollowTargetSystem());
            System(new NpcAction_StopFollowTargetSystem());
            System(new NpcAction_IdleSystem());
            System(new NpcAction_LookForTagetSystem());
            System(new NpcAction_RetreatSystem());
            
            System(new StopFollowingTargetOnIdleStateSystem());
            System(new StopFollowingTargetSystem());
            System(new NpcState_NavMeshRetreatSystem());
            
            System(new Remove<NpcState_Idle>());
            System(new Remove<NpcState_StopFollowTarget>());
        }
    }
}