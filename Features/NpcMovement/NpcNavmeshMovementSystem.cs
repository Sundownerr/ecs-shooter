using EcsMagic.CommonComponents;
using EcsMagic.NpcComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.NpcComponents;
using ProjectDawn.Navigation.Hybrid;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NpcNavmeshMovementSystem : ISystem
    {
        private Stash<DistanceToTarget> _distanceToTarget;
        private Filter _filter;
        private Stash<NpcState_FollowTarget> _followTarget;
        private Stash<Reference<AgentAuthoring>> _navMeshAgents;
        private Stash<TargetWorldPosition> _targetWorldPosition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NpcState_FollowTarget, CanWalk, Targets, Active>();

            _navMeshAgents = World.GetStash<Reference<AgentAuthoring>>();
            _distanceToTarget = World.GetStash<DistanceToTarget>();
            _targetWorldPosition = World.GetStash<TargetWorldPosition>();
            _followTarget = World.GetStash<NpcState_FollowTarget>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref Reference<AgentAuthoring> navMeshAgent = ref _navMeshAgents.Get(entity);

                if (!navMeshAgent.Value.enabled)
                    continue;

                var body = navMeshAgent.Value.EntityBody;
                
                ref var followTarget = ref _followTarget.Get(entity);
                
                if (body.RemainingDistance < followTarget.MinDistance) {
                    ref var worldDistance = ref _distanceToTarget.Get(entity);
                
                    if (worldDistance.Value < followTarget.MinDistance) {
                        body.IsStopped = true;
                        continue;
                    }
                }

                ref var targetNavMeshPosition = ref _targetWorldPosition.Get(entity);

                body.IsStopped = false;
                navMeshAgent.Value.EntityBody = body;

                navMeshAgent.Value.SetDestination(targetNavMeshPosition.Value);
            }
        }
    }
}