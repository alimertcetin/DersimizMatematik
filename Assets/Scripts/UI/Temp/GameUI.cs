using ElRaccoone.Tweens;
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
            var scale = gameObject.transform.localScale;
            uiGameObject.transform.localScale = Vector3.zero;
            uiGameObject.SetActive(true);
            uiGameObject.TweenCancelAll();
            uiGameObject.TweenLocalScale(scale, 0.5f)
                .SetEaseElasticInOut()
                .SetOnComplete(() => uiGameObject.GetComponent<CanvasGroup>().interactable = true);
            isActive = true;
        }

        public virtual void Hide()
        {
            uiGameObject.TweenCancelAll();
            var scale = gameObject.transform.localScale;
            uiGameObject.TweenLocalScale(Vector3.zero, 1f)
                .SetEaseElasticInOut()
                .SetOnComplete(() =>
                {
                    uiGameObject.GetComponent<CanvasGroup>().interactable = false;
                    uiGameObject.SetActive(false);
                    uiGameObject.transform.localScale = scale;
                });
            isActive = false;
        }

        protected virtual void OnDestroy() => UISystem.RemoveUI(this);
    }

}