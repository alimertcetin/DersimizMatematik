using System;
using Cinemachine;
using UnityEngine;

namespace LessonIsMath.CameraSystems
{
    public class CameraDataContainer : MonoBehaviour
    {
        [SerializeField] CameraType cameraType;
        CinemachineVirtualCameraBase virtualCamera;
        public CameraType CameraType => cameraType;
        public CinemachineVirtualCameraBase VirtualCamera => virtualCamera;

        void Awake()
        {
            virtualCamera = GetComponent<CinemachineVirtualCameraBase>();
            if (virtualCamera == null) Debug.LogError("There is no cinemachine virtual camera on this gameObject!");
        }

        void OnEnable()
        {
            CameraTransitionManager.CameraDataContainers.Add(this);
        }

        void OnDisable()
        {
            CameraTransitionManager.CameraDataContainers.Remove(this);
        }
    }
}