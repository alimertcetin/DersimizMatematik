using System;
using System.Collections.Generic;
using ElRaccoone.Tweens;
using LessonIsMath.DoorSystems;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.Tween;
using LessonIsMath.UI.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.InventorySystem.ScriptableObjects.NonSerializedData;
using XIV.InventorySystem.UI;

namespace LessonIsMath.UI
{
    public class KeycardUI : GameUI, PlayerControls.IGameUIActions
    {
        [SerializeField] NonSerializedItemDataContainerSO ItemDataContainerSO;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] InventoryChangeChannelSO inventoryChangedChannel;
        [SerializeField] BoolEventChannelSO keycardUIChannel;
        [SerializeField] StringEventChannelSO warningChannel;
        [SerializeField] CustomButton insertCardButton;
        [SerializeField] GameObject insertKeycardIcon;
        [SerializeField] GameObject cardModelGreen;
        [SerializeField] GameObject cardModelYellow;
        [SerializeField] GameObject cardModelRed;

        Vector3 initialPosition;
        GameObject currentCardGo;
        Inventory inventory;
        InventorySlot[] slots;
        CustomButton[] keycardButtons;
        KeycardRequiredDoor keycardRequiredDoor;
        bool spamLock;
        bool isInserting;
        KeycardItem currentKeycardItem;
        
        protected override void Awake()
        {
            base.Awake();
            slots = uiGameObject.GetComponentInChildren<HorizontalLayoutGroup>().GetComponentsInChildren<InventorySlot>();
            keycardButtons = new CustomButton[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                keycardButtons[i] = slots[i].GetComponent<CustomButton>();
            }

            initialPosition = insertKeycardIcon.transform.position;
        }

        void OnEnable()
        {
            inventoryLoadedChannel.Register(OnInventoryLoaded);
            inventoryChangedChannel.Register(OnInventoryChanged);
            insertCardButton.RegisterOnClick(InsertKeycard);
            for (int i = 0; i < keycardButtons.Length; i++)
            {
                int slotID = i;
                keycardButtons[i].RegisterOnClick(() => OnKeycardButtonClick(slotID));
            }
        }

        void OnDisable()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
            inventoryChangedChannel.Unregister(OnInventoryChanged);
            insertCardButton.UnregisterOnClick();
            for (int i = 0; i < keycardButtons.Length; i++)
            {
                keycardButtons[i].UnregisterOnClick();
            }
        }

        public override void Show()
        {
            base.Show();
            InputManager.GameUI.SetCallbacks(this);
            InputManager.GameUI.Enable();
            InputManager.GameState.Disable();
        }

        public override void Hide()
        {
            base.Hide();
            InputManager.GameUI.Disable();
            InputManager.GameState.Enable();
            insertKeycardIcon.TweenCancelAll();
            insertKeycardIcon.SetActive(false);
        }
        
        void StartIconTween(Vector3 initialPosition, Vector3 movement)
        {
            insertKeycardIcon.TweenPosition(initialPosition + movement, 0.5f)
                .SetOnComplete(() =>
                {
                    insertKeycardIcon.TweenPosition(initialPosition - movement, 0.5f)
                        .SetOnComplete(() => StartIconTween(initialPosition, movement));
                });
        }

        void OnInventoryLoaded(Inventory inventory)
        {
            this.inventory = inventory;
            RefreshSlots();
        }

        void OnInventoryChanged(InventoryChange inventoryChange)
        {
            for (int i = 0; i < inventoryChange.ChangeCount; i++)
            {
                if (inventoryChange.ChangedItems[i].ChangedItem.Item is KeycardItem)
                {
                    RefreshSlots();
                    break;
                }
            }
        }

        void InsertKeycard()
        {
            if (isInserting || spamLock) return;

            insertKeycardIcon.SetActive(false);
            if (currentKeycardItem == null)
            {
                warningChannel.RaiseEvent("Select a card");
                return;
            }

            int indexOfItem = keycardRequiredDoor.GetIndexOfRequiredItem(currentKeycardItem);
            isInserting = indexOfItem > -1;

            if (isInserting == false)
            {
                warningChannel.RaiseEvent("Card reader doesnt need this keycard");
                return;
            }

            insertCardButton.image.color = Color.gray;
            currentCardGo.transform.MoveTowardsTween(currentCardGo.transform.position + (currentCardGo.transform.right * 0.28f), 1.5f, () =>
            {
                keycardRequiredDoor.RemoveAt(indexOfItem);
                isInserting = false;
                int amount = 1;
                inventory.Remove(currentKeycardItem, ref amount);
                insertCardButton.image.color = Color.white;
                currentKeycardItem = null;
                if (keycardRequiredDoor.IsKeycardRequired() == false)
                {
                    SetKeycard(KeycardType.None);
                    keycardUIChannel.RaiseEvent(false);
                }
            });
            currentCardGo.transform.ScaleTween(Vector3.zero, 1f);
        }

        void OnKeycardButtonClick(int slotIndex)
        {
            if (spamLock) return;

            spamLock = true;
            InventorySlot slot = slots[slotIndex];
            currentKeycardItem = (KeycardItem)slot.inventoryItem.Item;
            SetKeycard(currentKeycardItem.KeycardType);

            void SetButtonColors(Color color)
            {
                for (int i = 0; i < keycardButtons.Length; i++)
                {
                    keycardButtons[i].image.color = color;
                }

                insertCardButton.image.color = color;
            }

            SetButtonColors(Color.gray);
            var cardReader = keycardRequiredDoor.GetCardReader();
            Transform cardReaderTransform = cardReader.transform;
            Vector3 offset = cardReader.GetCardInsertPosition() - cardReaderTransform.position - (currentCardGo.transform.right * 0.125f);
            currentCardGo.transform.MoveTowardsTween(cardReaderTransform, offset, 5f, () =>
            {
                currentCardGo.transform.SetParent(Camera.main.transform);
                spamLock = false;
                SetButtonColors(Color.white);
                bool activate = keycardRequiredDoor.GetIndexOfRequiredItem(currentKeycardItem) > -1;
                insertKeycardIcon.SetActive(activate);
                if (activate)
                {
                    insertKeycardIcon.TweenCancelAll();
                    StartIconTween(initialPosition, Vector3.up * 5f);
                }
            });
        }

        void RefreshSlots()
        {
            IList<ReadOnlyInventoryItem> greenKeycards = inventory.GetItemsOfType<KeycardItem>((item) => item.KeycardType == KeycardType.Green);
            IList<ReadOnlyInventoryItem> yellowKeycards = inventory.GetItemsOfType<KeycardItem>((item) => item.KeycardType == KeycardType.Yellow);
            IList<ReadOnlyInventoryItem> redKeycards = inventory.GetItemsOfType<KeycardItem>((item) => item.KeycardType == KeycardType.Red);
            slots[0].SetItem(greenKeycards[0], ItemDataContainerSO.GetSprite(greenKeycards[0].Item));
            slots[1].SetItem(yellowKeycards[0], ItemDataContainerSO.GetSprite(yellowKeycards[0].Item));
            slots[2].SetItem(redKeycards[0], ItemDataContainerSO.GetSprite(redKeycards[0].Item));
        }

        public void SetKeycardRequiredDoor(KeycardRequiredDoor keycardRequiredDoor)
        {
            this.keycardRequiredDoor = keycardRequiredDoor;
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (spamLock || isInserting) return;
            SetKeycard(KeycardType.None);
            if (context.performed) keycardUIChannel.RaiseEvent(false);
        }
        
        public void SetKeycard(KeycardType type)
        {
            if (currentCardGo != null)
            {
                Destroy(currentCardGo);
            }
            switch (type)
            {
                case KeycardType.None: return;
                case KeycardType.Green:
                    currentCardGo = Instantiate(cardModelGreen);
                    break;
                case KeycardType.Yellow:
                    currentCardGo = Instantiate(cardModelYellow);
                    break;
                case KeycardType.Red:
                    currentCardGo = Instantiate(cardModelRed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var camTransform = Camera.main.transform;
            currentCardGo.transform.position = camTransform.position - camTransform.up * 2f;
            currentCardGo.transform.rotation = Quaternion.LookRotation(-camTransform.forward) * Quaternion.AngleAxis(90f, currentCardGo.transform.forward);
        }
    }
}