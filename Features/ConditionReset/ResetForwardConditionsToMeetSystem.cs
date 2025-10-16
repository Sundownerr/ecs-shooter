using Game.StateMachineComponents;
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
    public sealed class ResetForwardConditionsToMeetSystem : ISystem
    {
        private Filter _filter;
        private Stash<ForwardConditionsToMeet> _forwardConditionsToMeet;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<ForwardConditionsToMeet>();
            _forwardConditionsToMeet = World.GetStash<ForwardConditionsToMeet>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var nativeFilter = _filter.AsNative();
            
            var parallelJob = new ParallelJob {
                Entities = nativeFilter,
                ForwardConditionsToMeet = _forwardConditionsToMeet.AsNative(),
            };
            
            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct ParallelJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeFilter Entities;
            public NativeStash<ForwardConditionsToMeet> ForwardConditionsToMeet;

            public void Execute(int index)
            {
                ref var toMeet = ref ForwardConditionsToMeet.Get(Entities[index]);
                toMeet.Remaining = toMeet.Total;
            }
        }
    }
}