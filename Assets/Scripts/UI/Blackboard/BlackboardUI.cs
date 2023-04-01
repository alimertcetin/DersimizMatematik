using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.StatSystems;
using LessonIsMath.StatSystems.Stats;
using LessonIsMath.UI.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;

namespace LessonIsMath.UI
{
    public class BlackboardUI : ParentGameUI, PlayerControls.IGameUIActions, IStatListener
    {
        // TODO : Remove channel dependency
        [SerializeField] BoolEventChannelSO blackBoardUIChannel = default;
        [SerializeField] StatEventChannelSO brainPowerStatLoadedChannel;
        [SerializeField] EarnNumberPage earnNumberPage;
        [SerializeField] MakeOperationPage makeOperationPage;
        [SerializeField] CustomButton btn_EarnNumber;
        [SerializeField] CustomButton btn_MakeOperation;
        [SerializeField] CustomButton btn_Exit;
        [SerializeField] NumberItemSO[] numberItems;
        [SerializeField] StringEventChannelSO warningUIChannel = default;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;

        PageUI currentPage;
        Inventory inventory;
        IStat brainPowerStat;

        protected override void Awake()
        {
            base.Awake();
            earnNumberPage.SetBlackboard(this);
            makeOperationPage.SetBlackboard(this);
        }

        void OnEnable()
        {
            inventoryLoadedChannel.Register(OnInventoryLoaded);
            brainPowerStatLoadedChannel.OnEventRaised += OnBrainPowerLoaded;
        }

        void OnDisable()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
            brainPowerStatLoadedChannel.OnEventRaised -= OnBrainPowerLoaded;
        }

        void OnBrainPowerLoaded(IStat stat) => brainPowerStat = stat;
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
            if (brainPowerStat != null) brainPowerStat.Register(this);
        }

        public override void Hide()
        {
            base.Hide();
            btn_Exit.UnregisterOnClick();

            btn_EarnNumber.onClick.RemoveListener(ShowEarnNumberUI);
            btn_MakeOperation.onClick.RemoveListener(ShowMakeOperationUI);

            if (currentPage != null) currentPage.Hide();
            currentPage = null;

            InputManager.GameUI.Disable();
            InputManager.GameState.Enable();
            if (brainPowerStat != null) brainPowerStat.Unregister(this);
        }

        void ShowEarnNumberUI()
        {
            InputManager.GameUI.Disable();
            OpenPage(earnNumberPage);
            currentPage = earnNumberPage;
        }

        void ShowMakeOperationUI()
        {
            InputManager.GameUI.Disable();
            OpenPage(makeOperationPage);
            currentPage = makeOperationPage;
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

        void IStatListener.OnStatChanged(IStat stat)
        {
            var statData = stat.GetStatData();
            if (statData.normalizedCurrent > Mathf.Epsilon) return;
            
            if (currentPage != null) currentPage.Hide();
            ComeBack(currentPage);
            currentPage = null;
            blackBoardUIChannel.RaiseEvent(false);
            ShowWarning("You dont have enough brain power to use Blackboard");

        }
    }
}
