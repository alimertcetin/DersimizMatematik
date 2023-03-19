using System;
using Cinemachine;
using UnityEngine;

namespace LessonIsMath.CameraSystems
{
    public class CameraDataContainer : MonoBehaviour
    {
        [SerializeField] CameraType cameraType;
        [SerializeField] CinemachineVirtualCameraBase virtualCamera;
        public CameraType CameraType => cameraType;
        public CinemachineVirtualCameraBase VirtualCamera => virtualCamera;
        
#if UNITY_EDITOR
        void OnValidate()
        {
            virtualCamera = GetComponent<CinemachineVirtualCameraBase>();
            if (virtualCamera == null) Debug.LogError("There is no cinemachine virtual camera on this gameObject!");
        }
#endif

        void OnEnable()
        {
            CameraTransitionManager.CameraDataContainers.Add(cameraType, this);
        }

        void OnDisable()
        {
            CameraTransitionManager.CameraDataContainers.Remove(cameraType);
        }
    }
}