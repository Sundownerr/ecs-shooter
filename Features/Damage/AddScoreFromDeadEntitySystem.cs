using Game.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AddScoreFromDeadEntitySystem : ISystem
    {
        private readonly PlayerScoreService _playerScoreService;
        private Stash<DamageApplied> _damageApplied;
        private Filter _filter;
        private Stash<ScoreReward> _scoreReward;

        public AddScoreFromDeadEntitySystem(ServiceLocator serviceLocator)
        {
            _playerScoreService = serviceLocator.Get<PlayerScoreService>();
        }

        public void Dispose() { }

        public void OnAwake()
        {
            _filter = Entities.With<DiedNow, DamageApplied, ScoreReward>();
            _damageApplied = World.GetStash<DamageApplied>();
            _scoreReward = World.GetStash<ScoreReward>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter) {
                ref var damageApplied = ref _damageApplied.Get(entity);

                if (damageApplied.LastDamageDealer is not DamageDealerType.Player)
                    continue;

                ref var scoreReward = ref _scoreReward.Get(entity);
                _playerScoreService.Add(Score.ForKill(scoreReward.Value));
            }
        }
    }
}