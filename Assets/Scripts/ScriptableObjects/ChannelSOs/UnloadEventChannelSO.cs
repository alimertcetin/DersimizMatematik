using LessonIsMath.ScriptableObjects.SceneSOs;
using UnityEngine;
using UnityEngine.Events;
using XIV.ScriptableObjects.Channels;

namespace LessonIsMath.ScriptableObjects.ChannelSOs
{

    /// <summary>
    /// This class is used for scene-loading events.
    /// Takes a GameSceneSO of the location or menu that needs to be loaded, and a bool to specify if a loading screen needs to display.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/UnLoad Event Channel")]
    public class UnloadEventChannelSO : EventChannelBaseSO
    {
        public UnityAction<GameSceneSO> OnUnLoadingRequested;

        public void RaiseEvent(GameSceneSO locationToUnLoad)
        {
            if (OnUnLoadingRequested != null)
            {
                OnUnLoadingRequested.Invoke(locationToUnLoad);
            }
            else
            {
                Debug.LogWarning("A Scene loading was requested, but nobody picked it up. " +
                    "Check why there is no SceneLoader already present, " +
                    "and make sure it's listening on this Load Event channel.");
            }
        }
    }
}