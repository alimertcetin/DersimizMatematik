using UnityEngine;
using TMPro;
using LessonIsMath.PlayerSystems;
using XIV.UI;

namespace LessonIsMath.UI
{
    public class Settings_UI : GameUI
    {
        [SerializeField] private TMP_Text Hud_Button_Text = default;
        [SerializeField] private TMP_Text Animation_Button_Text = default;
        private PlayerAnimationController playerAnimController;

        protected override void Awake()
        {
            base.Awake();
            playerAnimController = FindObjectOfType<PlayerAnimationController>();
        }

        public void btn_Hud()
        {
            var hud = UISystem.GetUI<HUD_UI>();
            if (hud.isActive)
            {
                UISystem.Hide<HUD_UI>();
            }
            else
            {
                UISystem.Show<HUD_UI>();
            }
            Hud_Button_Text.text = hud.isActive ? "Hud Açık" : "Hud Kapalı";
        }

        public void btn_Animation()
        {
            playerAnimController.enabled = !playerAnimController.enabled;
            Animation_Button_Text.text = playerAnimController.enabled ? "Anim Açık" : "Anim Kapalı";
        }

        public void btn_Back()
        {
            UISystem.Show<PausedMenu_UI>();
        }
    }
}