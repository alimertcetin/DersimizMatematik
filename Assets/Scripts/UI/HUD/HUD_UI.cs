using ElRaccoone.Tweens;
using UnityEngine;
using LessonIsMath.UI;

namespace XIV.UI
{
    public class HUD_UI : ParentGameUI
    {
        [SerializeField] HUDNumbersPage numbers;
        [SerializeField] HUDKeycardsPage keycards;
        [SerializeField] float alphaFadeDuration = 0.5f;

        public void ShowNumbers() => numbers.Show();
        public void HideNumbers() => numbers.Hide();
        public void ShowKeycards() => keycards.Show();
        public void HideKeycards() => keycards.Hide();

        public override void Show()
        {
            uiGameObject.SetActive(true);
            uiGameObject.TweenCancelAll();
            uiGameObject.TweenCanvasGroupAlpha(1f, alphaFadeDuration)
                .SetOnComplete(() => isActive = true)
                .SetOnCancel(() => isActive = true);
        }

        public override void Hide()
        {
            void OnTweenEnd()
            {
                uiGameObject.SetActive(false);
                isActive = false;
            }
            uiGameObject.TweenCancelAll();
            uiGameObject.TweenCanvasGroupAlpha(0f, alphaFadeDuration)
                .SetOnComplete(OnTweenEnd)
                .SetOnCancel(OnTweenEnd);
        }
    }
}