using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_CheckSphereSystem : ISystem
    {
        private Filter _filter;

        // Stashes for component access
        private Stash<CheckSphereCondition> _checkSphere;
        private Stash<ConditionFulfilled> _conditionFulfilled;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<CheckSphereCondition, Active>();

            // Initialize stashes
            _checkSphere = World.GetStash<CheckSphereCondition>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var checkSphere = ref _checkSphere.Get(entity);

                if (!Physics.CheckSphere(checkSphere.CenterTransform.position, checkSphere.Radius,
                    checkSphere.LayerMask))
                    continue;

                ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                conditionFulfilled.Value++;
            }
        }
    }
}
