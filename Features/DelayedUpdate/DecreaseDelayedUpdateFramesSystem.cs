using Game.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Native;
using Unity.Burst;
using Unity.Collections;
using Unity.IL2CPP.CompilerServices;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DecreaseDelayedUpdateFramesSystem : ISystem
    {
        private Filter _filter;
        private Stash<DelayedUpdate> _framesPerUpdateStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<DelayedUpdate>();
            _framesPerUpdateStash = World.GetStash<DelayedUpdate>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var nativeFilter = _filter.AsNative();

            var parallelJob = new Job {
                entities = nativeFilter,
                framesPerUpdateStash = _framesPerUpdateStash.AsNative(),
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct Job : IJobParallelFor
        {
            [ReadOnly] public NativeFilter entities;
            public NativeStash<DelayedUpdate> framesPerUpdateStash;

            public void Execute(int index)
            {
                ref var framesPerUpdate = ref framesPerUpdateStash.Get(entities[index]);

                framesPerUpdate.RemainingFrames = math.select(
                    math.clamp(framesPerUpdate.RemainingFrames - 1, -1, framesPerUpdate.MaxFrames),
                    framesPerUpdate.MaxFrames,
                    framesPerUpdate.RemainingFrames < 0);
            }
        }
    }
}