using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XIV.UI;

namespace GameCore.UI
{
    public class BlackboardMainUI : MonoBehaviour, PlayerControls.IBlackBoardUIManagementActions
    {
        [SerializeField] private GameObject blackBoardUI = null;
        [SerializeField] private GameObject blackBoardMainPageUI = null;
        [SerializeField] private MakeOperationUI makeOperationUI = null;
        [SerializeField] private EarnNumberUI earnNumberUI = null;
        [SerializeField] private Button btn_makeOperation;
        [SerializeField] private Button btn_earnNumber;
        [SerializeField] private Button btn_exit;

        private void Awake()
        {
            InputManager.PlayerControls.BlackBoardUIManagement.SetCallbacks(this);
        }

        public void ShowUI()
        {
            btn_makeOperation.onClick.AddListener(ShowMakeOperationUI);
            btn_earnNumber.onClick.AddListener(ShowEarnNumberUI);
            btn_exit.onClick.AddListener(CloseUI);

            InputManager.BlackBoardUIManagement.Enable();
            InputManager.GameManager.Disable();
            InputManager.GamePlay.Disable();
            blackBoardUI.gameObject.SetActive(true);
            blackBoardMainPageUI.SetActive(true);
        }

        public void CloseUI()
        {
            btn_makeOperation.onClick.RemoveListener(ShowMakeOperationUI);
            btn_earnNumber.onClick.RemoveListener(ShowEarnNumberUI);
            btn_exit.onClick.RemoveListener(CloseUI);

            InputManager.BlackBoardUIManagement.Disable();
            InputManager.GameManager.Enable();
            InputManager.GamePlay.Enable();
            blackBoardUI.gameObject.SetActive(false);
            blackBoardMainPageUI.SetActive(false);
        }

        void PlayerControls.IBlackBoardUIManagementActions.OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) CloseUI();
        }

        void ShowEarnNumberUI()
        {
            blackBoardMainPageUI.SetActive(false);
            earnNumberUI.ShowUI();
        }

        void ShowMakeOperationUI()
        {
            blackBoardMainPageUI.SetActive(false);
            makeOperationUI.ShowUI();
        }
    }
}
