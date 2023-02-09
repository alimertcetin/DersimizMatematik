using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV.InventorySystem;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ChannelSOs;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.SaveSystem;

[RequireComponent(typeof(SaveableEntity))]
public class Keycard : MonoBehaviour, ISaveable
{
    [SerializeField] KeycardItemSO keycardItemSO;
    [SerializeField] InventoryChannelSO inventoryLoadedChannel;
    [SerializeField] ParticleSystem CollectedParticle = null;
    [SerializeField] StringEventChannelSO notificationChannel = default;

    Inventory inventory;
    bool triggerEntered;
    bool Collected;
    Collider col = default;
    new MeshRenderer renderer = default;

    private void Awake()
    {
        col = GetComponent<Collider>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
        inventoryLoadedChannel.Register(OnInventoryLoaded);
    }

    private void OnDisable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
        inventoryLoadedChannel.Unregister(OnInventoryLoaded);
    }

    private void OnInventoryLoaded(Inventory obj)
    {
        this.inventory = obj;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerEntered = true;
            notificationChannel.RaiseEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerEntered = false;
        }
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (triggerEntered == false || Collected)
        {
            return;
        }

        var item = keycardItemSO.GetItem();
        int amount = 1;

        if (inventory.CanAdd(item, amount) == false)
        {
            notificationChannel.RaiseEvent("There is not enough space for this keycard", true);
            return;
        }

        inventory.TryAdd(keycardItemSO.GetItem(), ref amount);

        SpawnParicle();
        Collected = true;
        col.enabled = false;
        renderer.enabled = false;

        if (TryGetComponent(out ObjectBasedEvents events))
            StartCoroutine(disableEvents(2, events));
    }

    private IEnumerator disableEvents(float time, ObjectBasedEvents events)
    {
        yield return new WaitForSeconds(time);
        events.enabled = false;

    }

    private void SpawnParicle()
    {
        GameObject go = Instantiate(CollectedParticle.gameObject);
        go.transform.position = this.transform.position;
        Destroy(go, 5.0f);
    }

    private string SetKeycardText()
    {
        KeycardItem item = keycardItemSO.GetItem() as KeycardItem;
        return "Press " + InputManager.InteractionKeyName
            + " to collect " + item.GetColoredCardString();
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
