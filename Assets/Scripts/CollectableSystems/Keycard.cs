using System.Collections;
using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.SaveSystems;

namespace LessonIsMath.CollectableSystems
{
    [RequireComponent(typeof(SaveableEntity))]
    public class Keycard : MonoBehaviour, ISaveable, IInteractable
    {
        [SerializeField] KeycardItemSO keycardItemSO;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] ParticleSystem CollectedParticle = null;
        [SerializeField] StringEventChannelSO warningChannel = default;

        Inventory inventory;
        bool Collected;
        Collider col = default;
        new MeshRenderer renderer = default;

        private void Awake()
        {
            col = GetComponent<Collider>();
            renderer = GetComponentInChildren<MeshRenderer>();
        }

        private void OnEnable()
        {
            inventoryLoadedChannel.Register(OnInventoryLoaded);
        }

        private void OnDisable()
        {
            inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        }

        private void OnInventoryLoaded(Inventory obj)
        {
            inventory = obj;
        }

        public bool IsAvailable()
        {
            return !Collected;
        }

        public void Interact(IInteractor interactor)
        {
            var item = keycardItemSO.GetItem();
            int amount = 1;

            if (inventory.CanAdd(item, amount) == false)
            {
                warningChannel.RaiseEvent("There is not enough space for this keycard", true);
                interactor.OnInteractionEnd(this);
                return;
            }

            inventory.TryAdd(keycardItemSO.GetItem(), ref amount);

            SpawnParicle();
            Collected = true;
            col.enabled = false;
            renderer.enabled = false;
            interactor.OnInteractionEnd(this);
        }

        public string GetInteractionString()
        {
            return "Press " + InputManager.InteractionKeyName + " to collect " + keycardItemSO.GetItem().GetColoredCardString();
        }

        void SpawnParicle()
        {
            GameObject go = Instantiate(CollectedParticle.gameObject);
            go.transform.position = transform.position;
            Destroy(go, 5.0f);
        }

        #region -_- SAVE -_-

        public object CaptureState()
        {
            return new SaveData
            {
                isCollected = Collected
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;
            Collected = saveData.isCollected;
            if (!Collected)
            {
                col.enabled = true;
                renderer.enabled = true;
            }
        }

        [System.Serializable]
        struct SaveData
        {
            public bool isCollected;
        }

        #endregion

    }
}