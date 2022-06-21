using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Door_Status))]
[DisallowMultipleComponent]
public class DoorKeycard_Management : MonoBehaviour, ISaveable
{
    private Door_Notification DoorNotification = default;

    public Action AllKeycardsRemoved;
    public Action KeycardRemoved;

    [SerializeField]
    private List<Door_and_Keycard_Level> gerekenKeycardlar = null;
    public List<Door_and_Keycard_Level> GerekenKeycardlar { get => gerekenKeycardlar; private set { } }

    private Door_Is_Locked Door_Is_Locked_Script;
    private PlayerInventory inventory;

    private bool yesil;
    private bool sari;
    private bool kirmizi;
    private bool triggered;
    private bool doorOpened;
    public bool DoorOpened { get => doorOpened; set => doorOpened = value; }

    private const string WARNING_TEXT = "Gereken Keycardlar olmadan bu kapıyı açamazsın.";
    private const string YESIL_RENKLIYAZDIR = "<b><color=green>Yeşil</color></b>";
    private const string SARI_RENKLIYAZDIR = "<b><color=yellow>Sarı</color></b>";
    private const string KIRMIZI_RENKLIYAZDIR = "<b><color=red>Kırmızı</color></b>";

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
    }

    private void OnDisable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
    }

    private void Awake()
    {
        inventory = FindObjectOfType<PlayerInventory>();

        DoorNotification = GetComponent<Door_Notification>();
        Door_Is_Locked_Script = GetComponent<Door_Is_Locked>();

        if (Door_Is_Locked_Script != null)
        {
            Door_Is_Locked_Script.KapiAcildi += Door_Is_Locked_Kapi_Acildi;
        }
        else
        {
            DoorOpened = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggered && gerekenKeycardlar.Count == 0)
        {
            AllKeycardsRemoved?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
        }
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (triggered && doorOpened && gerekenKeycardlar.Count != 0)
        {
            RemoveKeycardFromDoor();
            KeycardRemoved.Invoke();
        }
    }

    private void Door_Is_Locked_Kapi_Acildi(Door_Is_Locked door)
    {
        doorOpened = !door.DoorLocked;
    }

    private void RemoveKeycardFromDoor()
    {
        GetKeycardName(out Door_and_Keycard_Level _keycard, out string _typeName);

        if (inventory.KeycardCikar_Success(_typeName))
        {
            gerekenKeycardlar.Remove(_keycard);
        }
        else
        {
            DoorNotification.Warn(WARNING_TEXT);
        }
    }

    private void GetKeycardName(out Door_and_Keycard_Level KeycardType, out string TypeName)
    {
        KeycardType = Door_and_Keycard_Level.None;
        TypeName = "None";
        foreach (Door_and_Keycard_Level item in gerekenKeycardlar)
        {
            switch (item)
            {
                case Door_and_Keycard_Level.Yesil:
                    KeycardType = item;
                    TypeName = "green";
                    break;
                case Door_and_Keycard_Level.Sari:
                    KeycardType = item;
                    TypeName = "yellow";
                    break;
                case Door_and_Keycard_Level.Kirmizi:
                    KeycardType = item;
                    TypeName = "red";
                    break;
                default:
                    break;
            }
        }
    }

    public string Door_Keycard_NotificationText()
    {
        string str = "Bu kapıyı açmak için ";

        CountKeycards(out int Yesil_count, out int Sari_count, out int Kirmizi_count);

        if (Yesil_count != 0)
        {
            str += $"{Yesil_count} tane {YESIL_RENKLIYAZDIR} ";
        }

        if (Sari_count != 0)
        {
            str += $"{Sari_count} tane {SARI_RENKLIYAZDIR} ";
        }

        if (Kirmizi_count != 0)
        {
            str += $"{Kirmizi_count} tane {KIRMIZI_RENKLIYAZDIR} ";
        }

        if (Yesil_count != 0 || Sari_count != 0 || Kirmizi_count != 0)
        {
            return str += "Keycard gerekli.";
        }
        else
        {
            return "";
        }
    }

    private void CountKeycards(out int yesilCount, out int sariCount, out int kirmiziCount)
    {
        yesilCount = 0;
        sariCount = 0;
        kirmiziCount = 0;
        foreach (Door_and_Keycard_Level item in gerekenKeycardlar)
        {
            if(item == Door_and_Keycard_Level.Yesil)
            {
                yesilCount += 1;
            }
            else if(item == Door_and_Keycard_Level.Sari)
            {
                sariCount += 1;
            }
            else if(item == Door_and_Keycard_Level.Kirmizi)
            {
                kirmiziCount += 1;
            }
        }
    }

    #region -_- Save -_-

    public object CaptureState()
    {
        return new SaveData
        {
            _KeycardsAreRemoved = gerekenKeycardlar == null || gerekenKeycardlar.Count == 0 ? true : false,
            _DoorOpened = DoorOpened,
            _GerekenKeycardlar = GerekenKeycardlar
        };
    }

    public void RestoreState(object state)
    {
        SaveData saveData = (SaveData)state;
        if (saveData._KeycardsAreRemoved)
        {
            gerekenKeycardlar = null;
        }
        DoorOpened = saveData._DoorOpened;
        GerekenKeycardlar = saveData._GerekenKeycardlar;
    }

    [System.Serializable]
    private struct SaveData
    {
        public bool _KeycardsAreRemoved;
        public bool _DoorOpened;
        public List<Door_and_Keycard_Level> _GerekenKeycardlar;
    }

    #endregion

}
