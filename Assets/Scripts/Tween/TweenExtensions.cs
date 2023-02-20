using ElRaccoone.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace LessonIsMath.Tween
{
    public static class TweenExtensions
    {
        public static void ClickTween(this Button button, float duration)
        {
            const float tweenFactor = 0.9f;

            button.TweenCancelAll();
            var buttonTransform = button.transform;
            var pos = buttonTransform.position;
            var scale = buttonTransform.localScale;

            var newPos = pos + Vector3.up * tweenFactor;
            var newScale = scale * tweenFactor;
            button.TweenPosition(newPos, duration)
                .TweenLocalScale(newScale, duration)
                .SetOnComplete(() =>
                {
                    button.TweenPosition(pos, duration)
                    .TweenLocalScale(scale, duration).
                    SetOnCancel(OnCancel);
                })
                .SetOnCancel(OnCancel);

            void OnCancel()
            {
                buttonTransform.localScale = scale;
                buttonTransform.position = pos;
            }
        }
    }
}
