using System;
using System.Collections.Generic;
using Game.Data;

namespace Game.Systems
{
    public struct Score
    {
        public int Value;
        public ScoreType Type;

        public static Score ForKill(int value) => new() {Value = value, Type = ScoreType.Kill,};
    }

    public enum ScoreType { None = 0, Kill = 1, }

    public readonly struct ScoreKey : IEquatable<ScoreKey>
    {
        private readonly ScoreType Type;
        private readonly int Value;

        public ScoreKey(ScoreType type, int value)
        {
            Type = type;
            Value = value;
        }

        public bool Equals(ScoreKey other) =>
            Type == other.Type && Value == other.Value;

        public override bool Equals(object obj) =>
            obj is ScoreKey other && Equals(other);

        public override int GetHashCode() =>
            HashCode.Combine((int) Type, Value);
    }

    public class AccumulatedScore
    {
        public int Times;
        public int Value;
    }

    public class PlayerScoreService
    {
        private readonly GameConfig _gameConfig;

        public readonly Dictionary<ScoreKey, AccumulatedScore> AccumulatedScore = new();
        public readonly Dictionary<ScoreKey, AccumulatedScore> NewAccumulatedScore = new();
        private float _currentCleanTime;

        public PlayerScoreService(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public void Add(Score score)
        {
            var key = new ScoreKey(score.Type, score.Value);

            if (!NewAccumulatedScore.TryAdd(key, new AccumulatedScore {Value = score.Value, Times = 1,}))
                NewAccumulatedScore[key].Times++;

            if (!AccumulatedScore.TryAdd(key, new AccumulatedScore {Value = score.Value, Times = 1,}))
                AccumulatedScore[key].Times++;
        }

        public void Update(float deltaTime)
        {
            _currentCleanTime += deltaTime;

            var cleanTime = _gameConfig.ScoreSettings.ShowTime +
                            _gameConfig.ScoreSettings.FadeTime +
                            _gameConfig.ScoreSettings.AccumulationDuration;

            if (_currentCleanTime >= cleanTime) {
                AccumulatedScore.Clear();
                UpdateClearTimer();
            }
        }

        public void ClearNewAccumulatedScore() => NewAccumulatedScore.Clear();

        public void UpdateClearTimer() =>
            _currentCleanTime = 0;
    }
}