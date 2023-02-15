using System.Collections;
using TMPro;
using UnityEngine;

namespace LessonIsMath.UI
{
    public class WarningPopup : MonoBehaviour
    {
        private TMP_Text txt_Uyari;
        Coroutine warnCoroutine;

        private void Awake()
        {
            txt_Uyari = GetComponentInChildren<TMP_Text>();
        }

        public void ShowWarning(string text, bool show, float warningDuration)
        {
            gameObject.SetActive(show);
            txt_Uyari.text = text;
            if (show == false) return;

            if (warnCoroutine != null) StopCoroutine(warnCoroutine);

            warnCoroutine = StartCoroutine(Warn(warningDuration));
        }

        private IEnumerator Warn(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
    }
}