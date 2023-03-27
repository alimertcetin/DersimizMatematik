using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.Events;
using XIV;
using XIV.Easing;
using XIV.EventSystem;
using XIV.EventSystem.Events;
using XIV.Extensions;
using XIV.InventorySystem;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.SaveSystems;
using XIV.XIVMath;

namespace LessonIsMath.CollectableSystems
{
    [RequireComponent(typeof(SaveableEntity))]
    public class Keycard : MonoBehaviour, ISaveable, IInteractable
    {
        [SerializeField] KeycardItemSO keycardItemSO;
        [SerializeField] InventoryChannelSO inventoryLoadedChannel;
        [SerializeField] ParticleSystem CollectedParticle;
        [SerializeField] float collectDuration = 1.5f;
        [SerializeField] StringEventChannelSO warningChannel;
        [SerializeField] UnityEvent onKeycardCollected;

        Inventory inventory;
        bool collected;
        Collider col;
        new MeshRenderer renderer;
        bool isInInteraction;
        bool IInteractable.IsInInteraction => isInInteraction;

        void Awake()
        {
            col = GetComponent<Collider>();
            renderer = GetComponentInChildren<MeshRenderer>();
        }

        void OnEnable() => inventoryLoadedChannel.Register(OnInventoryLoaded);
        void OnDisable() => inventoryLoadedChannel.Unregister(OnInventoryLoaded);
        void OnInventoryLoaded(Inventory inventory) => this.inventory = inventory;

        InteractionSettings IInteractable.GetInteractionSettings() => new InteractionSettings();
        bool IInteractable.IsAvailableForInteraction() => !collected;
        
        void IInteractable.Interact(IInteractor interactor)
        {
            var item = keycardItemSO.GetItem();
            int amount = 1;

            bool exists = inventory.Contains(item, out var itemIndex);
            bool canAdd = inventory.CanAdd(item, amount);
            if (canAdd == false || (exists && inventory[itemIndex].Amount >= item.StackableAmount))
            {
                warningChannel.RaiseEvent("There is not enough space for this keycard");
                interactor.OnInteractionEnd(this);
                return;
            }

            isInInteraction = true;

            var interactorTransform = ((Component)interactor).transform;
            var startPos = transform.position;
            // endpos is player backpack position, it works for now but later on we could need IInteractor.GetBackpackPosition or something like that
            var endPos = interactorTransform.position - (interactorTransform.forward * 0.15f) + (Vector3.up * 1.15f);
            var mid1 = startPos + (endPos - startPos) * 0.25f + Vector3.up;
            var mid2 = startPos + (endPos - startPos) * 0.75f + Random.insideUnitSphere;
            XIVEventSystem.SendEvent(new InvokeForSecondsEvent(collectDuration).AddAction((timer) =>
            {
                var endPos = interactorTransform.position - (interactorTransform.forward * 0.15f) + (Vector3.up * 1.15f);
                var t = EasingFunction.SmoothStart1(timer.NormalizedTime);
#if UNITY_EDITOR
                XIVDebug.DrawBezier(startPos, mid1,  mid2, endPos, 0.25f);
#endif
                var newPosition = BezierMath.GetPoint(startPos, mid1,  mid2, endPos, t);
                transform.position = newPosition;
            }).OnCompleted(() =>
            {
                inventory.TryAdd(item, ref amount);
                SpawnParicle(interactorTransform);
                renderer.enabled = false;
                onKeycardCollected.Invoke();
            }));
            collected = true;
            col.enabled = false;
            interactor.OnInteractionEnd(this);
        }

        string IInteractable.GetInteractionString()
        {
            return "Press " + InputManager.InteractionKeyName + " to collect " + keycardItemSO.GetItem().GetColoredCardString();
        }

        InteractionPositionData IInteractable.GetInteractionPositionData(IInteractor interactor)
        {
            var interactorPos = (interactor as Component).transform.position;
            var targetPos = Vector3.MoveTowards(interactorPos, transform.position.SetY(interactorPos.y), 0.2f);
            return new InteractionPositionData
            {
                startPos = interactorPos,
                targetPosition = interactorPos,
                targetForwardDirection = (interactorPos - targetPos).normalized,
            };
        }

        void SpawnParicle(Transform parent)
        {
            GameObject go = Instantiate(CollectedParticle.gameObject, parent);
            go.transform.position = transform.position;
            Destroy(go, 5.0f);
        }

        #region --- SAVE ---

        [System.Serializable]
        struct SaveData
        {
            public bool isCollected;
        }

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

        #endregion

    }
}