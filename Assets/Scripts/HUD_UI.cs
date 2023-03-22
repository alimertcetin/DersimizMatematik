using UnityEngine;
using LessonIsMath.UI;
using TMPro;
using UnityEngine.UI;
using XIV.EventSystem;
using XIV.EventSystem.Events;
using XIV.Extensions;
using XIV.InventorySystem.UI;

namespace XIV.UI
{
    public class HUD_UI : GameUI
    {
        [SerializeField] InventoryPanel numbers;
        [SerializeField] InventoryPanel keycards;
        [SerializeField] float alphaFadeDuration = 0.5f;
        Image[] uiImages;
        TMP_Text[] uiTexts;
        float[] uiImagesDefaultAlphas;
        float[] uiTextsDefaultAlphas;
        IEvent customTweenEvent;

        protected override void Awake()
        {
            base.Awake();
            uiImages = uiGameObject.GetComponentsInChildren<Image>();
            uiTexts = uiGameObject.GetComponentsInChildren<TMP_Text>();

            int imageLength = uiImages.Length;
            uiImagesDefaultAlphas = new float[imageLength];
            for (int i = 0; i < imageLength; i++)
            {
                uiImagesDefaultAlphas[i] = uiImages[i].color.a;
            }

            int textsLength = uiTexts.Length;
            uiTextsDefaultAlphas = new float[textsLength];
            for (int i = 0; i < textsLength; i++)
            {
                uiTextsDefaultAlphas[i] = uiTexts[i].color.a;
            }
        }

        public void ShowNumbers() => numbers.gameObject.SetActive(true);
        public void HideNumbers() => numbers.gameObject.SetActive(false);
        public void ShowKeycards() => keycards.gameObject.SetActive(true);
        public void HideKeycards() => keycards.gameObject.SetActive(false);

        public override void Show()
        {
            // really need a custom tween system...
            XIVEventSystem.CancelEvent(customTweenEvent);
            for (var i = 0; i < uiImages.Length; i++)
            {
                uiImages[i].gameObject.SetActive(true);
            }
            for (var i = 0; i < uiTexts.Length; i++)
            {
                uiTexts[i].gameObject.SetActive(true);
            }

            int imagesLength = uiImages.Length;
            float[] imageStartColorA = new float[imagesLength];
            for (int i = 0; i < imagesLength; i++)
            {
                imageStartColorA[i] = uiImages[i].color.a;
            }

            int textsLength = uiTexts.Length;
            var textStartColorA = new float[textsLength];
            for (int i = 0; i < textsLength; i++)
            {
                textStartColorA[i] = uiTexts[i].color.a;
            }
            customTweenEvent = new InvokeForSecondsEvent(alphaFadeDuration).AddAction((timer) =>
                {
                    for (var i = 0; i < uiImages.Length; i++)
                    {
                        Image image = uiImages[i];
                        image.color = image.color.SetA(Mathf.Lerp(imageStartColorA[i], uiImagesDefaultAlphas[i], timer.NormalizedTime));
                    }
                    for (var i = 0; i < uiTexts.Length; i++)
                    {
                        var tmpText = uiTexts[i];
                        tmpText.color = tmpText.color.SetA(Mathf.Lerp(textStartColorA[i], uiTextsDefaultAlphas[i], timer.NormalizedTime));
                    }
                })
                .OnCompleted(() => isActive = true)
                .OnCanceled(() => isActive = true);
            XIVEventSystem.SendEvent(customTweenEvent);
        }

        public override void Hide()
        {
            XIVEventSystem.CancelEvent(customTweenEvent);

            int imagesLength = uiImages.Length;
            float[] imageStartColorA = new float[imagesLength];
            for (int i = 0; i < imagesLength; i++)
            {
                imageStartColorA[i] = uiImages[i].color.a;
            }

            int textsLength = uiTexts.Length;
            var textStartColorA = new float[textsLength];
            for (int i = 0; i < textsLength; i++)
            {
                textStartColorA[i] = uiTexts[i].color.a;
            }
            customTweenEvent = new InvokeForSecondsEvent(alphaFadeDuration).AddAction((timer) =>
                {
                    
                    for (var i = 0; i < uiImages.Length; i++)
                    {
                        Image image = uiImages[i];
                        image.color = image.color.SetA(Mathf.Lerp(imageStartColorA[i], 0f, timer.NormalizedTime));
                    }
                    for (var i = 0; i < uiTexts.Length; i++)
                    {
                        var tmpText = uiTexts[i];
                        tmpText.color = tmpText.color.SetA(Mathf.Lerp(textStartColorA[i], 0f, timer.NormalizedTime));
                    }
                })
                .OnCompleted(() =>
                {
                    for (var i = 0; i < uiImages.Length; i++)
                    {
                        uiImages[i].gameObject.SetActive(false);
                    }
                    for (var i = 0; i < uiTexts.Length; i++)
                    {
                        uiTexts[i].gameObject.SetActive(false);
                    }
                    isActive = false;
                })
                .OnCanceled(() => isActive = false);
            XIVEventSystem.SendEvent(customTweenEvent);
        }
    }
}