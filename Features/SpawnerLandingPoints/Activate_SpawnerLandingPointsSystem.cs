using EcsMagic.CommonComponents;
using EcsMagic.PlayerComponenets;
using Game.Components;
using Game.Data;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Activate_SpawnerLandingPointsSystem : ISystem
    {
        private const int MAX_POINTS = 1024;
        private readonly SpawnerLandingPointSettings _config;
        private readonly Entity[] _pointsToActivate;
        private Stash<ActiveSpawnerLandingPoint> _activePoints;
        private Filter _activePointsFilter;
        private Filter _playerFilter;

        private Stash<SpawnerLandingPoint> _point;
        private Filter _updateEntityFilter;
        private Filter _validInactivePointsFilter;

        public Activate_SpawnerLandingPointsSystem(DataLocator dataLocator)
        {
            _pointsToActivate = new Entity[MAX_POINTS];
            _config = dataLocator.Get<GameConfig>().SpawnerLandingPoints;
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _playerFilter = Entities.With<Player>();

            // Valid points that are not yet active
            _validInactivePointsFilter = World.Filter
                .With<SpawnerLandingPoint, ValidSpawnerLandingPoint>()
                .Without<ActiveSpawnerLandingPoint>()
                .Build();

            _activePointsFilter = Entities.With<SpawnerLandingPoint, ActiveSpawnerLandingPoint>();

            _point = World.GetStash<SpawnerLandingPoint>();
            _activePoints = World.GetStash<ActiveSpawnerLandingPoint>();
            _updateEntityFilter = Entities.With<Active, SpawnerLandingPointsUpdateEntity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            if (_updateEntityFilter.IsEmpty() || _playerFilter.IsEmpty())
                return;

            var currentActivePoints = 0;
            foreach (var _ in _activePointsFilter)
                currentActivePoints++;

            var remainingPointsToActivate = _config.MaxActivePoints - currentActivePoints;
            if (remainingPointsToActivate <= 0)
                return;

            var validInactivePointCount = 0;

            foreach (var pointEntity in _validInactivePointsFilter) {
                if (validInactivePointCount >= MAX_POINTS)
                    break;

                _pointsToActivate[validInactivePointCount] = pointEntity;
                validInactivePointCount++;
            }

            if (validInactivePointCount <= 0)
                return;

            remainingPointsToActivate = Mathf.Min(remainingPointsToActivate, validInactivePointCount);

            if (_config.RandomizePoints)
                // Use Fisher-Yates shuffle for randomization
                for(var i = 0; i < validInactivePointCount; i++) {
                    var randomIndex = Random.Range(i, validInactivePointCount);

                    // Swap
                    (_pointsToActivate[i], _pointsToActivate[randomIndex]) =
                        (_pointsToActivate[randomIndex], _pointsToActivate[i]);
                }

            // Activate points until we've reached the target count
            for(var i = 0; i < remainingPointsToActivate; i++) {
                ref var point = ref _point.Get(_pointsToActivate[i]);
                point.Provider.Activate();
                _activePoints.Add(_pointsToActivate[i]);
            }
        }
    }
}