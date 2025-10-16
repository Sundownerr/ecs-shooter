using Game.Components;
using Game.Features;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ApplyAccumulatedScoreSystem : ISystem
    {
        private readonly PlayerScoreService _playerScoreService;
        private readonly PlayerScoreUiProvider _playerScoreUi;
        private Filter _filter;
        private Stash<PlayerResources> _playerReources;
        private Filter _playerResourcesFilter;

        public ApplyAccumulatedScoreSystem(ServiceLocator serviceLocator)
        {
            _playerScoreService = serviceLocator.Get<PlayerScoreService>();
            _playerScoreUi = serviceLocator.Get<UiService>().ScoreUi();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _playerResourcesFilter = Entities.With<PlayerResources>();
            _playerReources = World.GetStash<PlayerResources>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var totalScore = 0;

            foreach (var (scoreKey, newScore) in _playerScoreService.NewAccumulatedScore) {
                foreach (var entity in _playerResourcesFilter) {
                    ref var playerResources = ref _playerReources.Get(entity);
                    playerResources.Score += newScore.Value * newScore.Times;
                    totalScore = playerResources.Score;
                }

                var accumulatedScore = _playerScoreService.AccumulatedScore[scoreKey];
                _playerScoreUi.CreateScoreUi(accumulatedScore.Value, accumulatedScore.Times, scoreKey);
            }

            _playerScoreService.Update(deltaTime);

            if (totalScore > 0) {
                _playerScoreUi.UpdateTotalScore(totalScore);
                _playerScoreService.ClearNewAccumulatedScore();
            }
        }
    }
}