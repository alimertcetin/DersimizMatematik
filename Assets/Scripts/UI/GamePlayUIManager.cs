using System.Collections;
using LessonIsMath.DoorSystems;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.UI;

namespace LessonIsMath.UI
{
    public class GamePlayUIManager : MonoBehaviour
    {
        [SerializeField] private float warningTime = 2f;
        [SerializeField] private Notification notification = default;
        [SerializeField] private WarningPopup warningPopup = default;

        [Header("Listening To")]
        [SerializeField] private BoolEventChannelSO pauseMenuUIChannel = default;
        [SerializeField] private BoolEventChannelSO blackBoardUIChannel = default;
        [SerializeField] private DoorEventChannelSO lockedDoorUIChannel = default;
        [SerializeField] private BoolEventChannelSO hudUIChannel = default;
        [SerializeField] private StringEventChannelSO notificationChannel = default;
        [SerializeField] private StringEventChannelSO warningChannel = default;

        private void OnEnable()
        {
            pauseMenuUIChannel.OnEventRaised += ShowPauseMenuUI;
            blackBoardUIChannel.OnEventRaised += ShowBlackBoardUI;
            lockedDoorUIChannel.OnEventRaised += ShowLockedDoorUI;
            hudUIChannel.OnEventRaised += ShowHud;
            notificationChannel.OnEventRaised += ShowNotification;
            warningChannel.OnEventRaised += ShowWarning;
        }

        private void OnDisable()
        {
            pauseMenuUIChannel.OnEventRaised -= ShowPauseMenuUI;
            blackBoardUIChannel.OnEventRaised -= ShowBlackBoardUI;
            lockedDoorUIChannel.OnEventRaised -= ShowLockedDoorUI;
            hudUIChannel.OnEventRaised -= ShowHud;
            notificationChannel.OnEventRaised -= ShowNotification;
            warningChannel.OnEventRaised -= ShowWarning;
        }

        private void ShowPauseMenuUI(bool value)
        {
            if (value)
            {
                InputManager.GamePlay.Disable();
                UISystem.Show<PausedMenu_UI>();
            }
            else
            {
                InputManager.GamePlay.Enable();
                UISystem.Hide<PausedMenu_UI>();
            }
        }

        private void ShowBlackBoardUI(bool value)
        {
            if (value) UISystem.Show<BlackboardUI>();
            else UISystem.Hide<BlackboardUI>();
        }

        private void ShowLockedDoorUI(Door door, bool value)
        {
            UISystem.GetUI<LockedDoor_UI>()?.SetDoor(door);
            if (value) UISystem.Show<LockedDoor_UI>();
            else UISystem.Hide<LockedDoor_UI>();
        }

        private void ShowHud(bool value)
        {
            if (value) UISystem.Show<HUD_UI>();
            else UISystem.Hide<HUD_UI>();
        }

        void ShowWarning(string text, bool show)
        {
            warningPopup.ShowWarning(text, show, warningTime);
        }

        private void ShowNotification(string str, bool value)
        {
            notification.SetText(str);
            notification.gameObject.SetActive(value);
        }
    }
}