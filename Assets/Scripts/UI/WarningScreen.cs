using TMPro;
using UnityEngine;

namespace LessonIsMath.UI
{
    public class WarningScreen : MonoBehaviour
    {
        private TMP_Text txt_Uyari;

        private void Awake()
        {
            txt_Uyari = GetComponentInChildren<TMP_Text>();
        }

        public void SetText(string text)
        {
            txt_Uyari.text = text;
        }
    }
}