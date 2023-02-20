using Cinemachine;
using LessonIsMath.Input;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraController : MonoBehaviour
{
    CinemachineFreeLook cameraLook;
    float CameraSpeed_X;
    float CameraSpeed_Y;

    private void Awake()
    {
        cameraLook = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        CameraSpeed_Y = cameraLook.m_YAxis.m_MaxSpeed;
        CameraSpeed_X = cameraLook.m_XAxis.m_MaxSpeed;

        InputManager.CharacterMovement.enabled += onGamePlayEnabled;
        InputManager.CharacterMovement.disabled += onGamePlayDisabled;
    }

    private void OnDisable()
    {
        InputManager.CharacterMovement.enabled -= onGamePlayEnabled;
        InputManager.CharacterMovement.disabled -= onGamePlayDisabled;
    }

    private void onGamePlayEnabled()
    {
        cameraLook.m_YAxis.m_MaxSpeed = CameraSpeed_Y;
        cameraLook.m_XAxis.m_MaxSpeed = CameraSpeed_X;
    }

    private void onGamePlayDisabled()
    {
        cameraLook.m_YAxis.m_MaxSpeed = 0;
        cameraLook.m_XAxis.m_MaxSpeed = 0;
    }
}
