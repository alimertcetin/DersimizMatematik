using UnityEngine;
using TMPro;

namespace XIV.UI
{
    public class HUD_UI : MonoBehaviour
    {
        // TODO : Remove this class
    }

    public class HUDNumberUI : MonoBehaviour
    {
        [SerializeField] RectTransform contentParent;
        [SerializeField] GameObject numberPrefab;

        private void Awake()
        {
            for (int i = 0; i < 9; i++)
            {
                var numberGo = Object.Instantiate(numberPrefab, contentParent);
                numberGo.GetComponent<NumberUIItem>().SetValue(i);
            }
        }
    }

    public class NumberUIItem : MonoBehaviour
    {
        int value;
        int amount;
        public TMP_Text text;
        public TMP_Text amountText;

        public void SetValue(int value)
        {
            this.value = value;
            text.text = value.ToString();
        }

        public void SetAmount(int value)
        {
            this.value = value;
            text.text = value.ToString();
        }
    }
}