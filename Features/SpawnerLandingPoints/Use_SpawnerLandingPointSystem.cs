using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Use_SpawnerLandingPointSystem : ISystem
    {
        private Stash<SpawnerLandingPoint> _points;
        private Stash<ActiveSpawnerLandingPoint> _activePoints;
        private Stash<UsedSpawnerLandingPoint> _usedPoints;

        public void Dispose() { }

        public void OnAwake()
        {
            _points = World.GetStash<SpawnerLandingPoint>();
            _activePoints = World.GetStash<ActiveSpawnerLandingPoint>();
            _usedPoints = World.GetStash<UsedSpawnerLandingPoint>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            // This system won't run automatically - it will be triggered by other systems
        }

        // Public method that can be called by other systems
        public void MarkPointAsUsed(Entity pointEntity)
        {
            if (_activePoints.Has(pointEntity) && !_usedPoints.Has(pointEntity))
            {
                ref var point = ref _points.Get(pointEntity);
                point.Provider.Use();
                _usedPoints.Add(pointEntity);
            }
        }
    }
}
