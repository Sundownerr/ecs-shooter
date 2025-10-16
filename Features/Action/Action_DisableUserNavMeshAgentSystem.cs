using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using ProjectDawn.Navigation.Hybrid;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.AI;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Action_DisableUserNavMeshAgentSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<UserEntity> _userEntity;
        private Stash<Reference<AgentAuthoring>> _agentAuthoring;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<DisableUserNavMeshAgent, Active>();

            // Initialize stashes
            _userEntity = World.GetStash<UserEntity>();
            _agentAuthoring = World.GetStash<Reference<AgentAuthoring>>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var user = ref _userEntity.Get(entity);

                ref Reference<AgentAuthoring> userNavMeshAgent = ref _agentAuthoring.Get(user.Entity);
                userNavMeshAgent.Value.enabled = false;

                _active.Remove(entity);
            }
        }
    }
}
