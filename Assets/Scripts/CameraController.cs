using System;
using Cinemachine;
using LessonIsMath.Input;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraController : MonoBehaviour
{
    CinemachineFreeLook cameraLook;
    float CameraSpeed_X;
    float CameraSpeed_Y;

    void Awake()
    {
        cameraLook = GetComponent<CinemachineFreeLook>();
    }

    void OnEnable()
    {
        CameraSpeed_Y = cameraLook.m_YAxis.m_MaxSpeed;
        CameraSpeed_X = cameraLook.m_XAxis.m_MaxSpeed;

        InputManager.CharacterMovement.OnEnable += OnGamePlayEnabled;
        InputManager.CharacterMovement.OnDisable += OnGamePlayDisabled;
    }

    void OnDisable()
    {
        InputManager.CharacterMovement.OnEnable -= OnGamePlayEnabled;
        InputManager.CharacterMovement.OnDisable -= OnGamePlayDisabled;
    }
    
#if UNITY_EDITOR
    bool toggle;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (toggle) OnGamePlayEnabled();
            else OnGamePlayDisabled();
            toggle = !toggle;
        }
    }
#endif

    void OnGamePlayEnabled()
    {
        cameraLook.m_YAxis.m_MaxSpeed = CameraSpeed_Y;
        cameraLook.m_XAxis.m_MaxSpeed = CameraSpeed_X;
    }

    void OnGamePlayDisabled()
    {
        cameraLook.m_YAxis.m_MaxSpeed = 0;
        cameraLook.m_XAxis.m_MaxSpeed = 0;
    }
}
