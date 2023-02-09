using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;

namespace XIV.InventorySystem.UI
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class InventoryItemDragger : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        GraphicRaycaster raycaster;
        bool isSelected;
        Vector3 startPos;
        Transform parentBefore;
        InventorySlot selectedSlot;
        Inventory inventory;

        void Start()
        {
            raycaster = GetComponent<GraphicRaycaster>();
        }

        void OnEnable() => inventoryLoadedChannel.Register(LoadInventory);
        void OnDisable() => inventoryLoadedChannel.Unregister(LoadInventory);
        void LoadInventory(Inventory inventory) => this.inventory = inventory;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isSelected) ClearSelection();
            
            if (eventData.used) return;

            var results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);
            for (var i = 0; i < results.Count; i++)
            {
                if (!results[i].gameObject.TryGetComponent(out selectedSlot)) continue;
                
                isSelected = true;
                eventData.Use();
                var selectedSlotTransform = selectedSlot.transform;
                parentBefore = selectedSlotTransform.parent;
                startPos = selectedSlotTransform.position;
                selectedSlotTransform.SetParent(selectedSlotTransform.root);
                selectedSlotTransform.position = Input.mousePosition;
                selectedSlot.GetComponent<Image>().raycastTarget = false;
                
                break;
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (isSelected == false || eventData.used) return;
            selectedSlot.transform.position = Input.mousePosition;
            eventData.Use();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isSelected == false) return;
            
            var results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);
            for (var i = 0; i < results.Count; i++)
            {
                if (!results[i].gameObject.TryGetComponent(out InventorySlot other)) continue;
                
                inventory.Swap(selectedSlot.inventoryItem.Index, other.inventoryItem.Index);
                
                eventData.Use();
                break;
            }
            
            ClearSelection();
        }

        void ClearSelection()
        {
            selectedSlot.GetComponent<Image>().raycastTarget = true;
            selectedSlot.transform.position = startPos;
            selectedSlot.transform.SetParent(parentBefore);
            isSelected = false;
        }
    }
}