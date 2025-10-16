using EcsMagic.CommonComponents;
using EcsMagic.PlayerComponenets;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Deactivate_SpawnerLandingPointsSystem : ISystem
    {
        private readonly SpawnerLandingPointSettings _config;
        private Stash<ActiveSpawnerLandingPoint> _activePoints;
        private Filter _activePointsFilter;
        private Stash<Player> _player;
        private Filter _playerFilter;
        private Stash<SpawnerLandingPoint> _points;
        private Filter _updateEntityFilter;

        public Deactivate_SpawnerLandingPointsSystem(DataLocator dataLocator)
        {
            _config = dataLocator.Get<GameConfig>().SpawnerLandingPoints;
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _activePointsFilter = Entities.With<ActiveSpawnerLandingPoint>();

            _points = World.GetStash<SpawnerLandingPoint>();
            _activePoints = World.GetStash<ActiveSpawnerLandingPoint>();
            _player = World.GetStash<Player>();
            _updateEntityFilter = Entities.With<Active, SpawnerLandingPointsUpdateEntity>();
            _playerFilter = Entities.With<Player>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            if (_updateEntityFilter.IsEmpty() || _playerFilter.IsEmpty())
                return;

            ref var player = ref _player.Get(_playerFilter.First());
            var playerTransform = player.Instance.transform;
            var playerPosition = playerTransform.position;
            var playerForward = playerTransform.forward;

            foreach (var pointEntity in _activePointsFilter) {
                var shouldDeactivate = false;

                foreach (var deactivation in _config.DeactivationConstraints) {
                    if (!deactivation.Constraint.IsValid(pointEntity, playerPosition, playerForward, _points))
                        continue;
                    
                    shouldDeactivate = true;
                    break;
                }

                if (!shouldDeactivate)
                    continue;

                ref var point = ref _points.Get(pointEntity);
                point.Provider.Deactivate();
                _activePoints.Remove(pointEntity);
            }
        }
    }
}