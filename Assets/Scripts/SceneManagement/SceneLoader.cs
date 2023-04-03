using System.Collections;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.ScriptableObjects.SceneSOs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LessonIsMath.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public static UnityAction<LoadEventChannelSO> menuChannelLoaded;
        [SerializeField] GameSceneSO gamePlaySceneSO;

        [Header("Load Events")]
        [SerializeField] LoadEventChannelSO loadLocationChannel;
        [SerializeField] LoadEventChannelSO loadMenuChannel;

        [Header("Broadcasting on")]
        [SerializeField] BoolEventChannelSO showLoadingScreenChannel;
        [SerializeField] VoidEventChannelSO onSceneReadyChannel;

        AsyncOperationHandle<SceneInstance> sceneLoadOperation;
        AsyncOperationHandle<SceneInstance> persistantManagerLoadOperation;

        //Parameters coming from scene loading requests
        GameSceneSO sceneToLoad;
        GameSceneSO currentScene;
        bool showLoadingScreen;

        SceneInstance persistantManagerSceneInstance;

        void OnEnable()
        {
            menuChannelLoaded += OnMenuChannelLoaded;
        }

        void OnMenuChannelLoaded(LoadEventChannelSO loadMenuChannel)
        {
            this.loadMenuChannel = loadMenuChannel;
            loadLocationChannel.OnLoadingRequested += LoadLocation;
            this.loadMenuChannel.OnLoadingRequested += LoadMenu;
        }

        void OnDisable()
        {
            menuChannelLoaded -= OnMenuChannelLoaded;
            loadLocationChannel.OnLoadingRequested -= LoadLocation;
            loadMenuChannel.OnLoadingRequested -= LoadMenu;
        }

        void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen)
        {
            sceneToLoad = locationToLoad;
            this.showLoadingScreen = showLoadingScreen;

            //In case we are coming from the main menu, we need to load the Persistant Manager scene first
            if (persistantManagerSceneInstance.Scene.isLoaded == false)
            {
                persistantManagerLoadOperation = gamePlaySceneSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                persistantManagerLoadOperation.Completed += OnPersistantMangersLoaded;
            }
            else
            {
                UnloadPreviousScene();
            }
        }

        void OnPersistantMangersLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            persistantManagerSceneInstance = persistantManagerLoadOperation.Result;

            UnloadPreviousScene();
        }

        void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen)
        {
            sceneToLoad = menuToLoad;
            this.showLoadingScreen = showLoadingScreen;

            //In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
            if (persistantManagerSceneInstance.Scene != null && persistantManagerSceneInstance.Scene.isLoaded)
                Addressables.UnloadSceneAsync(persistantManagerLoadOperation, true);

            UnloadPreviousScene();
        }

        void UnloadPreviousScene()
        {
            if (currentScene != null) //would be null if the game was started in Initialisation
            {
                if (currentScene.sceneReference.OperationHandle.IsValid())
                {
                    //Unload the scene through its AssetReference, i.e. through the Addressable system
                    currentScene.sceneReference.UnLoadScene();
                }
            }

            StartCoroutine(LoadNewScene());
        }

        IEnumerator LoadNewScene()
        {
            if (showLoadingScreen)
            {
                showLoadingScreenChannel.RaiseEvent(true);
            }

            sceneLoadOperation = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            yield return sceneLoadOperation;

            currentScene = sceneToLoad;
            SetActiveScene();
            if (showLoadingScreen)
            {
                showLoadingScreenChannel.RaiseEvent(false);
            }
        }

        void SetActiveScene()
        {
            Scene s = sceneLoadOperation.Result.Scene;
            SceneManager.SetActiveScene(s);

            LightProbes.TetrahedralizeAsync();

            onSceneReadyChannel.RaiseEvent();
        }
        
        void ExitGame()
        {
            Application.Quit();
            Debug.Log("Exit!");
        }
    }
}