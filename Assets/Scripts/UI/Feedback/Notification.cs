using TMPro;
using UnityEngine;

namespace XIV.UI
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] TMP_Text txt;

        private void Awake()
        {
            txt = GetComponentInChildren<TMP_Text>();
        }

        public void SetText(string text)
        {
            txt.text = text;
        }
    }
}