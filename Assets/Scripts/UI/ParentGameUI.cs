using ElRaccoone.Tweens;
using UnityEngine;

namespace LessonIsMath.UI
{
    public abstract class ParentGameUI : GameUI
    {
        [SerializeField] protected GameObject mainPageGO;

        public override void Show()
        {
            base.Show();
            mainPageGO.transform.localScale = Vector3.one;
            mainPageGO.SetActive(true);
        }

        public override void Hide()
        {
            mainPageGO.transform.localScale = Vector3.one;
            uiGameObject.TweenCancelAll();
            uiGameObject.TweenLocalScale(Vector3.zero, 0.5f)
                .SetEaseExpoInOut()
                .SetOnComplete(() =>
                {
                    mainPageGO.SetActive(false);
                    uiGameObject.SetActive(false);
                });
            isActive = false;
        }

        public virtual void OpenPage(PageUI page)
        {
            mainPageGO.SetActive(false);
            page.Show();
        }

        public virtual void ComeBack(PageUI from)
        {
            mainPageGO.transform.localScale = Vector3.zero;
            mainPageGO.SetActive(true);
            mainPageGO.TweenCancelAll();
            mainPageGO.TweenLocalScale(Vector3.one, 0.5f)
                .SetEaseExpoInOut();
        }
        
    }
}