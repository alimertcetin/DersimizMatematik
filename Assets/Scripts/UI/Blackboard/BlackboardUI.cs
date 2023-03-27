﻿using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.UI.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;

namespace LessonIsMath.UI
{
    public class BlackboardUI : ParentGameUI, PlayerControls.IGameUIActions
    {
        // TODO : Remove channel dependency
        [SerializeField] BoolEventChannelSO blackBoardUIChannel = default;
        [SerializeField] EarnNumberPage earnNumberPage;
        [SerializeField] MakeOperationPage makeOperationPage;
        [SerializeField] CustomButton btn_EarnNumber;
        [SerializeField] CustomButton btn_MakeOperation;
        [SerializeField] CustomButton btn_Exit;
        [SerializeField] NumberItemSO[] numberItems;
        [SerializeField] StringEventChannelSO warningUIChannel = default;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;

        Inventory inventory;

        protected override void Awake()
        {
            base.Awake();
            earnNumberPage.SetBlackboard(this);
            makeOperationPage.SetBlackboard(this);
        }

        void OnEnable()
        {
            inventoryLoadedChannel.Register(OnInventoryLoaded);
        }

        void OnDisable()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        }

        void OnInventoryLoaded(Inventory obj) => this.inventory = obj;

        public override void Show()
        {
            base.Show();
            btn_Exit.RegisterOnClick(() => blackBoardUIChannel.RaiseEvent(false));

            btn_EarnNumber.onClick.AddListener(ShowEarnNumberUI);
            btn_MakeOperation.onClick.AddListener(ShowMakeOperationUI);
            InputManager.GameUI.SetCallbacks(this);
            InputManager.GameUI.Enable();
            InputManager.GameState.Disable();
        }

        public override void Hide()
        {
            base.Hide();
            btn_Exit.UnregisterOnClick();

            btn_EarnNumber.onClick.RemoveListener(ShowEarnNumberUI);
            btn_MakeOperation.onClick.RemoveListener(ShowMakeOperationUI);

            earnNumberPage.Hide();
            makeOperationPage.Hide();

            InputManager.GameUI.Disable();
            InputManager.GameState.Enable();
        }

        void ShowEarnNumberUI()
        {
            InputManager.GameUI.Disable();
            OpenPage(earnNumberPage);
        }

        void ShowMakeOperationUI()
        {
            InputManager.GameUI.Disable();
            OpenPage(makeOperationPage);
        }

        public override void ComeBack(PageUI from)
        {
            base.ComeBack(from);
            InputManager.GameUI.Enable();
        }

        void PlayerControls.IGameUIActions.OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) blackBoardUIChannel.RaiseEvent(false);
        }

        public bool IsNumberExistsInInventory(int number)
        {
            for (int i = 0; i < inventory.SlotCount; i++)
            {
                if (inventory[i].Item is NumberItem item && item.Value == number)
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowWarning(string text)
        {
            warningUIChannel.RaiseEvent(text);
        }

        public bool AddNumberToInventory(int number)
        {
            var item = numberItems[number].GetItem();
            int amount = 1;
            if (inventory.CanAdd(item, amount) == false || (inventory.Contains(item, out var itemIndex) && inventory[itemIndex].Amount >= item.StackableAmount))
            {
                return false;
            }
            return inventory.TryAdd(item, ref amount);
        }

        public void RemoveNumberFromInventory(int number)
        {
            int amount = 1;
            for (int i = 0; i < inventory.SlotCount; i++)
            {
                if (inventory[i].Item is NumberItem item && item.Value == number)
                {
                    inventory.RemoveAt(i, ref amount);
                    break;
                }
            }
        }
    }
}
