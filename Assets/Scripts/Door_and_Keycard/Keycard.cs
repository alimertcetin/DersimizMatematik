using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SaveableEntity))]
public class Keycard : MonoBehaviour, ISaveable
{
    private PlayerInventory inventory;

    [SerializeField]
    private Door_and_Keycard_Level keycardType = Door_and_Keycard_Level.None;
    public Door_and_Keycard_Level KeycardType { get => keycardType; private set => keycardType = value; }

    [Tooltip("Toplandığında oynatılacak particle efektini bu alana ekleyin.")]
    [SerializeField] private ParticleSystem CollectedParticle = null;
    private bool triggerEntered;
    private bool Collected;
    private Collider col = default;
    private new MeshRenderer renderer = default;

    public UnityAction KeycardCollected; //TODO : Use UnityAction

    private void Awake()
    {
        col = GetComponent<Collider>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;
        inventory = FindObjectOfType<PlayerInventory>();
    }

    private void OnDisable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerEntered = true;
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
        if (triggerEntered && !Collected && inventory.KeycardEkle(keycardType))
        {
            SpawnParicle();
            Collected = true;
            col.enabled = false;
            renderer.enabled = false;
            KeycardCollected?.Invoke();

            if (TryGetComponent(out ObjectBasedEvents events))
                StartCoroutine(disableEvents(2, events));
        }
    }

    private IEnumerator disableEvents(float time, ObjectBasedEvents events)
    {
        yield return new WaitForSeconds(time);
        events.enabled = false;

    }

    private void SpawnParicle()
    {
        GameObject go = Instantiate(CollectedParticle.gameObject); //Particle yarat.
        go.transform.position = this.transform.position; //Particle konumunu bu Gameobject olarak belirle
        Destroy(go, 5.0f); //Particle'ı 5 saniye sonra yok et.
    }


    #region -_- SAVE -_-

    public object CaptureState()
    {
        return new SaveData
        {
            _isCollected = Collected
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        Collected = saveData._isCollected;
        if (!Collected)
        {
            col.enabled = true;
            renderer.enabled = true;
        }
    }

    [System.Serializable]
    struct SaveData
    {
        public bool _isCollected;
    }

    #endregion

}
