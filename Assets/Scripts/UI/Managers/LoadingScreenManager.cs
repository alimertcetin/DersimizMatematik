using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;

namespace LessonIsMath.UI
{
    public class LoadingScreenManager : MonoBehaviour
    {
        [SerializeField] BoolEventChannelSO showLoadingScreenChannel = default;
        [SerializeField] GameObject loadingUI;

        void OnEnable()
        {
            if (showLoadingScreenChannel != null) showLoadingScreenChannel.OnEventRaised += ToggleLoadingScreen;
        }

        void OnDisable()
        {
            if (showLoadingScreenChannel != null) showLoadingScreenChannel.OnEventRaised -= ToggleLoadingScreen;
        }

        void ToggleLoadingScreen(bool value)
        {
            loadingUI.SetActive(value);
        }

    }
}