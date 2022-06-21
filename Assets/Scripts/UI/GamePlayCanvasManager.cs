using System.Collections;
using UnityEngine;
using XIV.UI;

public class GamePlayCanvasManager : MonoBehaviour
{
    [Header("Warning Settings")]
    [SerializeField] private float WarningTime = 2f;

    [Header("UI Elements")]
    [SerializeField] private PausedMenu_UI PauseMenuUI = default;
    [SerializeField] private BlackBoard_UI BlackBoardUI = default;
    [SerializeField] private LockedDoor_UI LockedDoorUI = default;
    [SerializeField] private HUD_UI HudUI = default;
    [SerializeField] private Notification Notification = default;
    [SerializeField] private WarningScreen WarningUI = default;

    public bool pauseMenu_acitveSelf { get => PauseMenuUI.gameObject.activeSelf; }
    public bool BlackBoardUI_acitveSelf { get => BlackBoardUI.gameObject.activeSelf; }
    public bool LockedDoorUI_acitveSelf { get => LockedDoorUI.gameObject.activeSelf; }
    public bool HudUI_acitveSelf { get => HudUI.gameObject.activeSelf; }
    public bool Notification_acitveSelf { get => Notification.gameObject.activeSelf; }
    public bool WarningUI_acitveSelf { get => WarningUI.gameObject.activeSelf; }

    [Header("Listening To")]
    [SerializeField] private BoolEventChannelSO PauseMenuUIChannel = default;
    [SerializeField] private BoolEventChannelSO BlackBoardUIChannel = default;
    [SerializeField] private LockedDoorUIEventChannel LockedDoorUIChannel = default;
    [SerializeField] private BoolEventChannelSO HudUIChannel = default;
    [SerializeField] private StringEventChannelSO NotificationUIChannel = default;
    [SerializeField] private StringEventChannelSO WarningUIChannel = default;

    private void OnEnable()
    {
        PauseMenuUIChannel.OnEventRaised += onPauseMenuRequested;
        BlackBoardUIChannel.OnEventRaised += onBlackBoardRequested;
        LockedDoorUIChannel.OnInteractPressed += onlockedDoorRequested;
        LockedDoorUIChannel.ScriptTransfer += lockedDoorScriptTransfer;
        HudUIChannel.OnEventRaised += onHudRequested;
        NotificationUIChannel.OnEventRaised += onNotificationRequested;
        WarningUIChannel.OnEventRaised += onWarningRequested;
    }

    private void OnDisable()
    {
        PauseMenuUIChannel.OnEventRaised -= onPauseMenuRequested;
        BlackBoardUIChannel.OnEventRaised -= onBlackBoardRequested;
        LockedDoorUIChannel.OnInteractPressed -= onlockedDoorRequested;
        LockedDoorUIChannel.ScriptTransfer -= lockedDoorScriptTransfer;
        HudUIChannel.OnEventRaised -= onHudRequested;
        NotificationUIChannel.OnEventRaised -= onNotificationRequested;
        WarningUIChannel.OnEventRaised -= onWarningRequested;
    }

    private void onPauseMenuRequested(bool value)
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

    private void onBlackBoardRequested(bool value)
    {
        BlackBoardUI.gameObject.SetActive(value);
    }

    private void onlockedDoorRequested()
    {
        if (LockedDoorUI.gameObject.activeSelf)
        {
            LockedDoorUI.gameObject.SetActive(false);
        }
        else
        {
            LockedDoorUI.gameObject.SetActive(true);
        }
    }

    private void lockedDoorScriptTransfer(Door_Is_Locked door)
    {
        LockedDoorUI.RecieveScriptFromDoor(door);
    }

    private void onHudRequested(bool value)
    {
        //TODO : Try to enable after disabled it.
        HudUI.gameObject.SetActive(value);
    }

    private void onNotificationRequested(string str, bool value)
    {
        Notification.gameObject.SetActive(value);
        Notification.SetText(str);
    }

    private void onWarningRequested(string str, bool value)
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
