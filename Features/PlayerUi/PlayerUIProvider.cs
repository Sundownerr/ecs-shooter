using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerUIProvider : MonoBehaviour
    {
        public TMP_Text HealthText;
        public Image HealthFill;
        [Space]
        public TMP_Text ManaText;
        public Image ManaFill;
        [Space]
        public TMP_Text YellowCubesCount;
        [Space] // Ammo UI Elements
        public TMP_Text AmmoText;
        public GameObject ReloadingIndicator;
        [Space]
        public TMP_Text VelocityText;
        [Space]
        public RectTransform Crosshair;
        public GameObject CrosshairHit;
        public GameObject CrosshairKill;
        private Sequence _crosshairHitSequence;
        private Sequence _crosshairShootSequence;
        private Sequence _crosshairKillSequence;

        private void Awake()
        {
            var defaultCrossheirSize = Crosshair.sizeDelta;
            _crosshairShootSequence = DOTween.Sequence()
                .Append(Crosshair.DOSizeDelta(new Vector2(50, 50), 0.04f))
                .Append(Crosshair.DOSizeDelta(defaultCrossheirSize, 0.05f))
                .SetAutoKill(false).Pause();

            CrosshairHit.SetActive(false);

            _crosshairHitSequence = DOTween.Sequence()
                .AppendCallback(() => CrosshairHit.SetActive(true))
                .AppendInterval(0.1f)
                .AppendCallback(() => CrosshairHit.SetActive(false))
                .SetAutoKill(false).Pause();

            CrosshairKill.SetActive(false);

            _crosshairKillSequence = DOTween.Sequence()
                .AppendCallback(() => CrosshairKill.SetActive(true))
                .AppendInterval(0.1f)
                .AppendCallback(() => CrosshairKill.SetActive(false))
                .SetAutoKill(false).Pause();
        }

        public void PlayCrosshairShootAnimation() =>
            _crosshairShootSequence.Restart();

        public void PlayCrosshairHitAnimation() =>
            _crosshairHitSequence.Restart();

        public void PlayCrosshairKillAnimation() =>
            _crosshairKillSequence.Restart();
    }
}