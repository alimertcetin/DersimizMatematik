using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XIV.InventorySystem.UI
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] Image itemImage;
        [SerializeField] TMP_Text amountText;
        public bool IsEmpty => inventoryItem.Amount <= 0;
        
        public ReadOnlyInventoryItem inventoryItem { get; private set; }

        public void SetItem(ReadOnlyInventoryItem inventoryItem)
        {
            this.inventoryItem = inventoryItem;
            UpdateVisual();
        }

        public void UpdateVisual()
        {
            bool empty = IsEmpty;
            if (empty == false) UpdateProperties();
            
            SetActiveVisual(!empty);
        }

        void UpdateProperties()
        {
            this.itemImage.sprite = inventoryItem.Item.UISprite;
            this.amountText.text = inventoryItem.Amount > 1 ? inventoryItem.Amount.ToString() : "";
        }

        void SetActiveVisual(bool val)
        {
            itemImage.enabled = val;
            amountText.enabled = val;
        }
    }
}