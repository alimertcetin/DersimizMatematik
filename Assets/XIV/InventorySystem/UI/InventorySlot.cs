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
        Sprite uiSprite;

        public void SetItem(ReadOnlyInventoryItem inventoryItem, Sprite uiSprite)
        {
            this.inventoryItem = inventoryItem;
            UpdateVisual(uiSprite);
        }

        public void UpdateVisual(Sprite uiSprite)
        {
            this.uiSprite = uiSprite;
            bool empty = IsEmpty;
            if (empty == false) UpdateProperties();
            
            SetActiveVisual(!empty);
        }

        void UpdateProperties()
        {
            this.itemImage.sprite = uiSprite;
            this.amountText.text = inventoryItem.Amount > 1 ? inventoryItem.Amount.ToString() : "";
        }

        void SetActiveVisual(bool val)
        {
            itemImage.enabled = val;
            amountText.enabled = val;
        }
    }
}