using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XIV.InventorySystem;

namespace XIV.UI
{
    public class HUDItemSlot : MonoBehaviour
    {
        [SerializeField] Image itemImage;
        [SerializeField] TMP_Text amountText;
        
        public ReadOnlyInventoryItem inventoryItem { get; private set; }

        public void SetItem(ReadOnlyInventoryItem inventoryItem)
        {
            this.inventoryItem = inventoryItem;
            UpdateProperties();
        }

        void UpdateProperties()
        {
            this.itemImage.sprite = inventoryItem.Item.UISprite;
            this.amountText.text = inventoryItem.Amount > 0 ? inventoryItem.Amount.ToString() : "";
        }
    }
}