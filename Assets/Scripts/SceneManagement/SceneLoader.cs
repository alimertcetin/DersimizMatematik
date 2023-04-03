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
    /// <summary>
    /// This class manages the scene loading and unloading.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static UnityAction<LoadEventChannelSO> onMenuChannelLoaded;

        [SerializeField] GameSceneSO _gameplayScene;

        [Header("Load Events")]
        [SerializeField]
        LoadEventChannelSO _loadLocation;

        [SerializeField] LoadEventChannelSO _loadMenu;
        [SerializeField] LoadEventChannelSO _coldStartupLocation;

        [Header("Broadcasting on")]
        [SerializeField]
        BoolEventChannelSO _toggleLoadingScreen;

        [SerializeField] VoidEventChannelSO _onSceneReady;

        AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
        AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

        //Parameters coming from scene loading requests
        GameSceneSO _sceneToLoad;
        GameSceneSO _currentlyLoadedScene;
        bool _showLoadingScreen;

        SceneInstance _gameplayManagerSceneInstance;

        void OnEnable()
        {
            onMenuChannelLoaded += LoadmenuChannelLoaded;
#if UNITY_EDITOR
            _coldStartupLocation.OnLoadingRequested += LocationColdStartup;
#endif
        }

        void OnDisable()
        {
            onMenuChannelLoaded -= LoadmenuChannelLoaded;
            _loadLocation.OnLoadingRequested -= LoadLocation;
            _loadMenu.OnLoadingRequested -= LoadMenu;
#if UNITY_EDITOR
            _coldStartupLocation.OnLoadingRequested -= LocationColdStartup;
#endif
        }

        void LoadmenuChannelLoaded(LoadEventChannelSO arg0)
        {
            _loadMenu = arg0;
            _loadLocation.OnLoadingRequested += LoadLocation;
            _loadMenu.OnLoadingRequested += LoadMenu;
        }

#if UNITY_EDITOR
        /// <summary>
        /// This special loading function is only used in the editor, when the developer presses Play in a Location scene, without passing by Initialisation.
        /// </summary>
        void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen)
        {
            _currentlyLoadedScene = currentlyOpenedLocation;

            if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
            {
                //Gameplay managers is loaded synchronously
                _gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.WaitForCompletion();
                _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

                StartGameplay();
            }
        }
#endif

        /// <summary>
        /// This function loads the location scenes passed as array parameter
        /// </summary>
        void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen)
        {
            _sceneToLoad = locationToLoad;
            _showLoadingScreen = showLoadingScreen;

            //In case we are coming from the main menu, we need to load the Gameplay manager scene first
            if (_gameplayManagerSceneInstance.Scene == null || !_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplayMangersLoaded;
            }
            else
            {
                UnloadPreviousScene();
            }
        }

        void OnGameplayMangersLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

            UnloadPreviousScene();
        }

        /// <summary>
        /// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
        /// </summary>
        void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen)
        {
            _sceneToLoad = menuToLoad;
            _showLoadingScreen = showLoadingScreen;

            //In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
            if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded)
                Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);

            UnloadPreviousScene();
        }

        /// <summary>
        /// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
        /// </summary>
        void UnloadPreviousScene()
        {
            if (_currentlyLoadedScene != null) //would be null if the game was started in Initialisation
            {
                if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
                {
                    //Unload the scene through its AssetReference, i.e. through the Addressable system
                    _currentlyLoadedScene.sceneReference.UnLoadScene();
                }
#if UNITY_EDITOR
                else
                {
                    //Only used when, after a "cold start", the player moves to a new scene
                    //Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
                    //the scene needs to be unloaded using regular SceneManager instead of as an Addressable
                    SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
                }
#endif
            }

            StartCoroutine(LoadNewScene());
        }

        /// <summary>
        /// Kicks off the asynchronous loading of a scene, either menu or Location.
        /// </summary>
        IEnumerator LoadNewScene()
        {
            if (_showLoadingScreen)
            {
                _toggleLoadingScreen.RaiseEvent(true);
            }

            _loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            yield return _loadingOperationHandle;

            _currentlyLoadedScene = _sceneToLoad;
            yield return new WaitForSeconds(5);
            SetActiveScene();
            if (_showLoadingScreen)
            {
                _toggleLoadingScreen.RaiseEvent(false);
            }
            //_loadingOperationHandle.Completed += OnNewSceneLoaded;
        }

        void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            //Save loaded scenes (to be unloaded at next load request)
            _currentlyLoadedScene = _sceneToLoad;
            SetActiveScene();

            if (_showLoadingScreen)
            {
                _toggleLoadingScreen.RaiseEvent(false);
            }
        }

        /// <summary>
        /// This function is called when all the scenes have been loaded
        /// </summary>
        void SetActiveScene()
        {
            Scene s = _loadingOperationHandle.Result.Scene;
            SceneManager.SetActiveScene(s);

            LightProbes.TetrahedralizeAsync();

            StartGameplay();
        }

        void StartGameplay()
        {
            _onSceneReady.RaiseEvent(); //Spawn system will spawn the PigChef
        }

        void ExitGame()
        {
            Application.Quit();
            Debug.Log("Exit!");
        }
    }
}