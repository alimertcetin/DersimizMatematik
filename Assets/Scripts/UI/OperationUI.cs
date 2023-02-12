using LessonIsMath.ScriptableObjects.Channels;
using UnityEngine;
using XIV.InventorySystem;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.Utils;

namespace LessonIsMath.UI
{
    public abstract class OperationUI : MonoBehaviour
    {
        [SerializeField] protected CustomButton btn_Back;
        [SerializeField] protected CustomButton btn_Delete;
        [SerializeField] protected CustomButton[] btn_Numbers;
        [SerializeField] protected NumberItemSO[] numberItems;

        [Header("UI Settings")]
        [SerializeField] protected Timer deleteTimer;

        [Header("Channels")]
        [SerializeField] protected StringEventChannelSO warningUIChannel = default;
        [SerializeField] protected InventoryChannelSO inventoryLoadedChannel;
        protected Inventory inventory;
        protected bool deleteStarted = false;

        private void Update()
        {
            if (deleteStarted == false || deleteTimer.Update(Time.deltaTime) == false) return;

            Delete();
            deleteTimer.Restart();
        }

        protected virtual void OnEnable()
        {
            btn_Back.Register(CloseUI);
            btn_Delete.Register(Delete);

            for (int i = 0; i < btn_Numbers.Length; i++)
            {
                int val = i;
                btn_Numbers[i].Register(() => OnNumberButtonClicked(val));
            }

            inventoryLoadedChannel.Register(OnInventoryLoaded);
        }

        protected virtual void OnDisable()
        {
            btn_Back.Unregister();
            btn_Delete.Unregister();

            for (int i = 0; i < btn_Numbers.Length; i++)
            {
                btn_Numbers[i].Unregister();
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
