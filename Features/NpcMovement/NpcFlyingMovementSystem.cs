using EcsMagic.CommonComponents;
using EcsMagic.NpcComponents;
using Game.AbilityComponents;
using Game.Components;
using Game.NpcComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NpcFlyingMovementSystem : IFixedSystem
    {
        private Filter _filter;
        private Stash<FlyingAgent> _flyingAgentStash;
        private Stash<DistanceToTarget> _distanceToTargetStash;
        private Stash<TargetWorldPosition> _targetWorldPositionStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<NpcState_FollowTarget, CanFly, Targets, Active>();

            _flyingAgentStash = World.GetStash<FlyingAgent>();
            _distanceToTargetStash = World.GetStash<DistanceToTarget>();
            _targetWorldPositionStash = World.GetStash<TargetWorldPosition>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var flyingAgent = ref _flyingAgentStash.Get(entity);

                var direction = Vector3.zero;

                var instance = flyingAgent.Instance;
                var transformPosition = instance.transform.position;

                if (Physics.SphereCast(transformPosition, instance.GroundCheckRadius, Vector3.down, out var hit,
                    instance.MinimumHeight))
                    direction = Vector3.up * instance.UpForce;

                ref var distanceToTarget = ref _distanceToTargetStash.Get(entity);

                if (distanceToTarget.Value > instance.StopDistance)
                {
                    ref var targetWorldPosition = ref _targetWorldPositionStash.Get(entity);
                    direction += ((Vector3)targetWorldPosition.Value - transformPosition).normalized *
                                 instance.ForwardForce;
                }

                flyingAgent.Rigidbody.AddForce(direction, ForceMode.Force);
            }
        }
    }
}
