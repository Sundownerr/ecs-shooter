using EcsMagic.CommonComponents;
using Game.AbilityComponents;
using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class Check_TotalYellowCubesSystem : ISystem
    {
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Filter _filter;
        private Stash<PlayerResources> _playerResources;
        private Filter _playerResourcesFilter;

        private Stash<TotalYellowCubesCondition> _totalYellowCubesCondition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<TotalYellowCubesCondition, Active>();
            _totalYellowCubesCondition = World.GetStash<TotalYellowCubesCondition>();
            _playerResources = World.GetStash<PlayerResources>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();

            _playerResourcesFilter = World.Filter.With<PlayerResources>().Build();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            if (_playerResourcesFilter.IsEmpty())
                return;

            var resourcesEntity = _playerResourcesFilter.First();
            ref var playerResources = ref _playerResources.Get(resourcesEntity);

            foreach (var entity in _filter)
            {
                ref var totalYellowCubesCondition = ref _totalYellowCubesCondition.Get(entity);

                var conditionMet = false;

                // Since we're checking total yellow cubes, UsePercent doesn't make much sense here
                // but we'll keep it for consistency with the existing YellowCubesCondition
                if (totalYellowCubesCondition.UsePercent)
                {
                    // We don't have a max value to calculate percentage against
                    // This could be implemented differently if needed
                    switch (totalYellowCubesCondition.ComparisonType)
                    {
                        case YellowCubesComparisonType.LessThan:
                            conditionMet = playerResources.YellowCubes < totalYellowCubesCondition.Value;
                            break;
                        case YellowCubesComparisonType.MoreThan:
                            conditionMet = playerResources.YellowCubes > totalYellowCubesCondition.Value;
                            break;
                    }
                }
                else
                {
                    switch (totalYellowCubesCondition.ComparisonType)
                    {
                        case YellowCubesComparisonType.LessThan:
                            conditionMet = playerResources.YellowCubes < totalYellowCubesCondition.Value;
                            break;
                        case YellowCubesComparisonType.MoreThan:
                            conditionMet = playerResources.YellowCubes > totalYellowCubesCondition.Value;
                            break;
                    }
                }

                if (conditionMet)
                {
                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                }
            }
        }
    }
}