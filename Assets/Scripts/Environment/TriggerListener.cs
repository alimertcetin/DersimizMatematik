using UnityEngine;
using UnityEngine.Events;
using XIV.SaveSystems;

namespace LessonIsMath.Environment
{
    [SelectionBase]
    public class TriggerListener : MonoBehaviour, ISaveable
    {
        [SerializeField] Collider collider;
        [SerializeField] LayerMask layersToListen;
        [SerializeField] UnityEvent onTriggerEnter;
        [SerializeField] UnityEvent onTriggerExit;

        void Awake()
        {
            collider.gameObject.AddComponent<TriggerListenerHelper>().owner = this;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            collider = GetComponentInChildren<Collider>();
            if (collider == null) Debug.LogError("There is no collider on gameObject. " + this.gameObject);
            if (collider.isTrigger == false) Debug.LogError("Collider's isTrigger flag is set to false. " + this.gameObject);
        }
#endif

        void TriggerEnter(Collider other)
        {
            if ((layersToListen & (1 << other.gameObject.layer)) == 0) return;
            onTriggerEnter.Invoke();
        }

        void TriggerExit(Collider other)
        {
            if ((layersToListen & (1 << other.gameObject.layer)) == 0) return;
            onTriggerExit.Invoke();
        }

        #region --- Save ---

        struct SaveData
        {
            public bool colliderEnabled;
        }

        object ISaveable.CaptureState()
        {
            return new SaveData
            {
                colliderEnabled = collider.enabled,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            var saveData = (SaveData)state;
            collider.enabled = saveData.colliderEnabled;
        }

        #endregion
        
        class TriggerListenerHelper : MonoBehaviour
        {
            public TriggerListener owner;

            void OnTriggerEnter(Collider other)
            {
                owner.TriggerEnter(other);
            }

            void OnTriggerExit(Collider other)
            {
                owner.TriggerExit(other);
            }
        }
    }
}