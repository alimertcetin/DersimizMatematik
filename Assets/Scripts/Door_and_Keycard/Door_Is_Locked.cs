using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Door_Status))]
[DisallowMultipleComponent]
public class Door_Is_Locked : MonoBehaviour,ISaveable
{
    public UnityAction<Door_Is_Locked> KapiAcildi;

    private bool _doorLocked = true;
    public bool DoorLocked { get => _doorLocked; set => _doorLocked = value; }

    private bool triggered;
    public bool Triggered { get => triggered; }
    private bool lockControl = false;

    [SerializeField]
    private string _doorLockedQuestion = "Question";
    public string DoorLockedQuestion { get => _doorLockedQuestion; set => _doorLockedQuestion = value; }

    [SerializeField]
    private int _doorLockedAnswer = 0;
    public int DoorLockedAnswer { get => _doorLockedAnswer; set => _doorLockedAnswer = value; }

    [SerializeField]
    private LockedDoorUIEventChannel manageDoorUI = default;

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
    }

    private void OnDisable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Triggered && DoorLocked)
        {
            manageDoorUI.InteractPressed();
        }
    }

    private void Update()
    {
        //TODO : Create a channel for DoorOpened event
        if (Triggered && !DoorLocked && !lockControl)
        {
            KapiAcildi.Invoke(this);
            lockControl = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manageDoorUI.ScriptTransfer(this);
            triggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            triggered = false;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            DoorLocked = DoorLocked,
            DoorLockedQuestion = DoorLockedQuestion,
            DoorLockedAnswer = DoorLockedAnswer
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        DoorLocked = saveData.DoorLocked;
        DoorLockedQuestion = saveData.DoorLockedQuestion;
        DoorLockedAnswer = saveData.DoorLockedAnswer;
    }

    [System.Serializable]
    struct SaveData
    {
        public bool DoorLocked;
        public string DoorLockedQuestion;
        public int DoorLockedAnswer;
    }
}
