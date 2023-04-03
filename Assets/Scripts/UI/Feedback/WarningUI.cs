using ElRaccoone.Tweens;
using TMPro;
using UnityEngine;

namespace LessonIsMath.UI
{
    public class WarningUI : GameUI
    {
        [SerializeField] float warningDuration = 2f;
        [SerializeField] TMP_Text txt_Warning;

        public override void Show()
        {
            base.Show();
            
            txt_Warning.transform.localScale = Vector3.zero;
            txt_Warning.gameObject.SetActive(true);
            txt_Warning.TweenLocalScale(Vector3.one, warningDuration * 0.5f)
                .SetEase(ElRaccoone.Tweens.Core.EaseType.BounceOut);
        }

        public override void Hide()
        {
            base.Hide();
            
            txt_Warning.transform.localScale = Vector3.one;
            txt_Warning.TweenLocalScale(Vector3.zero, warningDuration * 0.25f)
                .SetDelay(warningDuration * 0.25f)
                .SetEase(ElRaccoone.Tweens.Core.EaseType.BackIn)
                .SetOnComplete(() => gameObject.SetActive(false));
        }

        public void SetText(string text)
        {
            txt_Warning.text = text;
        }
    }
}