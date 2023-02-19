using ElRaccoone.Tweens;
using UnityEngine;

namespace LessonIsMath.UI
{
    public abstract class PageUI : MonoBehaviour
    {
        [SerializeField] protected GameObject uiGameObject;
        public bool isActive { get; private set; }
        public virtual void Show()
        {
            uiGameObject.transform.localScale = Vector3.zero;
            uiGameObject.SetActive(true);
            uiGameObject.TweenCancelAll();
            uiGameObject.TweenLocalScale(Vector3.one, 0.5f)
                .SetEaseExpoInOut();
            isActive = true;
        }

        public virtual void Hide()
        {
            uiGameObject.SetActive(false);
            isActive = false;
        }
    }

}