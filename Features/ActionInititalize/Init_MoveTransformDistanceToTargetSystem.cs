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
    public sealed class Init_MoveTransformDistanceToTargetSystem : ISystem
    {
        private Filter _filter;
        private Stash<DistanceToTarget> _distanceToTarget;
        private Stash<PositionFromConfig> _positionFromConfig;
        private Stash<TransformFromConfig> _transformFromConfig;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveTransform, Active, DistanceToTarget>()
                .Without<Initialized>().Build();
            _distanceToTarget = World.GetStash<DistanceToTarget>();
            _positionFromConfig = World.GetStash<PositionFromConfig>();
            _transformFromConfig = World.GetStash<TransformFromConfig>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var distanceToTarget = ref _distanceToTarget.Get(entity);
                ref var positionFromConfig = ref _positionFromConfig.Get(entity);
                ref var transformFromConfig = ref _transformFromConfig.Get(entity);

                distanceToTarget.Value =
                    Vector3.Distance(positionFromConfig.Position, transformFromConfig.Transform.position);
            }
        }
    }
}
