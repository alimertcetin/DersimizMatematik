using System.Collections.Generic;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.SaveSystems;

[RequireComponent(typeof(SaveableEntity))]
public class ObjectBasedEvents : MonoBehaviour, ISaveable
{
    [Header("Broadcasting To")]
    [SerializeField]
    private StringEventChannelSO WarningUIChannel = default;

    public enum Event_Enum { None, Enable, Disable, ColliderTrigger_True, ColliderTrigger_False, DestroyThisAfterEvents }

    [Tooltip("Triggerlandığında tanımlanan eventleri yerine getirir ve uyarı verilecek işaretliyse uyarı verir.")]
    [SerializeField] private bool useTriggerEnter = false;

    [Tooltip("İliştirilen gameobject bir trigger collider'a sahip olmalı ve bu collider character tarafından" +
        " triggerlanmış ve o süre içerisinde F basılmış olmalıdır.")]
    [SerializeField] private bool performnWhenInteractionPressed = false;

    [Tooltip("Create an event. If you will call DestroyThisAfterEvents assign it to the last index.")]
    [SerializeField] private List<Event_Enum> eventList = null;

    [Header("Add objects that will get effected by corresponding the event.")]
    [SerializeField] private List<GameObject> Enable_Objects = null;
    [SerializeField] private List<GameObject> Disable_Objects = null;
    [SerializeField] private List<GameObject> Collider_Trigger_Objects = null;

    [Tooltip("Not : Destroy seçildiği zaman nesne yok edilmez. Deaktif hale getirilir.")]
    [SerializeField] private bool uyariVerilecek = false;

    private bool TriggerEntered;
    [SerializeField] private string UyariText = "WARNING";

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
    }

    private void OnDisable()
    {
        TriggerEntered = false;
        InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
    }

    private void Update()
    {
        if (TriggerEntered && useTriggerEnter)
        {
            if (uyariVerilecek)
            {
                UyariVer(UyariText, true);
            }
            if (eventList != null)
            {
                HandleEvents();
            }
        }
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (TriggerEntered && performnWhenInteractionPressed)
        {
            if (uyariVerilecek)
            {
                UyariVer(UyariText, true);
            }
            if (eventList != null)
            {
                HandleEvents();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerEntered = false;
        }
    }

    public void HandleEvents()
    {
        foreach (Event_Enum @event in eventList)
        {
            if (@event == Event_Enum.Enable) { EnableObjects(); }
            else if (@event == Event_Enum.Disable) { DisableObjects(); }
            else if (@event == Event_Enum.ColliderTrigger_True) { ColliderIsTrigger(true); }
            else if (@event == Event_Enum.ColliderTrigger_False) { ColliderIsTrigger(false); }

            else if (@event == Event_Enum.DestroyThisAfterEvents) { gameObject.SetActive(false); }
        }
    }

    private void ColliderIsTrigger(bool triggered)
    {
        foreach (GameObject item in Collider_Trigger_Objects)
        {
            Collider col = item.GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = triggered;
            }

            for (int i = 0; i < item.transform.childCount; i++)
            {
                Collider childCol = item.GetComponentInChildren<Collider>();
                if (childCol != null)
                {
                    childCol.isTrigger = triggered;
                }
            }
        }
    }

    private void DisableObjects()
    {
        foreach (GameObject item in Disable_Objects)
        {
            item.SetActive(false);
        }
    }

    private void EnableObjects()
    {
        foreach (GameObject item in Enable_Objects)
        {
            item.SetActive(true);
        }
    }

    public void UyariVer(string text, bool value)
    {
        WarningUIChannel.RaiseEvent(text, value);
    }

    public object CaptureState()
    {
        bool ColTriggerState = false;
        TryGetComponent<Collider>(out Collider col);
        if (col != null)
        {
            ColTriggerState = col.isTrigger;
        }

        return new SaveData
        {
            _isActive = gameObject.activeSelf,
            _isTriggered = ColTriggerState,
        };
    }

    public void RestoreState(object state)
    {
        TryGetComponent<Collider>(out Collider col);
        SaveData saveData = (SaveData)state;
        gameObject.SetActive(saveData._isActive);
        if (col != null)
        {
            col.isTrigger = saveData._isTriggered;
        }
    }

    [System.Serializable]
    private struct SaveData
    {
        public bool _isActive;
        public bool _isTriggered;
    }
}
