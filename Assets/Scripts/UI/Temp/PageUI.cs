using UnityEngine;

namespace LessonIsMath.UI
{
    public abstract class PageUI : MonoBehaviour
    {
        [SerializeField] protected GameObject uiGameObject;
        public bool isActive { get; private set; }
        public virtual void Show()       {
            uiGameObject.SetActive(true);
            isActive = true;
        }

        public virtual void Hide()
        {
            uiGameObject.SetActive(false);
            isActive = false;
        }
    }

}