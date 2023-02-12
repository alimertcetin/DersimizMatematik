using UnityEngine;
using TMPro;
using LessonIsMath.PlayerSystems;

namespace XIV.UI
{
    public class Settings_UI : MonoBehaviour
    {
        [SerializeField] private HUD_UI Hud = default;
        [SerializeField] private GameObject Main = default;
        [SerializeField] private TMP_Text Hud_Button_Text = default;
        [SerializeField] private TMP_Text Animation_Button_Text = default;

        private PlayerAnimationController playerAnimController;

        private void Awake()
        {
            playerAnimController = FindObjectOfType<PlayerAnimationController>();
        }

        public void btn_Hud()
        {
            Hud.gameObject.SetActive(!Hud.gameObject.activeSelf);
            Hud_Button_Text.text = Hud.gameObject.activeSelf ? "Hud Açık" : "Hud Kapalı";
        }

        public void btn_Animation()
        {
            playerAnimController.enabled = !playerAnimController.enabled;
            Animation_Button_Text.text = playerAnimController.enabled ? "Anim Açık" : "Anim Kapalı";
        }

        public void btn_Back()
        {
            Main.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}