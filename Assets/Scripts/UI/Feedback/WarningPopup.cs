using System.Collections;
using ElRaccoone.Tweens;
using TMPro;
using UnityEngine;

namespace LessonIsMath.UI
{
    public class WarningPopup : MonoBehaviour
    {
        TMP_Text txt_Warning;
        Vector3 initialScale;

        private void Awake()
        {
            initialScale = transform.localScale;
            txt_Warning = GetComponentInChildren<TMP_Text>();
        }

        public void ShowWarning(string text, bool show, float warningDuration)
        {
            if (show && gameObject.activeSelf) return;

            gameObject.TweenCancelAll();

            if (show)
            {
                gameObject.SetActive(true);
                transform.localScale = Vector3.zero;
                txt_Warning.text = text;
                gameObject.TweenLocalScale(initialScale, warningDuration * 0.5f)
                    .SetEase(ElRaccoone.Tweens.Core.EaseType.BounceOut)
                    .SetOnComplete(() => ShowWarning("", false, warningDuration));
                return;
            }

            transform.localScale = initialScale;
            gameObject.TweenLocalScale(Vector3.zero, warningDuration * 0.25f)
                .SetDelay(warningDuration * 0.25f)
                .SetEase(ElRaccoone.Tweens.Core.EaseType.BackIn)
                .SetOnComplete(() => gameObject.SetActive(false));
        }
    }
}