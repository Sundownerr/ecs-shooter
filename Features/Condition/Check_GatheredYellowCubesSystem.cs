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
    public sealed class Check_GatheredYellowCubesSystem : ISystem
    {
        private Stash<ConditionFulfilled> _conditionFulfilled;
        private Filter _filter;
        private Stash<Level> _level;
        private Filter _levelFilter;

        private Stash<GatheredYellowCubesCondition> _yellowCubesCondition;

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<GatheredYellowCubesCondition, Active>();
            _yellowCubesCondition = World.GetStash<GatheredYellowCubesCondition>();
            _level = World.GetStash<Level>();
            _conditionFulfilled = World.GetStash<ConditionFulfilled>();

            _levelFilter = World.Filter.With<Level>().With<CurrentLevel>().Build();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var yellowCubesCondition = ref _yellowCubesCondition.Get(entity);
                ref var level = ref _level.Get(_levelFilter.First());

                var conditionMet = false;

                if (yellowCubesCondition.UsePercent) {
                    float percentGathered = 0;
                    if (level.YellowCubesOnLevel > 0)
                        percentGathered = level.PlayerGatheredYellowCubes * 100f / level.YellowCubesOnLevel;

                    switch (yellowCubesCondition.ComparisonType) {
                        case YellowCubesComparisonType.LessThan:
                            conditionMet = percentGathered < yellowCubesCondition.Value;
                            break;
                        case YellowCubesComparisonType.MoreThan:
                            conditionMet = percentGathered > yellowCubesCondition.Value;
                            break;
                    }
                }
                else {
                    switch (yellowCubesCondition.ComparisonType) {
                        case YellowCubesComparisonType.LessThan:
                            conditionMet = level.PlayerGatheredYellowCubes < yellowCubesCondition.Value;
                            break;
                        case YellowCubesComparisonType.MoreThan:
                            conditionMet = level.PlayerGatheredYellowCubes > yellowCubesCondition.Value;
                            break;
                    }
                }

                if (conditionMet) {
                    ref var conditionFulfilled = ref _conditionFulfilled.Get(entity);
                    conditionFulfilled.Value++;
                }
            }
        }
    }
}