using System.Collections.Generic;
using LessonIsMath.DoorSystems;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.Utils;

namespace LessonIsMath.UI
{
    public class LockedDoor_UI : GameUI, PlayerControls.IGameUIActions, IKeypadListener
    {
        [SerializeField] Keypad keypad;
        [SerializeField] CustomButton btn_Back;
        [SerializeField] float waitDeleteDuration;
        [SerializeField] float deleteDuration;
        Timer waitDeleteTimer;
        Timer deleteTimer;

        // TODO : Remove event channel dependency
        [SerializeField] DoorEventChannelSO lockedDoorUIChannel = default;
        [SerializeField] StringEventChannelSO warningUIChannel = default;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        Inventory inventory;
        bool deleteStarted = false;

        [SerializeField] TMP_Text txt_InputField = null;
        [SerializeField] TMP_Text txt_Soru = null;
        [SerializeField] int textMaxLenght = 14;

        List<NumberItem> inputNumberItems = new List<NumberItem>();
        Door currentDoor;

        protected override void Awake()
        {
            base.Awake();
            keypad.SetListener(this);
        }

        void Update()
        {
            if (deleteStarted == false || waitDeleteTimer.Update(Time.deltaTime) == false || deleteTimer.Update(Time.deltaTime) == false) return;

            Delete();
            var newDuration = deleteTimer.Duration - 0.025f;
            newDuration = newDuration < 0 ? 0 : newDuration;
            deleteTimer = new Timer(newDuration);
        }

        void OnEnable()
        {
            btn_Back.RegisterOnClick(() => lockedDoorUIChannel.RaiseEvent(null, false));
            inventoryLoadedChannel.Register(OnInventoryLoaded);
        }

        void OnDisable()
        {
            btn_Back.UnregisterOnClick();
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        }

        void OnInventoryLoaded(Inventory obj)
        {
            this.inventory = obj;
        }

        public void SetDoor(Door door)
        {
            this.currentDoor = door;
        }

        public override void Show()
        {
            base.Show();
            keypad.Enable();
            txt_Soru.text = currentDoor.GetQuestionString();
            InputManager.GameUI.Enable();
            InputManager.GameUI.SetCallbacks(this);
            InputManager.Keypad.Enable();
            InputManager.GameState.Disable();
            InputManager.CharacterMovement.Disable();
        }

        public override void Hide()
        {
            base.Hide();
            keypad.Disable();
            ClearTextCompletly();

            InputManager.GameUI.Disable();
            InputManager.Keypad.Disable();
            InputManager.GameState.Enable();
            InputManager.CharacterMovement.Enable();
        }

        void OnNumberButtonClicked(int value)
        {
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

            if (txt_InputField.text.Length > textMaxLenght)
            {
                warningUIChannel.RaiseEvent("You cant enter anymore number", true);
                return;
            }

            if (IsExist(out int index) == false)
            {
                warningUIChannel.RaiseEvent("This number is not exists in your inventory!", true);
                return;
            }

            txt_InputField.text += value.ToString();
            inputNumberItems.Add(inventory[index].Item as NumberItem);
            int amount = 1;

            inventory.RemoveAt(index, ref amount);
        }

        void ClearTextCompletly()
        {
            while (inputNumberItems.Count != 0)
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
            if (currentDoor.SolveQuestion(int.Parse(txt_InputField.text)) == false)
            {
                warningUIChannel.RaiseEvent("Wrong answer", true);
                return;
            }

            txt_InputField.text = "";
            inputNumberItems.Clear();
            lockedDoorUIChannel.RaiseEvent(null, false);
        }

        void PlayerControls.IGameUIActions.OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) lockedDoorUIChannel.RaiseEvent(null, false);
        }

        void IKeypadListener.OnEnter() => Answer();
        void IKeypadListener.OnDeleteStarted()
        {
            Delete();
            deleteStarted = true;
            waitDeleteTimer = new Timer(waitDeleteDuration);
            deleteTimer = new Timer(deleteDuration);
        }
        void IKeypadListener.OnDeleteCanceled()
        {
            deleteStarted = false;
        }
        void IKeypadListener.OnNumberPressed(int value) => OnNumberButtonClicked(value);
    }
}