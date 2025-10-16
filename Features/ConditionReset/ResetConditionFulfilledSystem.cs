using Game.AbilityComponents;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Native;
using Unity.Burst;
using Unity.Collections;
using Unity.IL2CPP.CompilerServices;
using Unity.Jobs;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ResetConditionFulfilledSystem : ISystem
    {
        private Stash<ConditionFulfilled> _conditionFullfilled;
        private Filter _filter;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ConditionFulfilled>();
            _conditionFullfilled = World.GetStash<ConditionFulfilled>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var nativeFilter = _filter.AsNative();

            var parallelJob = new ParallelJob {
                Entities = nativeFilter,
                ConditionFulfilled = _conditionFullfilled.AsNative(),
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct ParallelJob : IJobParallelFor
        {
            public NativeStash<ConditionFulfilled> ConditionFulfilled;
            [ReadOnly]
            public NativeFilter Entities;

            public void Execute(int index)
            {
                ref var conditionFulfilled = ref ConditionFulfilled.Get(Entities[index]);
                conditionFulfilled.Value = 0;
            }
        }
    }
}