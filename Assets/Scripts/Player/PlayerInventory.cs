using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class PlayerInventory : MonoBehaviour, ISaveable
{
    [SerializeField] private VoidEventChannelSO OnInventoryLoaded = default;
    [SerializeField] private PlayerInventorySO inventorySO = default;

    public int Inventory_Capacity { get => inventorySO.Capacity; }

    private void Awake()
    {
        OnInventoryLoaded.RaiseEvent();
    }

    public bool InventoryControl_Sayi(int value)
    {
        return inventorySO.InventoryControl_Sayi(value);
    }

    /// <summary>
    /// Keycard envantere eklendiyse true döndürür.
    /// </summary>
    public bool KeycardEkle(Door_and_Keycard_Level keycard)
    {
        return inventorySO.KeycardEkle(keycard);
    }

    /// <summary>
    /// Keycard'ı envanterden kaldırmayı dener. Başarılıysa true döndürür.
    /// </summary>
    /// <param name="keycardColor">green, red, yellow</param>
    public bool KeycardCikar_Success(string keycardColor)
    {
        return inventorySO.KeycardCikar_Success(keycardColor);
    }

    /// <summary>
    /// İşlem başarılıysa true döndürür.
    /// </summary>
    public bool Sayi_Ekle(int eklenecekSayi, int eklenecekMiktar)
    {
        return inventorySO.Sayi_Ekle(eklenecekSayi, eklenecekMiktar);
    }

    /// <summary>
    /// İşlem başarılıysa true döndürür.
    /// </summary>
    public bool Sayi_Cikar(int cikarilacakSayi, int cikarilacakMiktar)
    {
        return inventorySO.Sayi_Cikar(cikarilacakSayi, cikarilacakMiktar);
    }

    public object CaptureState()
    {
        return new SaveData
        {
            Rakam_0 = inventorySO.Rakam_0,
            Rakam_1 = inventorySO.Rakam_1,
            Rakam_2 = inventorySO.Rakam_2,
            Rakam_3 = inventorySO.Rakam_3,
            Rakam_4 = inventorySO.Rakam_4,
            Rakam_5 = inventorySO.Rakam_5,
            Rakam_6 = inventorySO.Rakam_6,
            Rakam_7 = inventorySO.Rakam_7,
            Rakam_8 = inventorySO.Rakam_8,
            Rakam_9 = inventorySO.Rakam_9,
            yesilKeycard = inventorySO.yesilKeycard,
            sariKeycard = inventorySO.sariKeycard,
            kirmiziKeycard = inventorySO.kirmiziKeycard,
            inventoryCapacity = inventorySO.Capacity
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        inventorySO.Rakam_0 = saveData.Rakam_0;
        inventorySO.Rakam_1 = saveData.Rakam_1;
        inventorySO.Rakam_2 = saveData.Rakam_2;
        inventorySO.Rakam_3 = saveData.Rakam_3;
        inventorySO.Rakam_4 = saveData.Rakam_4;
        inventorySO.Rakam_5 = saveData.Rakam_5;
        inventorySO.Rakam_6 = saveData.Rakam_6;
        inventorySO.Rakam_7 = saveData.Rakam_7;
        inventorySO.Rakam_8 = saveData.Rakam_8;
        inventorySO.Rakam_9 = saveData.Rakam_9;

        inventorySO.yesilKeycard = saveData.yesilKeycard;
        inventorySO.sariKeycard = saveData.sariKeycard;
        inventorySO.kirmiziKeycard = saveData.kirmiziKeycard;

        inventorySO.Capacity = saveData.inventoryCapacity;

        inventorySO.InventoryChanged_Number.Invoke();
        inventorySO.InventoryChanged_Keycard.Invoke();
    }

    [System.Serializable]
    struct SaveData
    {
        public int Rakam_0;
        public int Rakam_1;
        public int Rakam_2;
        public int Rakam_3;
        public int Rakam_4;
        public int Rakam_5;
        public int Rakam_6;
        public int Rakam_7;
        public int Rakam_8;
        public int Rakam_9;

        public int yesilKeycard;
        public int sariKeycard;
        public int kirmiziKeycard;

        public int inventoryCapacity;
    }
}
