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
        [SerializeField] float warningTime = 2f;
        [SerializeField] Notification notification;
        [SerializeField] WarningPopup warningPopup;

        [Header("Listening To")]
        [SerializeField]
        BoolEventChannelSO pauseMenuUIChannel;
        [SerializeField] BoolEventChannelSO blackBoardUIChannel;
        [SerializeField] DoorEventChannelSO lockedDoorUIChannel;
        [SerializeField] BoolEventChannelSO hudUIChannel;
        [SerializeField] StringEventChannelSO notificationChannel;
        [SerializeField] StringEventChannelSO warningChannel;

        void OnEnable()
        {
            pauseMenuUIChannel.OnEventRaised += ShowPauseMenuUI;
            blackBoardUIChannel.OnEventRaised += ShowBlackBoardUI;
            lockedDoorUIChannel.OnEventRaised += ShowLockedDoorUI;
            hudUIChannel.OnEventRaised += ShowHud;
            notificationChannel.OnEventRaised += ShowNotification;
            warningChannel.OnEventRaised += ShowWarning;
        }

        void OnDisable()
        {
            pauseMenuUIChannel.OnEventRaised -= ShowPauseMenuUI;
            blackBoardUIChannel.OnEventRaised -= ShowBlackBoardUI;
            lockedDoorUIChannel.OnEventRaised -= ShowLockedDoorUI;
            hudUIChannel.OnEventRaised -= ShowHud;
            notificationChannel.OnEventRaised -= ShowNotification;
            warningChannel.OnEventRaised -= ShowWarning;
        }

        void ShowPauseMenuUI(bool value)
        {
            if (value)
            {
                InputManager.CharacterMovement.Disable();
                UISystem.Show<PausedMenu_UI>();
            }
            else
            {
                InputManager.CharacterMovement.Enable();
                UISystem.Hide<PausedMenu_UI>();
            }
        }

        void ShowBlackBoardUI(bool value)
        {
            if (value) UISystem.Show<BlackboardUI>();
            else UISystem.Hide<BlackboardUI>();
        }

        void ShowLockedDoorUI(DoorManager doorManager, bool value)
        {
            UISystem.GetUI<LockedDoor_UI>()?.SetDoor(doorManager);
            if (value) UISystem.Show<LockedDoor_UI>();
            else UISystem.Hide<LockedDoor_UI>();
        }

        void ShowHud(bool value)
        {
            if (value) UISystem.Show<HUD_UI>();
            else UISystem.Hide<HUD_UI>();
        }

        void ShowWarning(string text, bool show)
        {
            warningPopup.ShowWarning(text, show, warningTime);
        }

        void ShowNotification(string str, bool value)
        {
            notification.SetText(str);
            notification.gameObject.SetActive(value);
        }
    }
}