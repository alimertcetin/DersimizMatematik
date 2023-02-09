using System;
using GameCore.ScriptableObjects.DoorData;
using UnityEngine;
using XIV.Extensions;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.SaveSystem;
using XIV.Utils;

namespace GameCore.DoorSystem
{
    public class Door : MonoBehaviour, ISaveable
    {
        [SerializeField] StringEventChannelSO notificationChannel = default;
        [SerializeField] KeycardItemSO[] requiredKeycards;
        [SerializeField] DoorQuestionSO doorQuestionSO;

        DoorAnimation doorAnimation;
        bool isLocked;
        bool triggered = false;
        bool[] removedKeycards;
        bool isOpen;

        private void Awake()
        {
            removedKeycards = new bool[requiredKeycards.Length];
            doorAnimation = GetComponent<DoorAnimation>();
            isLocked = doorQuestionSO == null ? false : isLocked;
        }

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
            if (triggered == false) return;
            // TODO : Complete
        }

        public bool SolveQuestion(int answer)
        {
            if (doorQuestionSO.arithmeticOperation.Answer == answer)
            {
                this.isLocked = false;
                return true;
            }
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") == false) return;

            triggered = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (triggered == false) return;

            RefreshNotification();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") == false) return;

            triggered = false;
            notificationChannel.RaiseEvent(null, false);
        }

        void CountKeycards(out int greenCount, out int yellowCount, out int redCount)
        {
            greenCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Green);
            yellowCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Yellow);
            redCount = ArrayUtils.Count(requiredKeycards, (itemSO) => itemSO.item.KeycardType == KeycardType.Red);
        }

        bool IsRemovedAllKeycards()
        {
            int length = removedKeycards.Length;
            for (int i = 0; i < length; i++)
            {
                if (removedKeycards[i] == false) return false;
            }
            return true;
        }

        void RefreshNotification()
        {
            if (InputManager.PlayerControls.LockedDoorUI.enabled)
            {
                notificationChannel.RaiseEvent(null, false);
            }
            else if (IsRemovedAllKeycards() == false)
            {
                notificationChannel.RaiseEvent(GetKeycardNotificationString());
            }
            else if (isLocked == false)
            {
                notificationChannel.RaiseEvent(GetInteractionString(isOpen));
            }
            else if (isLocked)
            {
                notificationChannel.RaiseEvent("Door is locked. Press " + InputManager.InteractionKeyName + " button to see the Question.");
            }
        }

        string GetInteractionString(bool isDoorOpen)
        {
            return isDoorOpen
                ? "Press " + InputManager.InteractionKeyName + " to Close"
                : "Press " + InputManager.InteractionKeyName + " to Open";
        }

        string GetKeycardNotificationString()
        {
            CountKeycards(out int greenCount, out int yellowCount, out int redCount);

            static string Format(int amount, string color) => $"{amount} " + color + " keycard ";

            string str = "You need ";

            if (greenCount > 0) str += Format(greenCount, "green".Green());
            if (yellowCount > 0) str += Format(yellowCount, "yellow".Yellow());
            if (redCount > 0) str += Format(redCount, "red".Red());

            return greenCount > 0 || yellowCount > 0 || redCount > 0 ? str : "";
        }

        public string GetQuestionString()
        {
            return doorQuestionSO.arithmeticOperation.ToString();
        }

        #region -_- Save -_-

        [Serializable]
        private struct SaveData
        {
            public bool isLocked;
            public bool isOpen;
            public bool[] removedKeycards;
        }

        object ISaveable.CaptureState()
        {
            return new SaveData
            {
                isLocked = this.isLocked,
                isOpen = this.isOpen,
                removedKeycards = this.removedKeycards,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            this.removedKeycards = saveData.removedKeycards;
            this.isOpen = saveData.isOpen;
            this.isLocked = saveData.isLocked;

            doorAnimation.Interact(isOpen);
        }

        #endregion


    }
}