using EcsMagic.CommonComponents;
using Game.Components;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitializeSpawnerLandingPointsSystem : ISystem
    {
        private readonly SpawnerLandingPointSettings _spawnerLandingPoints;
        private Stash<Active> _active;
        private Filter _filter;

        private Stash<IncreasingTimer> _timer;
        private Stash<TimerCompleted> _timerCompleted;

        private Entity _updateEntity;

        public InitializeSpawnerLandingPointsSystem(DataLocator dataLocator)
        {
            _spawnerLandingPoints = dataLocator.Get<GameConfig>().SpawnerLandingPoints;
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<SpawnerLandingPointsUpdateEntity, TimerCompleted>();
            _timer = World.GetStash<IncreasingTimer>();
            _timerCompleted = World.GetStash<TimerCompleted>();
            Stash<SpawnerLandingPointsUpdateEntity> spawnerUpdate = World.GetStash<SpawnerLandingPointsUpdateEntity>();

            _updateEntity = World.CreateEntity();
            _timer.Set(_updateEntity, new IncreasingTimer {
                Duration = _spawnerLandingPoints.UpdateInterval,
                Elapsed = 0f,
            });

            spawnerUpdate.Add(_updateEntity);

            _active = World.GetStash<Active>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var timer = ref _timer.Get(_updateEntity);
                timer.Elapsed = 0;

                _timerCompleted.Remove(entity);
                _active.Add(entity);
            }
        }
    }
}