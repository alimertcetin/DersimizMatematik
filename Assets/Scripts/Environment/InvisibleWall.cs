using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.Events;
using XIV.SaveSystems;

namespace LessonIsMath.Environment
{
    [SelectionBase]
    public class InvisibleWall  : MonoBehaviour, ISaveable
    {
        [SerializeField] VoidEventChannelSO unlockWallChannel;
        [SerializeField] VoidEventChannelSO lockWallChannel;
        [SerializeField] Collider blockCollider;
        [SerializeField] UnityEvent onUnlocked;
        [SerializeField] UnityEvent onLocked;
        
        Renderer collidererRenderer;
        bool isLocked;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (blockCollider == null) blockCollider = GetComponentInChildren<Collider>();
            if (blockCollider == null) Debug.LogWarning("There is no collider attached to block collider field");
        }
#endif

        void Awake()
        {
            collidererRenderer = blockCollider.GetComponent<Renderer>();
            isLocked = true;
            blockCollider.enabled = true;
            collidererRenderer.enabled = true;
        }

        void OnEnable()
        {
            if (unlockWallChannel != null) unlockWallChannel.OnEventRaised += Unlock;
            if (lockWallChannel != null) lockWallChannel.OnEventRaised += Lock;
        }

        void OnDisable()
        {
            if (unlockWallChannel != null) unlockWallChannel.OnEventRaised -= Unlock;
            if (lockWallChannel != null) lockWallChannel.OnEventRaised -= Lock;
        }

        void Unlock()
        {
            isLocked = false;
            blockCollider.enabled = false;
            collidererRenderer.enabled = false;
            onUnlocked.Invoke();
        }

        void Lock()
        {
            isLocked = true;
            blockCollider.enabled = true;
            collidererRenderer.enabled = true;
            onLocked.Invoke();
        }

        #region --- Save ---

        struct SaveData
        {
            public bool isLocked;
        }
        
        object ISaveable.CaptureState()
        {
            return new SaveData
            {
                isLocked = this.isLocked,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            var saveData = (SaveData)state;
            this.isLocked = saveData.isLocked;
            if (isLocked) Lock();
            else Unlock();
        }

        #endregion
        
    }
}
