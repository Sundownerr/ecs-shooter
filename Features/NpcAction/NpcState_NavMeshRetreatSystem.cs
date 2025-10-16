using EcsMagic.CommonComponents;
using EcsMagic.NpcComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.NpcComponents;
using ProjectDawn.Navigation.Hybrid;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NpcState_NavMeshRetreatSystem : ISystem
    {
        private Filter _filter;
        private Stash<WorldPosition> _worldPositionStash;
        private Stash<TargetWorldPosition> _targetWorldPositionStash;
        private Stash<NpcState_Retreat> _npcStateRetreatStash;
        private Stash<Reference<AgentAuthoring>> _navMeshAgentStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NpcState_Retreat, CanWalk, Active>();
            _worldPositionStash = World.GetStash<WorldPosition>();
            _targetWorldPositionStash = World.GetStash<TargetWorldPosition>();
            _npcStateRetreatStash = World.GetStash<NpcState_Retreat>();
            _navMeshAgentStash = World.GetStash<Reference<AgentAuthoring>>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var userPosition = ref _worldPositionStash.Get(entity);
                ref var targetPosition = ref _targetWorldPositionStash.Get(entity);
                ref var state = ref _npcStateRetreatStash.Get(entity);
                var direction = math.normalize(userPosition.Value - targetPosition.Value);

                var minRetreatPoint = userPosition.Value + direction * state.MinDistance;
                var randomPoint = (float3)Random.insideUnitSphere * state.Radius;
                var retreatPosition = minRetreatPoint + randomPoint;
                NavMeshHit hit;

                var acc = 0;
                var navMeshSearchRadius = 1f;


                while (!NavMesh.SamplePosition(retreatPosition, out hit, navMeshSearchRadius, NavMesh.AllAreas) && acc < 10)
                {
                    randomPoint = (float3)Random.insideUnitSphere * state.Radius;
                    retreatPosition = minRetreatPoint + randomPoint;
                    navMeshSearchRadius *= 2f;
                    acc++;
                }

                if (acc >= 10)
                {
                    Debug.LogWarning("Cant find retreat position");
                }

                ref var navMeshAgent = ref _navMeshAgentStash.Get(entity);

                var body = navMeshAgent.Value.EntityBody;
                body.IsStopped = false;
                navMeshAgent.Value.EntityBody = body;

                navMeshAgent.Value.SetDestinationDeferred(hit.position);

                _npcStateRetreatStash.Remove(entity);
            }
        }
    }
}
