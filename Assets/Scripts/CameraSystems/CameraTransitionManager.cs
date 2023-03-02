using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using LessonIsMath.ScriptableObjects.ChannelSOs;

namespace LessonIsMath.CameraSystems
{
    public enum CameraType
    {
        None = 0,
        Character,
        SideViewLeft,
        SideViewRight,
        Closeup,
    }

    static class CameraPriority
    {
        public static int Inactive = 0;
        public static int Override = 20;
    }

    public class CameraTransitionManager : MonoBehaviour
    {
        public static List<CameraDataContainer> CameraDataContainers = new List<CameraDataContainer>(4);
        [SerializeField] CameraTransitionEventChannelSO cameraTransitionChannel;

        void OnEnable()
        {
            cameraTransitionChannel.OnEventRaised += HandleTransition;
        }

        void OnDisable()
        {
            cameraTransitionChannel.OnEventRaised -= HandleTransition;
        }

        void HandleTransition(CameraType cameraType)
        {
            int count = CameraDataContainers.Count;
            for (int i = 0; i < count; i++)
            {
                var container = CameraDataContainers[i];
                if (container.CameraType != cameraType)
                {
                    container.VirtualCamera.Priority = CameraPriority.Inactive;
                    continue;
                }
                
                container.VirtualCamera.Priority = CameraPriority.Override;
            }
        }

        // Update is called once per frame
#if UNITY_EDITOR
        bool toggle;
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.B) == false) return;
            if (toggle) HandleTransition(CameraType.SideViewLeft);
            else HandleTransition(CameraType.Character);
            toggle = !toggle;
        }
#endif
    }
}