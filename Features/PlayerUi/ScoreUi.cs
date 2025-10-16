using DG.Tweening;
using Game.Data;
using TMPro;
using UnityEngine;

namespace Game
{
    public class ScoreUi : MonoBehaviour
    {
        public TMP_Text ScoreValue;
        private Sequence _sequence;

        public void Construct(ScoreSettings settings) =>
            _sequence = DOTween.Sequence()
                .Append(transform.DOScale(1, settings.ShowTime).From(1.5f))
                .Join(ScoreValue.DOFade(1, settings.ShowTime))
                .AppendInterval(settings.AccumulationDuration)
                .Append(ScoreValue.DOFade(0, settings.FadeTime))
                .AppendCallback(() => {
                    ScoreValue.enabled = false;
                    gameObject.SetActive(false);
                })
                .SetAutoKill(false );

        public void PlayAnimation(int score, int times)
        {
            ScoreValue.SetText($"+{score} x {times}");
            gameObject.SetActive(true);
            ScoreValue.enabled = true;
            _sequence.Rewind();
            _sequence.Restart();
            _sequence.PlayForward();
        }
    }
}