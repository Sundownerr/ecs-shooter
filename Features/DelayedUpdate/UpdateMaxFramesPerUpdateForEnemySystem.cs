using EcsMagic.NpcComponents;
using Game.Components;
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
    public sealed class UpdateMaxFramesPerUpdateForEnemySystem : ISystem
    {
        private Stash<DistanceToTarget> _distanceToTargetStash;
        private Filter _filter;
        private Stash<DelayedUpdate> _delayedUpdateStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Npc, DelayedUpdate, DistanceToTarget>();
            _delayedUpdateStash = World.GetStash<DelayedUpdate>();
            _distanceToTargetStash = World.GetStash<DistanceToTarget>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var nativeFilter = _filter.AsNative();

            var parallelJob = new Job {
                _entities = nativeFilter,
                _delayedUpdateStash = _delayedUpdateStash.AsNative(),
                _distanceToTargetStash = _distanceToTargetStash.AsNative(),
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct Job : IJobParallelFor
        {
            [ReadOnly] public NativeStash<DistanceToTarget> _distanceToTargetStash;
            [ReadOnly] public NativeFilter _entities;
            public NativeStash<DelayedUpdate> _delayedUpdateStash;

            public void Execute(int index)
            {
                ref var delayedUpdate = ref _delayedUpdateStash.Get(_entities[index]);
                ref var distanceToPlayer = ref _distanceToTargetStash.Get(_entities[index]);

                delayedUpdate.MaxFrames = (short) (distanceToPlayer.Value * 0.6f);
            }
        }
    }
}