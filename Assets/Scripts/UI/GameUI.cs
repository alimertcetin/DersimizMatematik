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
            var scale = gameObject.transform.localScale;
            uiGameObject.transform.localScale = Vector3.zero;
            uiGameObject.SetActive(true);
            uiGameObject.TweenCancelAll();
            uiGameObject.TweenLocalScale(scale, 0.5f)
                .SetEaseExpoInOut();
            isActive = true;
        }

        public virtual void Hide()
        {
            uiGameObject.TweenCancelAll();
            var scale = gameObject.transform.localScale;
            uiGameObject.TweenLocalScale(Vector3.zero, 0.5f)
                .SetEaseExpoInOut()
                .SetOnComplete(() =>
                {
                    uiGameObject.SetActive(false);
                    uiGameObject.transform.localScale = scale;
                });
            isActive = false;
        }

        protected virtual void OnDestroy() => UISystem.RemoveUI(this);
    }

}