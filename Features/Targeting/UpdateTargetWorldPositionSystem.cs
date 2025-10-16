using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateTargetWorldPositionSystem : ISystem
    {
        private Filter _filter;
        private Stash<LastNavMeshPosition> _lastNavMeshPositionStash;
        private Stash<Targets> _targetStash;
        private Stash<TargetWorldPosition> _targetWorldPositionStash;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Targets, TargetWorldPosition>();
            _targetStash = World.GetStash<Targets>();
            _lastNavMeshPositionStash = World.GetStash<LastNavMeshPosition>();
            _targetWorldPositionStash = World.GetStash<TargetWorldPosition>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var targets = ref _targetStash.Get(entity);
                ref var targetWorldPosition = ref _targetWorldPositionStash.Get(entity);
                ref var lastNavMeshPosition = ref _lastNavMeshPositionStash.Get(targets.List[0]);

                targetWorldPosition.Value = lastNavMeshPosition.Value;
            }
        }
    }
}