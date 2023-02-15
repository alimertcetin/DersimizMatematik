using UnityEngine;

namespace LessonIsMath.UI
{
    public abstract class GameUI : MonoBehaviour
    {
        [SerializeField] protected GameObject uiGameObject;
        public bool isActive { get; private set; }
        protected virtual void Awake() => UISystem.AddUI(this);
        public virtual void Show()
        {
            uiGameObject.SetActive(true);
            isActive = true;
        }

        public virtual void Hide()
        {
            uiGameObject.SetActive(false);
            isActive = false;
        }

        protected virtual void OnDestroy() => UISystem.RemoveUI(this);
    }

}