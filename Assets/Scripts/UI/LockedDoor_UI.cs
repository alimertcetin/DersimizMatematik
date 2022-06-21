using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace XIV.UI
{
    public class LockedDoor_UI : MonoBehaviour, PlayerControls.ILockedDoorUIActions
    {
        [Header("Broadcasting To")]
        [SerializeField]
        private StringEventChannelSO WarningUIChannel = default;

        public UnityAction SoruCevaplandi;
        private PlayerInventory inventory;
        private Door_Is_Locked LockedDoor_Script;

        [SerializeField] private TMP_Text txt_InputField = null;
        [SerializeField] private TMP_Text txt_Soru = null;
        [SerializeField] private int textMaxLenght = 14;

        private float maintimer;
        private float timer;
        private float controlEachDeleteTime = .3f;
        private bool deleteStarted = false;

        private void Awake()
        {
            inventory = FindObjectOfType<PlayerInventory>();
            InputManager.PlayerControls.LockedDoorUI.SetCallbacks(this);
        }

        private void OnEnable()
        {
            if (LockedDoor_Script != null)
            {
                txt_Soru.text = LockedDoor_Script.DoorLockedQuestion;
            }

            InputManager.LockedDoorUI.Enable();
            InputManager.GameManager.Disable();
            InputManager.GamePlay.Disable();
        }

        private void OnDisable()
        {
            TextiTamamenTemizle();

            InputManager.LockedDoorUI.Disable();
            InputManager.GameManager.Enable();
            InputManager.GamePlay.Enable();
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
                    Sil();
                    timer = 0;
                }
                if (maintimer > controlEachDeleteTime + 1)
                {
                    Sil();
                    timer = 0;
                }
            }
        }

        public void RecieveScriptFromDoor(Door_Is_Locked Door)
        {
            LockedDoor_Script = Door;
        }

        public void NumberOnClick(int value)
        {
            if (txt_InputField.text.Length > textMaxLenght)
            {
                UyariVer("Daha fazla sayı giremezsin.");
                return;
            }
            //Eğer değer envanterde varsa
            if (inventory.InventoryControl_Sayi(value))
            {
                txt_InputField.text += value.ToString();
                inventory.Sayi_Cikar(value, 1);
            }
            else
            {
                UyariVer("Bu rakam envanterinde yok!");
            }
        }

        public void TextiTamamenTemizle()
        {
            if (txt_InputField.text.Length != 0)
            {
                while (true)
                {
                    Sil();
                    if (txt_InputField.text.Length == 0)
                    {
                        break;
                    }
                }
            }
        }

        public void Sil()
        {
            if (txt_InputField.text.Length != 0)
            {
                //Son girilen yazıyı bul ve last input'a ata.
                string last_input = txt_InputField.text.Substring(txt_InputField.text.Length - 1);
                //Son girilen yazıyı sil.
                txt_InputField.text = txt_InputField.text.Remove(txt_InputField.text.Length - 1);
                inventory.Sayi_Ekle(int.Parse(last_input), 1);
            }
        }

        public void Cevapla()
        {
            if (txt_InputField.text == LockedDoor_Script.DoorLockedAnswer.ToString())
            {
                LockedDoor_Script.DoorLocked = false;
                txt_InputField.text = "";
                SoruCevaplandi?.Invoke();
                this.gameObject.SetActive(false);
            }
            else
            {
                UyariVer("Şifre Yanlış");
            }
        }

        private void UyariVer(string text, bool value = true)
        {
            WarningUIChannel.RaiseEvent(text, value);
        }

        public void OnZero(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(0);
        }

        public void OnOne(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(1);
        }

        public void OnTwo(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(2);
        }

        public void OnThree(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(3);
        }

        public void OnFour(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(4);
        }

        public void OnFive(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(5);
        }

        public void OnSix(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(6);
        }

        public void OnSeven(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(7);
        }

        public void OnEight(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(8);
        }

        public void OnNine(InputAction.CallbackContext context)
        {
            if (context.performed)
                NumberOnClick(9);
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed)
                Cevapla();
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
                this.gameObject.SetActive(false);
        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Sil();
                deleteStarted = true;
            }

            if (context.canceled)
            {
                timer = 0;
                maintimer = 0;
                deleteStarted = false;
            }
        }
    }
}