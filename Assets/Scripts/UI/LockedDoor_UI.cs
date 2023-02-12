using System;
using System.Collections.Generic;
using LessonIsMath.DoorSystems;
using LessonIsMath.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.InventorySystem.Items;

namespace LessonIsMath.UI
{
    public class LockedDoor_UI : OperationUI, PlayerControls.ILockedDoorUIActions
    {
        [SerializeField] GameObject lockedDoorUI;
        [Header("Broadcasting To")]
        [SerializeField] private TMP_Text txt_InputField = null;
        [SerializeField] private TMP_Text txt_Soru = null;
        [SerializeField] private int textMaxLenght = 14;

        List<NumberItem> inputNumberItems = new List<NumberItem>();
        Door currentDoor;
        public bool isActive { get; private set; }

        private void Awake()
        {
            InputManager.PlayerControls.LockedDoorUI.SetCallbacks(this);
        }

        public override void ShowUI() => throw new NotImplementedException();

        public void ShowUI(Door door)
        {
            isActive = true;
            currentDoor = door;
            txt_Soru.text = currentDoor.GetQuestionString();

            InputManager.LockedDoorUI.Enable();
            InputManager.GameManager.Disable();
            InputManager.GamePlay.Disable();
            lockedDoorUI.SetActive(true);
        }

        public override void CloseUI()
        {
            isActive = false;
            ClearTextCompletly();

            InputManager.LockedDoorUI.Disable();
            InputManager.GameManager.Enable();
            InputManager.GamePlay.Enable();
            lockedDoorUI.SetActive(false);
        }

        protected override void OnNumberButtonClicked(int value)
        {
            if (txt_InputField.text.Length > textMaxLenght)
            {
                warningUIChannel.RaiseEvent("You cant enter anymore number", true);
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
                warningUIChannel.RaiseEvent("This number is not exists in your inventory!", true);
            }
        }

        public void ClearTextCompletly()
        {
            while (txt_InputField.text.Length != 0)
            {
                Delete();
            }
        }

        protected override void Delete()
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
            if (currentDoor.SolveQuestion(int.Parse(txt_InputField.text))) CloseUI();
            else warningUIChannel.RaiseEvent("Wrong answer", true);
        }

        public void OnZero(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(0);
        }

        public void OnOne(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(1);
        }

        public void OnTwo(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(2);
        }

        public void OnThree(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(3);
        }

        public void OnFour(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(4);
        }

        public void OnFive(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(5);
        }

        public void OnSix(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(6);
        }

        public void OnSeven(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(7);
        }

        public void OnEight(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(8);
        }

        public void OnNine(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(9);
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed) Answer();
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) CloseUI();
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