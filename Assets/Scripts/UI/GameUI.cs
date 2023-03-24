using ElRaccoone.Tweens;
using UnityEngine;

namespace LessonIsMath.UI
{
    public abstract class GameUI : MonoBehaviour
    {
        [SerializeField] protected GameObject uiGameObject;
        public bool isActive { get; protected set; }
        protected virtual void Awake() => UISystem.AddUI(this);
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
            uiGameObject.TweenCancelAll();
            uiGameObject.TweenLocalScale(Vector3.zero, 0.5f)
                .SetEaseExpoInOut()
                .SetOnComplete(() =>
                {
                    uiGameObject.SetActive(false);
                });
            isActive = false;
        }

        protected virtual void OnDestroy() => UISystem.RemoveUI(this);
    }
}