using EcsMagic.CommonComponents;
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
    public sealed class UpdateDistanceToTargetSystem : ISystem
    {
        private Stash<DistanceToTarget> _distanceToTargetStash;
        private Filter _filter;
        private Stash<TargetWorldPosition> _targetWorldPositionStash;
        private Stash<WorldPosition> _worldPositionStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<WorldPosition, DistanceToTarget, TargetWorldPosition>();
            _distanceToTargetStash = World.GetStash<DistanceToTarget>();
            _worldPositionStash = World.GetStash<WorldPosition>();
            _targetWorldPositionStash = World.GetStash<TargetWorldPosition>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var nativeFilter = _filter.AsNative();

            var parallelJob = new Job {
                entities = nativeFilter,
                positions = _worldPositionStash.AsNative(),
                distances = _distanceToTargetStash.AsNative(),
                targetPositions = _targetWorldPositionStash.AsNative(),
            };

            var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
            parallelJobHandle.Complete();
        }

        [BurstCompile]
        public struct Job : IJobParallelFor
        {
            public NativeStash<DistanceToTarget> distances;
            [ReadOnly] public NativeFilter entities;
            [ReadOnly] public NativeStash<WorldPosition> positions;
            [ReadOnly] public NativeStash<TargetWorldPosition> targetPositions;

            public void Execute(int index)
            {
                ref var botPosition = ref positions.Get(entities[index]);
                ref var targetPosition = ref targetPositions.Get(entities[index]);
                ref var distance = ref distances.Get(entities[index]);

                distance.Value = math.distance(targetPosition.Value, botPosition.Value);
            }
        }
    }
}