using System.Collections;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.ScriptableObjects.SceneSOs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LessonIsMath.SceneManagement
{
    /// <summary>
    /// This class is responsible for starting the game by loading the persistent managers scene 
    /// and raising the event to load the Main Menu
    /// </summary>
    public class InitializationLoader : MonoBehaviour
    {
        [SerializeField] GameSceneSO _managersScene;
        [SerializeField] GameSceneSO _menuToLoad;

        [Header("Broadcasting on")]
        public AssetReference _menuLoadChannel;

        void Start()
        {
            InputManager.DisableAllInput();
            CursorManager.UnlockCursor();

            //Load the persistent managers scene
            _managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
        }

        void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            _menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
        }

        void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
        {
            SceneLoader.onMenuChannelLoaded.Invoke(obj.Result);
            StartCoroutine(raiseEvent(obj.Result));
        }

        IEnumerator raiseEvent(LoadEventChannelSO eventChannelSO)
        {
            yield return new WaitForEndOfFrame();

            eventChannelSO.RaiseEvent(_menuToLoad);

            SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
        }
    }
}