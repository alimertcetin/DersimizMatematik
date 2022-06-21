using UnityEngine;
using TMPro;

namespace XIV.UI
{
    public class HUD_UI : MonoBehaviour
    {
        [SerializeField] private PlayerInventorySO inventorySO = default;

        [Header("HUD üzerindeki sayıların child textlerini sırasıyla ekleyin.")]
        [SerializeField] private TMP_Text[] NumbersChildTexts = null;

        [Header("Keycardların textlerini sırasıyla ekleyin.")]
        [Tooltip("Yeşil, sarı, kırmızı - 0,1,2 şeklinde.")]
        [SerializeField] private TMP_Text[] KeycardsChildTexts = new TMP_Text[3];

        private void Start()
        {
            SetNumberChildTexts();
            SetKeycardChildTexts();
        }

        private void OnEnable()
        {
            inventorySO.InventoryChanged_Keycard += SetKeycardChildTexts;
            inventorySO.InventoryChanged_Number += SetNumberChildTexts;
        }

        private void OnDisable()
        {
            inventorySO.InventoryChanged_Keycard -= SetKeycardChildTexts;
            inventorySO.InventoryChanged_Number -= SetNumberChildTexts;
        }

        public void SetKeycardChildTexts()
        {
            //Keycard Seviye 1
            if (inventorySO.yesilKeycard > 0)
            {
                KeycardsChildTexts[0].text = "x" + inventorySO.yesilKeycard.ToString();
                KeycardsChildTexts[0].enabled = true;
                KeycardsChildTexts[0].gameObject.SetActive(true);
            }
            else
                KeycardsChildTexts[0].gameObject.SetActive(false);

            //Keycard Seviye 2
            if (inventorySO.sariKeycard > 0)
            {
                KeycardsChildTexts[1].text = "x" + inventorySO.sariKeycard.ToString();
                KeycardsChildTexts[1].enabled = true;
                KeycardsChildTexts[1].gameObject.SetActive(true);
            }
            else
                KeycardsChildTexts[1].gameObject.SetActive(false);

            //Keycard Seviye 3
            if (inventorySO.kirmiziKeycard > 0)
            {
                KeycardsChildTexts[2].text = "x" + inventorySO.kirmiziKeycard.ToString();
                KeycardsChildTexts[2].enabled = true;
                KeycardsChildTexts[2].gameObject.SetActive(true);
            }
            else
                KeycardsChildTexts[2].gameObject.SetActive(false);
        }

        //Çalışması için diziye Child textler atanmış olmalı
        public void SetNumberChildTexts()
        {
            //Sayi_0
            if (inventorySO.Rakam_0 > 0)
            {
                NumbersChildTexts[0].text = "x" + inventorySO.Rakam_0.ToString();
                if (!NumbersChildTexts[0].enabled)
                    NumbersChildTexts[0].enabled = true;
            }
            else
            {
                NumbersChildTexts[0].enabled = false;
            }
            //Sayi_1
            if (inventorySO.Rakam_1 > 0)
            {
                NumbersChildTexts[1].text = "x" + inventorySO.Rakam_1.ToString();
                if (!NumbersChildTexts[1].enabled)
                    NumbersChildTexts[1].enabled = true;
            }
            else
            {
                NumbersChildTexts[1].enabled = false;
            }
            //Sayi_2
            if (inventorySO.Rakam_2 > 0)
            {
                NumbersChildTexts[2].text = "x" + inventorySO.Rakam_2.ToString();
                if (!NumbersChildTexts[2].enabled)
                    NumbersChildTexts[2].enabled = true;
            }
            else
            {
                NumbersChildTexts[2].enabled = false;
            }
            //Sayi_3
            if (inventorySO.Rakam_3 > 0)
            {
                NumbersChildTexts[3].text = "x" + inventorySO.Rakam_3.ToString();
                if (!NumbersChildTexts[3].enabled)
                    NumbersChildTexts[3].enabled = true;
            }
            else
            {
                NumbersChildTexts[3].enabled = false;
            }
            //Sayi_4
            if (inventorySO.Rakam_4 > 0)
            {
                NumbersChildTexts[4].text = "x" + inventorySO.Rakam_4.ToString();
                if (!NumbersChildTexts[4].enabled)
                    NumbersChildTexts[4].enabled = true;
            }
            else
            {
                NumbersChildTexts[4].enabled = false;
            }
            //Sayi_5
            if (inventorySO.Rakam_5 > 0)
            {
                NumbersChildTexts[5].text = "x" + inventorySO.Rakam_5.ToString();
                if (!NumbersChildTexts[5].enabled)
                    NumbersChildTexts[5].enabled = true;
            }
            else
            {
                NumbersChildTexts[5].enabled = false;
            }
            //Sayi_6
            if (inventorySO.Rakam_6 > 0)
            {
                NumbersChildTexts[6].text = "x" + inventorySO.Rakam_6.ToString();
                if (!NumbersChildTexts[6].enabled)
                    NumbersChildTexts[6].enabled = true;
            }
            else
            {
                NumbersChildTexts[6].enabled = false;
            }
            //Sayi_7
            if (inventorySO.Rakam_7 > 0)
            {
                NumbersChildTexts[7].text = "x" + inventorySO.Rakam_7.ToString();
                if (!NumbersChildTexts[7].enabled)
                    NumbersChildTexts[7].enabled = true;
            }
            else
            {
                NumbersChildTexts[7].enabled = false;
            }
            //Sayi_8
            if (inventorySO.Rakam_8 > 0)
            {
                NumbersChildTexts[8].text = "x" + inventorySO.Rakam_8.ToString();
                if (!NumbersChildTexts[8].enabled)
                    NumbersChildTexts[8].enabled = true;
            }
            else
            {
                NumbersChildTexts[8].enabled = false;
            }
            //Sayi_9
            if (inventorySO.Rakam_9 > 0)
            {
                NumbersChildTexts[9].text = "x" + inventorySO.Rakam_9.ToString();
                if (!NumbersChildTexts[9].enabled)
                    NumbersChildTexts[9].enabled = true;
            }
            else
            {
                NumbersChildTexts[9].enabled = false;
            }
        }
    }
}