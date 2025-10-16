using System;
using System.Collections.Generic;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ConstructLevelObjectsSystem : ISystem
    {
        private readonly List<LevelObject> _levelObjects = new();
        private readonly List<SpawnerProvider> _spawnerLandingPointProviders = new();
        private Filter _filter;
        private Filter _levelFilter;
        private Stash<SpawnerLandingPoint> _spawnerLandingPoint;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<Event_CompletedLoadingScene>();
            _levelFilter = Entities.With<Level>();
            _spawnerLandingPoint = World.GetStash<SpawnerLandingPoint>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var level = ref _levelFilter.First().GetComponent<Level>();

                _levelObjects.Clear();

                level.YellowCubesOnLevel = 0;

                level.Instance.GetComponentsInChildren(_levelObjects);
                level.Instance.GetComponentsInChildren(_spawnerLandingPointProviders);

                foreach (var levelObject in _levelObjects) {
                    switch (levelObject.Tag) {
                        case LevelObjectTag.None:
                            break;

                        case LevelObjectTag.YellowCube: {
                            // var yellowCubeEntity = World.CreateEntity();
                            // yellowCubeEntity.SetComponent(new YellowCube {Transform = levelObject.transform,});

                            level.YellowCubesOnLevel++;
                        }

                            break;

                        case LevelObjectTag.Teleporter:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                foreach (var provider in _spawnerLandingPointProviders) {
                    var providerEntity = provider.Initialize(World);

                    _spawnerLandingPoint.Set(providerEntity, new SpawnerLandingPoint {
                        Provider = provider,
                        Position = provider.transform.position,
                    });

                    providerEntity.AddComponent<MarkToDestroyWhenLevelChanged>();

                    foreach (var abilityProvider in provider.OnActivateAbility)
                        abilityProvider.Create(providerEntity, World);

                    foreach (var abilityProvider in provider.OnDeactivateAbility)
                        abilityProvider.Create(providerEntity, World);

                    if (provider.OnUsedAbility != null)
                        foreach (var abilityProvider in provider.OnUsedAbility)
                            abilityProvider.Create(providerEntity, World);

                    foreach (var abilityProvider in provider.AbilitiesToInitialize)
                        abilityProvider.Create(providerEntity, World);
                }
            }
        }
    }
}