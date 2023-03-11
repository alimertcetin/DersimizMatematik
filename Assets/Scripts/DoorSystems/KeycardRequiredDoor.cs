using System;
using UnityEngine;
using XIV.Extensions;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.SaveSystems;

namespace LessonIsMath.DoorSystems
{
    [RequireComponent(typeof(DoorManager))]
    public class KeycardRequiredDoor : MonoBehaviour, ISaveable
    {
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] KeycardItemSO[] requiredKeycards;
        bool[] removedKeycards;
        Inventory inventory;
        DoorManager doorManager;

        void Awake()
        {
            removedKeycards = new bool[requiredKeycards.Length];
            doorManager = GetComponent<DoorManager>();
        }

        void OnEnable() => inventoryLoadedChannel.Register(OnInventoryLoaded);
        void OnDisable() => inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        void OnInventoryLoaded(Inventory inventory) => this.inventory = inventory;

        public void OnInteract()
        {
            if (IsKeycardRequired() == false)
            {
                doorManager.OnInteractionEnd();
                return;
            }

            for (int i = 0; i < removedKeycards.Length; i++)
            {
                if (removedKeycards[i] || inventory.Contains(requiredKeycards[i].GetItem(), out var index) == false) continue;

                removedKeycards[i] = true;
                int amount = 1;
                inventory.RemoveAt(index, ref amount);
                break;
            }
            doorManager.OnInteractionEnd();
        }

        public bool IsKeycardRequired()
        {
            int length = requiredKeycards.Length;
            for (int i = 0; i < length; i++)
            {
                if (removedKeycards[i] == false) return true;
            }
            return false;
        }
        
        public void CountKeycards(out int greenCount, out int yellowCount, out int redCount)
        {
            greenCount = 0;
            yellowCount = 0;
            redCount = 0;
            for (int i = 0; i < removedKeycards.Length; i++)
            {
                if (removedKeycards[i]) continue;
                
                switch (requiredKeycards[i].item.KeycardType)
                {
                    case KeycardType.Green:
                        greenCount++;
                        break;
                    case KeycardType.Yellow:
                        yellowCount++;
                        break;
                    case KeycardType.Red:
                        redCount++;
                        break;
                    default: throw new NotImplementedException(requiredKeycards[i].item.KeycardType + " is not implemented.");
                }
            }
        }

        public string GetKeycardString()
        {
            if (IsKeycardRequired() == false) return "";
            CountKeycards(out int greenCount, out int yellowCount, out int redCount);

            string str = "You need ";
            if (greenCount > 0) str += $"{greenCount} green".Green();
            if (yellowCount > 0) str += $" {yellowCount} yellow".Yellow();
            if (redCount > 0) str += $" {redCount} red".Red();

            var total = greenCount + yellowCount + redCount;
            if (total < 2) str += " keycard";
            else str += " keycards";

            return total > 0 ? str : "";
        }
        
        #region --- Save ---

        [Serializable]
        struct SaveData
        {
            public bool[] removedKeycards;
        }

        object ISaveable.CaptureState()
        {
            return new SaveData
            {
                removedKeycards = removedKeycards,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            removedKeycards = saveData.removedKeycards;
            GetComponent<DoorManager>().RefreshDoorState();
        }

        #endregion
    }
}