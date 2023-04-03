using LessonIsMath.UI;
using TMPro;
using UnityEngine;

namespace XIV.UI
{
    public class NotificationUI : GameUI
    {
        [SerializeField] TMP_Text txt_Notification;

        public override void Show()
        {
            uiGameObject.SetActive(true);
            isActive = true;
        }

        public override void Hide()
        {
            uiGameObject.SetActive(false);
            isActive = false;
        }

        public void SetText(string text)
        {
            txt_Notification.text = text;
        }
    }
}