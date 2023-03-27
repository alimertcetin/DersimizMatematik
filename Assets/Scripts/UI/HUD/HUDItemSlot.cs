using ElRaccoone.Tweens;
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
            if (inventoryItem.Amount > this.inventoryItem.Amount)
            {
                amountText.TweenCancelAll();
                var scale = amountText.transform.localScale;
                amountText.TweenLocalScale(scale + Vector3.one * 0.2f, 0.15f)
                    .SetOnCancel(() => amountText.transform.localScale = scale)
                    .SetPingPong()
                    .SetEaseBounceInOut();
            }
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