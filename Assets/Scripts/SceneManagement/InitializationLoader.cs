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
    public class InitializationLoader : MonoBehaviour
    {
        [SerializeField] GameSceneSO persistantManagerSceneSO;
        [SerializeField] GameSceneSO menuToLoad;

        [Header("Broadcasting on")]
        [SerializeField] AssetReference menuLoadChannel;

        void Start()
        {
            InputManager.DisableAllInput();
            CursorManager.UnlockCursor();

            //Load the persistent managers scene
            persistantManagerSceneSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
        }

        void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
        }

        void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
        {
            SceneLoader.menuChannelLoaded.Invoke(obj.Result);
            StartCoroutine(raiseEvent(obj.Result));
        }

        IEnumerator raiseEvent(LoadEventChannelSO eventChannelSO)
        {
            yield return new WaitForEndOfFrame();

            eventChannelSO.RaiseEvent(menuToLoad);

            SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
        }
    }
}