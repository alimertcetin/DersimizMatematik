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
        [Header("Warning Settings")]
        [SerializeField] private float WarningTime = 2f;

        [Header("UI Elements")]
        [SerializeField] private PausedMenu_UI PauseMenuUI = default;
        [SerializeField] private BlackboardMainUI BlackBoardUI = default;
        [SerializeField] private LockedDoor_UI LockedDoorUI = default;
        [SerializeField] private HUD_UI HudUI = default;
        [SerializeField] private Notification Notification = default;
        [SerializeField] private WarningScreen WarningUI = default;

        public bool pauseMenu_acitveSelf { get => PauseMenuUI.gameObject.activeSelf; }

        [Header("Listening To")]
        [SerializeField] private BoolEventChannelSO PauseMenuUIChannel = default;
        [SerializeField] private BoolEventChannelSO BlackBoardUIChannel = default;
        [SerializeField] private DoorEventChannelSO LockedDoorUIChannel = default;
        [SerializeField] private BoolEventChannelSO HudUIChannel = default;
        [SerializeField] private StringEventChannelSO NotificationUIChannel = default;
        [SerializeField] private StringEventChannelSO WarningUIChannel = default;

        private void OnEnable()
        {
            PauseMenuUIChannel.OnEventRaised += ShowPauseMenuUI;
            BlackBoardUIChannel.OnEventRaised += ShowBlackBoardUI;
            LockedDoorUIChannel.OnEventRaised += ShowLockedDoorUI;
            HudUIChannel.OnEventRaised += ShowHud;
            NotificationUIChannel.OnEventRaised += ShowNotification;
            WarningUIChannel.OnEventRaised += ShowWarning;
        }

        private void OnDisable()
        {
            PauseMenuUIChannel.OnEventRaised -= ShowPauseMenuUI;
            BlackBoardUIChannel.OnEventRaised -= ShowBlackBoardUI;
            LockedDoorUIChannel.OnEventRaised -= ShowLockedDoorUI;
            HudUIChannel.OnEventRaised -= ShowHud;
            NotificationUIChannel.OnEventRaised -= ShowNotification;
            WarningUIChannel.OnEventRaised -= ShowWarning;
        }

        private void ShowPauseMenuUI(bool value)
        {
            if (value)
            {
                InputManager.GamePlay.Disable();
            }
            else
            {
                InputManager.GamePlay.Enable();
            }
            PauseMenuUI.gameObject.SetActive(value);
        }

        private void ShowBlackBoardUI(bool value)
        {
            if (value) BlackBoardUI.ShowUI();
            else BlackBoardUI.CloseUI();
        }

        private void ShowLockedDoorUI(Door door)
        {
            if (LockedDoorUI.isActive)
            {
                LockedDoorUI.CloseUI();
            }
            else
            {
                LockedDoorUI.ShowUI(door);
            }
        }

        private void ShowHud(bool value)
        {
            //TODO : Try to enable after disabled it.
            HudUI.gameObject.SetActive(value);
        }

        private void ShowNotification(string str, bool value)
        {
            Notification.gameObject.SetActive(value);
            Notification.SetText(str);
        }

        private void ShowWarning(string str, bool value)
        {
            if (value)
            {
                WarningUI.gameObject.SetActive(true);
                WarningUI.SetText(str);
                StartCoroutine(Warn(WarningTime));
            }
            else
            {
                WarningUI.gameObject.SetActive(false);
            }
        }

        private IEnumerator Warn(float time)
        {
            yield return new WaitForSeconds(time);
            WarningUI.gameObject.SetActive(false);
        }
    }
}