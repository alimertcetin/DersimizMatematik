using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XIV.UI
{
    public class IslemYap_UI : MonoBehaviour, PlayerControls.IIslemYapUIActions
    {
        [Header("Broadcasting To")]
        [SerializeField]
        private StringEventChannelSO WarningUIChannel = default;

        private PlayerInventory inventory;

        [SerializeField] private TMP_Text txt_islemYap_InputField = null;
        [SerializeField] private TMP_Text txt_ReviewInput = null;

        [SerializeField] private GameObject Giris_UI = null;

        [Tooltip("Input field'a maksimum girilebilecek sayı miktarı.")]
        [SerializeField] private int InputFiedlMaxTextLenght = 7;
        private int? sayi1;
        private bool toplama = false, cikarma = false;
        private const string BASAMAKASILDI = "Daha fazla basamak giremezsin.";

        private float maintimer;
        private float timer;
        private float controlEachDeleteTime = .2f;
        private bool deleteStarted = false;

        private void OnEnable()
        {
            InputManager.IslemYapUI.Enable();
            InputManager.PlayerControls.IslemYapUI.SetCallbacks(this);

            InputManager.BlackBoardUIManagement.Disable();
        }

        private void OnDisable()
        {
            if (sayi1 != null || txt_islemYap_InputField.text != null)
            {
                CancelOperation(ref sayi1);
            }

            txt_islemYap_InputField.text = "";

            InputManager.BlackBoardUIManagement.Enable();
            InputManager.IslemYapUI.Disable();
        }

        private void Awake()
        {
            inventory = FindObjectOfType<PlayerInventory>();
        }

        private void Update()
        {
            //TODO : Edit delete time.
            if (deleteStarted)
            {
                timer += Time.deltaTime;
                maintimer += Time.deltaTime;
                if (timer > controlEachDeleteTime)
                {
                    btn_Sil();
                    timer = 0;
                }
                if (maintimer > controlEachDeleteTime + 1)
                {
                    btn_Sil();
                    timer = 0;
                }
            }
        }

        //btn_OnClick
        public void btn_OnClick(int btnDeger)
        {
            if (inventory.InventoryControl_Sayi(btnDeger))
            {
                if (txt_islemYap_InputField.text.Length < InputFiedlMaxTextLenght)
                {
                    txt_islemYap_InputField.text += btnDeger;
                    inventory.Sayi_Cikar(btnDeger, 1);
                }
                else
                {
                    WarningUIChannel.RaiseEvent(BASAMAKASILDI, true);
                }
            }
            else
            {
                WarningUIChannel.RaiseEvent("Bu rakam envanterinde yok!");
            }
        }

        //btn_Geri
        public void btn_Geri()
        {
            gameObject.SetActive(false);
            Giris_UI.SetActive(true);
        }

        //btn_Sil
        public void btn_Sil()
        {
            if (txt_islemYap_InputField.text.Length == 0)
            {
                return;
            }
            else
            {
                //Son girilen yazıyı bul ve last input'a ata.
                string last_input = txt_islemYap_InputField.text.Substring(txt_islemYap_InputField.text.Length - 1);
                //Son girilen yazıyı sil.
                txt_islemYap_InputField.text = txt_islemYap_InputField.text.Remove(txt_islemYap_InputField.text.Length - 1);
                inventory.Sayi_Ekle(int.Parse(last_input), 1);
            }
        }

        //btn_Topla
        public void btn_Topla()
        {
            bool sonuc = int.TryParse(txt_islemYap_InputField.text, out int _sayi);
            if (sonuc && !cikarma && !toplama)
            {
                sayi1 = _sayi;
                txt_islemYap_InputField.text = "";
                txt_ReviewInput.text = "Girdiğiniz sayı \"" + sayi1.ToString() + "\" ve işlem Toplama.";
                cikarma = false;
                toplama = true;
                sonuc = false;
            }
            else if (!sonuc && toplama || cikarma)
            {
                WarningUIChannel.RaiseEvent("Önce seçtiğin işlemi tamamla veya iptal et!");
            }
            else
            {
                WarningUIChannel.RaiseEvent("Önce sayı girmelisin!");
            }
        }

        //btn_Çıkar
        public void btn_Cikar()
        {
            bool sonuc = int.TryParse(txt_islemYap_InputField.text, out int _sayi);
            if (sonuc && !cikarma && !toplama)
            {
                sayi1 = _sayi;
                txt_islemYap_InputField.text = "";
                txt_ReviewInput.text = "Girdiğiniz sayı \"" + sayi1.ToString() + "\" ve işlem Çıkarma.";
                cikarma = true;
                toplama = false;
                sonuc = false;
            }
            else if (!sonuc && toplama || cikarma)
            {
                WarningUIChannel.RaiseEvent("Önce seçtiğin işlemi tamamla veya iptal et!");
            }
            else
            {
                WarningUIChannel.RaiseEvent("Önce sayı girmelisin!");
            }
        }

        //btn_SonucuGoster
        public void btn_SonucuGoster()
        {
            if (toplama || cikarma)
            {
                int.TryParse(txt_islemYap_InputField.text, out int _sayi2);
                bool sonuc = islemiYap(sayi1, _sayi2, out int? islemSonucu);
                if (sonuc)
                {
                    txt_islemYap_InputField.text = islemSonucu.ToString();
                    txt_ReviewInput.text = "";
                    toplama = false;
                    cikarma = false;
                    sayi1 = null;
                }
                else if (txt_islemYap_InputField.text == "")
                {
                    WarningUIChannel.RaiseEvent("Bir \"Sayı\" girmelisin!");
                }
                else if (islemSonucu < 0)
                {
                    WarningUIChannel.RaiseEvent("İşlem sonucu 0'dan küçük olamaz!");
                }
                else
                {
                    WarningUIChannel.RaiseEvent("Bir terslik oldu. işlem seçip sayı girdiğinden emin misin!");
                }
            }
            else
            {
                WarningUIChannel.RaiseEvent("Öncelikle işlem seçip sayı girmelisin!");
            }
        }

        /// <summary>
        /// İşlem sonucu 0dan küçükse false döner.
        /// </summary>
        private bool islemiYap(int? _sayi1, int? _sayi2, out int? sonuc)
        {
            if (toplama)
            {
                sonuc = (int)_sayi1 + (int)_sayi2;
                return true;
            }
            else
            {
                if (_sayi1 - _sayi2 >= 0)
                {
                    sonuc = _sayi1 - _sayi2;
                    return true;
                }
                else
                {
                    sonuc = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Removes any number found in the txt_islemYap_InputField
        /// And clears current operations of operators.
        /// </summary>
        private void CancelOperation(ref int? _sayi1)
        {
            string str_sayi1 = _sayi1.ToString();
            while (str_sayi1.Length != 0)
            {
                string last_number = str_sayi1.Substring(str_sayi1.Length - 1);
                str_sayi1 = str_sayi1.Remove(str_sayi1.Length - 1);
                inventory.Sayi_Ekle(int.Parse(last_number), 1);
                if (str_sayi1.Length == 0)
                {
                    break;
                }
            }
            //sayi2 direkt inputField üzerinden alınıyor. Değişkene aktarılmıyor.
            while (txt_islemYap_InputField.text.Length != 0 || !string.IsNullOrEmpty(txt_islemYap_InputField.text))
            {
                string last_input = txt_islemYap_InputField.text.Substring(txt_islemYap_InputField.text.Length - 1);
                txt_islemYap_InputField.text = txt_islemYap_InputField.text.Remove(txt_islemYap_InputField.text.Length - 1);
                inventory.Sayi_Ekle(int.Parse(last_input), 1);
                if (txt_islemYap_InputField.text.Length == 0)
                {
                    break;
                }
            }
            _sayi1 = null;
            txt_ReviewInput.text = "";
            toplama = false;
            cikarma = false;
        }

        public void OnZero(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(0);
        }

        public void OnOne(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(1);
        }

        public void OnTwo(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(2);
        }

        public void OnThree(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(3);
        }

        public void OnFour(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(4);
        }

        public void OnFive(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(5);
        }

        public void OnSix(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(6);
        }

        public void OnSeven(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(7);
        }

        public void OnEight(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(8);
        }

        public void OnNine(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_OnClick(9);
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_SonucuGoster();
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_Geri();
        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                btn_Sil();
                deleteStarted = true;
            }

            if (context.canceled)
            {
                timer = 0;
                maintimer = 0;
                deleteStarted = false;
            }
        }

        public void OnMinus(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_Cikar();
        }

        public void OnPlus(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_Topla();
        }
    }
}