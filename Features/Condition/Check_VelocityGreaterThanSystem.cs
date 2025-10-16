using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_VelocityGreaterThanSystem : ISystem
    {
        private Filter _filter;
        private Stash<VelocityGreaterThan> _velocityGreaterThan;
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Stash<RigidbodyFromConfig> _rigidbodyFromConfig;
        private Stash<Active> _active;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<VelocityGreaterThan, RigidbodyFromConfig, Active>();
            _velocityGreaterThan = World.GetStash<VelocityGreaterThan>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
            _rigidbodyFromConfig = World.GetStash<RigidbodyFromConfig>();
            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var velocityGreaterThan = ref _velocityGreaterThan.Get(entity);
                ref var rigidbodyFromConfig = ref _rigidbodyFromConfig.Get(entity);

                // Skip if Rigidbody is not set yet
                if (rigidbodyFromConfig.Rigidbody == null)
                    continue;

                // Check if velocity magnitude is greater than the threshold
                if (rigidbodyFromConfig.Rigidbody.velocity.magnitude > velocityGreaterThan.Value)
                {
                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                }
            }
        }
    }
}
