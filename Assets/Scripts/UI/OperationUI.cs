using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XIV.InventorySystem;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.Utils;

namespace GameCore.UI
{
    public abstract class OperationUI : MonoBehaviour
    {
        [SerializeField] protected Button btn_Back;
        [SerializeField] protected Button btn_Delete;
        [SerializeField] protected Button[] btn_Numbers;
        [SerializeField] protected NumberItemSO[] numberItems;

        [Header("UI Settings")]
        [SerializeField] protected Timer deleteTimer;

        [Header("Channels")]
        [SerializeField] protected StringEventChannelSO warningUIChannel = default;
        [SerializeField] protected InventoryChannelSO inventoryLoadedChannel;

        protected UnityAction[] numberButtonOnClickActions;
        protected Inventory inventory;
        protected bool deleteStarted = false;

        protected virtual void Awake()
        {
            int length = btn_Numbers.Length;
            numberButtonOnClickActions = new UnityAction[length];

            for (int i = 0; i < length; i++)
            {
                int val = i;
                numberButtonOnClickActions[i] = () => OnNumberButtonClicked(val);
            }
        }

        private void Update()
        {
            if (deleteStarted == false || deleteTimer.Update(Time.deltaTime) == false) return;

            Delete();
            deleteTimer.Restart();
        }

        protected virtual void OnEnable()
        {
            btn_Back.onClick.AddListener(CloseUI);
            btn_Delete.onClick.AddListener(Delete);

            for (int i = 0; i < btn_Numbers.Length; i++)
            {
                btn_Numbers[i].onClick.AddListener(numberButtonOnClickActions[i]);
            }

            inventoryLoadedChannel.Register(OnInventoryLoaded);
        }

        protected virtual void OnDisable()
        {
            btn_Back.onClick.RemoveListener(CloseUI);
            btn_Delete.onClick.RemoveListener(Delete);

            for (int i = 0; i < btn_Numbers.Length; i++)
            {
                btn_Numbers[i].onClick.RemoveListener(numberButtonOnClickActions[i]);
            }

            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        }

        protected virtual void OnInventoryLoaded(Inventory obj)
        {
            this.inventory = obj;
        }

        public abstract void ShowUI();
        public abstract void CloseUI();
        protected abstract void Delete();
        protected abstract void OnNumberButtonClicked(int value);

    }

}
