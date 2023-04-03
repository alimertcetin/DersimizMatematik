using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.EventSystem;
using XIV.EventSystem.Events;
using XIV.UI;

namespace LessonIsMath.UI
{
    public class FeedbackUI : MonoBehaviour
    {
        [SerializeField] float warningDuration = 2f;
        [SerializeField] StringEventChannelSO notificationChannel;
        [SerializeField] StringEventChannelSO warningChannel;
        [SerializeField] BoolEventChannelSO showSaveIndicatorChannel;
        NotificationUI notificationUI;
        WarningUI warningUI;
        IEvent warningEvent;
        
        protected void Start()
        {
            notificationUI = UISystem.GetUI<NotificationUI>();
            warningUI = UISystem.GetUI<WarningUI>();
        }

        void OnEnable()
        {
            notificationChannel.OnEventRaised += ShowNotification;
            warningChannel.OnEventRaised += ShowWarning;
            showSaveIndicatorChannel.OnEventRaised += ShowSaveIndicator;
        }

        void OnDisable()
        {
            notificationChannel.OnEventRaised -= ShowNotification;
            warningChannel.OnEventRaised -= ShowWarning;
            showSaveIndicatorChannel.OnEventRaised -= ShowSaveIndicator;
        }

        void ShowNotification(string text)
        {
            bool show = string.IsNullOrEmpty(text) == false;
            notificationUI.SetText(text);
            if (show) UISystem.Show<NotificationUI>();
            else UISystem.Hide<NotificationUI>();
        }

        void ShowWarning(string text)
        {
            bool show = string.IsNullOrEmpty(text) == false;
            warningUI.SetText(text);
            if (show)
            {
                UISystem.Show<WarningUI>();
                if (warningEvent != null) XIVEventSystem.CancelEvent(warningEvent);
                warningEvent = new InvokeAfterEvent(warningDuration).OnCompleted(() =>
                {
                    warningChannel.RaiseEvent(string.Empty);
                    warningEvent = null;
                });
                XIVEventSystem.SendEvent(warningEvent);
            }
            else UISystem.Hide<WarningUI>();
        }

        void ShowSaveIndicator(bool value)
        {
            if (value) UISystem.Show<SaveFeedbackUI>();
            else UISystem.Hide<SaveFeedbackUI>();
        }
    }
}