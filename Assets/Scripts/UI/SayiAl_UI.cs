using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XIV.UI
{
    public class SayiAl_UI : MonoBehaviour, PlayerControls.ISayiAlUIActions
    {
        [Header("Broadcasting To")]
        [SerializeField]
        private StringEventChannelSO WarningUIChannel = default;
        private PlayerInventory inventory;

        [SerializeField] private GameObject Giris_UI = null;

        [SerializeField]
        private TMP_Text txt_SayiAl_InputField = null, txt_Soru = null;

        [Tooltip("Input field'a maksimum bu kadar basamak girilebilir.")]
        [SerializeField] private readonly int InputFiedlMaxTextLenght = 7;
        [Tooltip("Soru üretilirken maksimum üretilecek sayı değeri")]
        [SerializeField] private readonly int MaxSayiDegeri = 999;
        private int cevap;
        private const string BASAMAKASILDI = "Daha fazla basamak giremezsin.";

        private float maintimer;
        private float timer;
        private bool deleteStarted;
        private float controlEachDeleteTime = .2f;

        private void Start()
        {
            InputManager.PlayerControls.SayiAlUI.SetCallbacks(this);
        }

        private void OnEnable()
        {
            InputManager.SayiAlUI.Enable();
            InputManager.BlackBoardUIManagement.Disable();

            btn_SoruUret();
        }

        private void OnDisable()
        {
            txt_SayiAl_InputField.text = "";

            InputManager.SayiAlUI.Disable();
            InputManager.BlackBoardUIManagement.Enable();
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

        //Sayı butonları için
        public void btn_OnClick(int btnDeger)
        {
            if (txt_SayiAl_InputField.text.Length < InputFiedlMaxTextLenght)
            {
                txt_SayiAl_InputField.text += btnDeger;
            }
            else
            {
                UyariVer(BASAMAKASILDI);
            }
        }

        //btn_Geri
        public void btn_Geri()
        {
            gameObject.SetActive(false);
            txt_SayiAl_InputField.text = "";
            Giris_UI.SetActive(true);
        }

        //btn_SoruUret
        public void btn_SoruUret()
        {
            int OperatorChance = Random.Range(0, 2);
            int sayi1 = Random.Range(0, MaxSayiDegeri);
            int sayi2 = Random.Range(0, MaxSayiDegeri);
            string Operator = OperatorChance == 0 ? "+" : "-";
            if (OperatorChance == 0)
            {
                cevap = sayi1 + sayi2;
            }
            else
            {
                while (sayi1 - sayi2 < 0)
                {
                    sayi1 = Random.Range(0, MaxSayiDegeri);
                    sayi2 = Random.Range(0, MaxSayiDegeri);
                    if (sayi1 - sayi2 > 0)
                    {
                        break;
                    }
                }
                cevap = sayi1 - sayi2;
            }
            txt_Soru.text = $"Soru : {sayi1} {Operator} {sayi2}";
        }

        //btn_Sil
        public void btn_Sil()
        {
            if (txt_SayiAl_InputField.text != "")
            {
                txt_SayiAl_InputField.text = txt_SayiAl_InputField.text.Remove(txt_SayiAl_InputField.text.Length - 1);
            }
        }

        //btn_Cevapla
        public void btn_Cevapla()
        {
            if (inventory.Inventory_Capacity > 0)
            {
                if (txt_SayiAl_InputField.text == "")
                {
                    UyariVer("Boş bırakmana gerek yok, puan kaybetmezsin :)");
                }
                else if (txt_SayiAl_InputField.text == cevap.ToString())
                {
                    int _rndNumber = Random.Range(0, 10);
                    inventory.Sayi_Ekle(_rndNumber, 1);
                    UyariVer($"Kazandığın Sayı \"{_rndNumber}\"");

                    txt_SayiAl_InputField.text = ""; //Input alanındaki texti temizle.
                    btn_SoruUret(); //Rastgele başka bir işlem üret.
                }
                else
                {
                    UyariVer("Yanlış Cevap!");
                }
            }
            else
            {
                txt_SayiAl_InputField.text = "";
                UyariVer("Envanterin Dolu.");
            }
        }

        private void UyariVer(string warningText, bool value = true)
        {
            WarningUIChannel.RaiseEvent(warningText, value);
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

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_Geri();
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_Cevapla();
        }

        public void OnGenerateQuestion(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_SoruUret();
        }
    }
}