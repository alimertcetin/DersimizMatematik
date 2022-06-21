using UnityEngine;
using UnityEngine.SceneManagement;

namespace XIV.UI
{
    public class Main_Menu_UI : MonoBehaviour
    {
        [SerializeField] private GameObject Settings = null;
        [SerializeField] private GameObject Main = null;

        public void btn_Start() => SceneManager.LoadScene(1);

        public void btn_Settings()
        {
            Settings.SetActive(true);
            Main.SetActive(false);
        }

        public void btn_Exit() => Application.Quit();

        //public void btn_Back()
        //{
        //    if (SettingsMenu.activeSelf && !Main.activeSelf)
        //    {
        //        SettingsMenu.SetActive(false);
        //        Main.SetActive(true);
        //    }
        //}

    }
}