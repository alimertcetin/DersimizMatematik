using System;
using System.Collections.Generic;
using GameCore.DoorSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.Utils;
using static UnityEditor.Progress;

namespace XIV.UI
{
    public class LockedDoor_UI : MonoBehaviour, PlayerControls.ILockedDoorUIActions
    {
        [Header("Broadcasting To")]
        [SerializeField] StringEventChannelSO WarningUIChannel = default;
        [SerializeField] private TMP_Text txt_InputField = null;
        [SerializeField] private TMP_Text txt_Soru = null;
        [SerializeField] private int textMaxLenght = 14;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] Timer deleteTimer = new Timer(0.3f);

        List<NumberItem> inputNumberItems = new List<NumberItem>();
        Door currentDoor;
        Inventory inventory;

        bool deleteStarted = false;

        private void Awake()
        {
            InputManager.PlayerControls.LockedDoorUI.SetCallbacks(this);
            inventoryLoadedChannel.Register(OnInventoryLoaded);
        }

        private void OnDestroy()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        }

        private void Update()
        {
            if (deleteStarted == false) return;

            if (deleteTimer.Update(Time.deltaTime))
            {
                Delete();
                deleteTimer.Restart();
            }
        }

        private void OnInventoryLoaded(Inventory inventory)
        {
            this.inventory = inventory;
        }

        public void Show(Door door)
        {
            currentDoor = door;
            txt_Soru.text = currentDoor.GetQuestionString();

            InputManager.LockedDoorUI.Enable();
            InputManager.GameManager.Disable();
            InputManager.GamePlay.Disable();
            this.gameObject.SetActive(true);
        }

        public void Close()
        {
            ClearTextCompletly();

            InputManager.LockedDoorUI.Disable();
            InputManager.GameManager.Enable();
            InputManager.GamePlay.Enable();
            this.gameObject.SetActive(false);
        }

        public void NumberOnClick(int value)
        {
            if (txt_InputField.text.Length > textMaxLenght)
            {
                ShowWarning("You cant enter anymore number");
                return;
            }

            bool IsExist(out int index)
            {
                index = -1;
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (inventory[i].Item is NumberItem item && item.Value == value)
                    {
                        index = i;
                        return true;
                    }
                }
                return false;
            }

            if (IsExist(out int index))
            {
                txt_InputField.text += value.ToString();
                inputNumberItems.Add(inventory[index].Item as NumberItem);
                int amount = 1;
                inventory.RemoveAt(index, ref amount);

            }
            else
            {
                ShowWarning("This number is not exists in your inventory!");
            }
        }

        public void ClearTextCompletly()
        {
            while (txt_InputField.text.Length != 0)
            {
                Delete();
            }
        }

        void Delete()
        {
            if (inputNumberItems.Count == 0) return;

            txt_InputField.text = txt_InputField.text.Remove(txt_InputField.text.Length - 1);
            int index = inputNumberItems.Count - 1;
            int amount = 1;
            inventory.TryAdd(inputNumberItems[index], ref amount);
            inputNumberItems.RemoveAt(index);
        }

        void Answer()
        {
            if (currentDoor.SolveQuestion(int.Parse(txt_InputField.text))) Close();
            else ShowWarning("Wrong answer");
        }

        private void ShowWarning(string text, bool value = true)
        {
            WarningUIChannel.RaiseEvent(text, value);
        }

        public void OnZero(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(0);
        }

        public void OnOne(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(1);
        }

        public void OnTwo(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(2);
        }

        public void OnThree(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(3);
        }

        public void OnFour(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(4);
        }

        public void OnFive(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(5);
        }

        public void OnSix(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(6);
        }

        public void OnSeven(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(7);
        }

        public void OnEight(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(8);
        }

        public void OnNine(InputAction.CallbackContext context)
        {
            if (context.performed) NumberOnClick(9);
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed) Answer();
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) Close();
        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Delete();
                deleteStarted = true;
            }
            else if (context.canceled)
            {
                deleteTimer.Restart();
                deleteStarted = false;
            }
        }
    }
}