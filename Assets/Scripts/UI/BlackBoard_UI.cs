using UnityEngine;
using UnityEngine.InputSystem;

namespace XIV.UI
{
    public class BlackBoard_UI : MonoBehaviour, PlayerControls.IBlackBoardUIManagementActions
    {
        [SerializeField] private GameObject Giris_UI = null;
        [SerializeField] private GameObject islemYap_UI = null;
        [SerializeField] private GameObject SayiAl_UI = null;
        [SerializeField] private BoolEventChannelSO BlackBoardUIChannel = default;
        private PlayerInventory inventory;

        private void Awake()
        {
            inventory = FindObjectOfType<PlayerInventory>();
            InputManager.PlayerControls.BlackBoardUIManagement.SetCallbacks(this);
        }

        private void OnEnable()
        {
            InputManager.BlackBoardUIManagement.Enable();
            InputManager.GameManager.Disable();
            InputManager.GamePlay.Disable();
            Giris_UI.SetActive(true);
        }

        private void OnDisable()
        {
            islemYap_UI.SetActive(false);
            SayiAl_UI.SetActive(false);

            InputManager.BlackBoardUIManagement.Disable();
            InputManager.GameManager.Enable();
            InputManager.GamePlay.Enable();
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
                btn_Exit();
        }

        #region UI Yönetimi için

        //btn_SayiAl
        public void btn_SayiAl()
        {
            Giris_UI.SetActive(false);
            SayiAl_UI.SetActive(true);
        }

        //btn_islemYap
        public void btn_IslemYap()
        {
            Giris_UI.SetActive(false);
            islemYap_UI.SetActive(true);
        }

        //btn_Cikis
        public void btn_Exit()
        {
            InputManager.BlackBoardUIManagement.Disable();
            InputManager.GameManager.Enable();
            InputManager.GamePlay.Enable();
            BlackBoardUIChannel.RaiseEvent(false);
        }

        #endregion
    }

}
