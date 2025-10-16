using System.Collections.Generic;
using DG.Tweening;
using Game.Data;
using Game.Systems;
using TMPro;
using UnityEngine;

namespace Game
{
    public class PlayerScoreUiProvider : MonoBehaviour
    {
        private const int MAX_SCORE_UI = 1;
        public ScoreUi Prefab;
        public RectTransform ScoreContainer;
        public TMP_Text TotalScore;
        private readonly Dictionary<ScoreKey, Queue<ScoreUi>> _scoreUis = new();
        private ScoreSettings _scoreSettings;

        public void CreateScoreUi(int score, int times, ScoreKey key)
        {
            // Ensure we have a queue for this key
            if (!_scoreUis.ContainsKey(key))
                _scoreUis.Add(key, new Queue<ScoreUi>());

            ScoreUi scoreUi;

            if (_scoreUis[key].Count >= MAX_SCORE_UI) {
                scoreUi = _scoreUis[key].Dequeue();
                scoreUi.transform.SetAsFirstSibling();
            }
            else {
                scoreUi = Instantiate(Prefab, ScoreContainer);
                scoreUi.Construct(_scoreSettings);
                scoreUi.transform.SetAsLastSibling();
            }

            scoreUi.PlayAnimation(score, times);
            _scoreUis[key].Enqueue(scoreUi);
        }

        public void UpdateTotalScore(int totalScore)
        {
            TotalScore.text = totalScore.ToString();

            TotalScore.transform.DOKill(true);
            TotalScore.transform.DOScale(1f, 0.1f).From(2f);
        }

        public void Construct(GameConfig gameConfig) =>
            _scoreSettings = gameConfig.ScoreSettings;
    }
}