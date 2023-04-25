using UnityEngine;
using UnityEngine.UI;

namespace XIV.InventorySystem.Utils
{
    public class UIItemDisplayer : MonoBehaviour
    {
        public Image[] images;
        public Sprite[] sprites;
        public Button previousButton;
        public Button nextButton;
        public float slideDuration = 2f;

        public SlideStyleDisplayer SlideStyleDisplayer;
        public bool isInTransition;

        void OnEnable()
        {
            SlideStyleDisplayer = new SlideStyleDisplayer(images, sprites, slideDuration);
            nextButton.onClick.AddListener(() =>
            {
                if (isInTransition) return;
                isInTransition = true;
            });
        }

        void Update()
        {
            if (isInTransition)
            {
                isInTransition = !SlideStyleDisplayer.UpdateNext(Time.deltaTime);
            }
        }
    }
}