using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/LockedDoorUI Event Channel")]
public class LockedDoorUIEventChannel : EventChannelBaseSO
{
    public UnityAction<Door_Is_Locked> ScriptTransfer;

    public UnityAction OnInteractPressed;

    public void SendScript(Door_Is_Locked door)
    {
        if(ScriptTransfer != null)
        {
            ScriptTransfer.Invoke(door);
        }
        else
        {
            Debug.LogWarning("A ScriptTransfer was requested, but nobody picked it up. " +
                "Check why there is no LockedDoor_UI_Manager already present, " +
                "and make sure it's listening on this Event channel.");
        }
    }

    public void InteractPressed()
    {
        if (ScriptTransfer != null)
        {
            OnInteractPressed.Invoke();
        }
        else
        {
            Debug.LogWarning("Interact button pressed but nobody picked it up." + 
                "Check why there is no LockedDoor_UI_Manager already present.");
        }
    }
}
