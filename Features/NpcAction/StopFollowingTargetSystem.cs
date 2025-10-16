using EcsMagic.CommonComponents;
using EcsMagic.NpcComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.NpcComponents;
using ProjectDawn.Navigation.Hybrid;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.AI;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public sealed class StopFollowingTargetSystem : ISystem
    {
        private Filter _filter;
        private Stash<NpcState_FollowTarget> _npcStateFollowTargetStash;
        private Stash<Reference<AgentAuthoring>> _navMeshAgentStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NpcState_StopFollowTarget, NpcState_FollowTarget, CanWalk>();
            _npcStateFollowTargetStash = World.GetStash<NpcState_FollowTarget>();
            _navMeshAgentStash = World.GetStash<Reference<AgentAuthoring>>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                _npcStateFollowTargetStash.Remove(entity);

                ref Reference<AgentAuthoring> navMeshAgent = ref _navMeshAgentStash.Get(entity);

                var body = navMeshAgent.Value.EntityBody;
                body.IsStopped = true;
                navMeshAgent.Value.EntityBody = body;
            }
        }
    }
}
