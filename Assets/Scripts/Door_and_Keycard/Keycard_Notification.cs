using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Keycard_Notification : MonoBehaviour
{
    [SerializeField]
    private StringEventChannelSO notificationChannel = default;
    private Keycard keycardScript;

    private const string YESIL_TEXT = "<color=green><b>Yeşil</b></color>";
    private const string SARI_TEXT = "<color=yellow><b>Sarı</b></color>";
    private const string KIRMIZI_TEXT = "<color=red><b>Kırmızı</b></color>";

    private void Awake()
    {
        keycardScript = GetComponent<Keycard>();
    }

    private void OnEnable()
    {
        keycardScript.KeycardCollected += _keycardScript_KeycardCollected;
    }

    private void OnDisable()
    {
        keycardScript.KeycardCollected -= _keycardScript_KeycardCollected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            notificationChannel.RaiseEvent(SetKeycardText());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            notificationChannel.RaiseEvent("", false);
        }
    }

    private void _keycardScript_KeycardCollected()
    {
        notificationChannel.RaiseEvent("", false);
    }

    private string SetKeycardText()
    {
        string keycardType = "";

        if (keycardScript.KeycardType == Door_and_Keycard_Level.Yesil)
        {
            keycardType = YESIL_TEXT;
        }
        else if (keycardScript.KeycardType == Door_and_Keycard_Level.Sari)
        {
            keycardType = SARI_TEXT;
        }
        else if (keycardScript.KeycardType == Door_and_Keycard_Level.Kirmizi)
        {
            keycardType = KIRMIZI_TEXT;
        }
        else
        {
            Debug.LogWarning(name + " Keycard türüne ulaşamadı.");
        }

        return keycardType + " Keycard'ı toplamak için " + InputManager.InteractionKeyName
            + " tuşuna bas.";
    }

}
