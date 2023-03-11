using System.Collections;
using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV;
using XIV.Extensions;
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
        [SerializeField] Transform interactionPos;
        [SerializeField] KeycardItemSO keycardItemSO;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] ParticleSystem CollectedParticle;
        [SerializeField] StringEventChannelSO warningChannel;

        Inventory inventory;
        bool collected;
        Collider col;
        new MeshRenderer renderer;
        bool IInteractable.IsInInteraction => false;

        void Awake()
        {
            col = GetComponent<Collider>();
            renderer = GetComponentInChildren<MeshRenderer>();
        }

        void OnEnable() => inventoryLoadedChannel.Register(OnInventoryLoaded);
        void OnDisable() => inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        void OnInventoryLoaded(Inventory obj) => inventory = obj;

        bool IInteractable.IsAvailableForInteraction() => !collected;
        
        void IInteractable.Interact(IInteractor interactor)
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
            collected = true;
            col.enabled = false;
            renderer.enabled = false;
            interactor.OnInteractionEnd(this);
        }

        string IInteractable.GetInteractionString()
        {
            return "Press " + InputManager.InteractionKeyName + " to collect " + keycardItemSO.GetItem().GetColoredCardString();
        }

        InteractionTargetData IInteractable.GetInteractionTargetData(IInteractor interactor)
        {
            var interactorPos = (interactor as Component).transform.position;
            var transformPos = transform.position.SetY(interactorPos.y);
            return new InteractionTargetData
            {
                startPos = interactorPos,
                targetPosition = Vector3.MoveTowards(transformPos, interactorPos, 0.25f),
                targetForwardDirection = (interactorPos - transformPos).normalized,
            };
        }

        void SpawnParicle()
        {
            GameObject go = Instantiate(CollectedParticle.gameObject);
            go.transform.position = transform.position;
            Destroy(go, 5.0f);
        }

        #region --- SAVE ---

        public object CaptureState()
        {
            return new SaveData
            {
                isCollected = collected
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;
            collected = saveData.isCollected;
            if (!collected)
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