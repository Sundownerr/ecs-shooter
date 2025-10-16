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
    public sealed class Validate_SpawnerLandingPointsSystem : ISystem
    {
        private readonly SpawnerLandingPointSettings _config;
        private Stash<Player> _player;
        private Filter _playerFilter;
        private Stash<SpawnerLandingPoint> _points;
        private Filter _pointsFilter;
        private Filter _updateEntityFilter;
        private Stash<ValidSpawnerLandingPoint> _validPoints;
        private Filter _validPointsFilter;

        public Validate_SpawnerLandingPointsSystem(DataLocator dataLocator)
        {
            _config = dataLocator.Get<GameConfig>().SpawnerLandingPoints;
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _playerFilter = Entities.With<Player>();
            _pointsFilter = Entities.With<SpawnerLandingPoint>();
            _validPointsFilter = Entities.With<SpawnerLandingPoint, ValidSpawnerLandingPoint>();

            _points = World.GetStash<SpawnerLandingPoint>();
            _player = World.GetStash<Player>();
            _validPoints = World.GetStash<ValidSpawnerLandingPoint>();
            _updateEntityFilter = Entities.With<Active, SpawnerLandingPointsUpdateEntity>();
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

            foreach (var pointEntity in _validPointsFilter)
                _validPoints.Remove(pointEntity);

            foreach (var pointEntity in _pointsFilter) {
                var isValid = true;

                foreach (var activation in _config.ActivationConstraints) {
                    if (activation.Constraint.IsValid(pointEntity, playerPosition, playerForward, _points))
                        continue;

                    isValid = false;
                    break;
                }

                if (isValid)
                    _validPoints.Add(pointEntity);
            }
        }
    }
}