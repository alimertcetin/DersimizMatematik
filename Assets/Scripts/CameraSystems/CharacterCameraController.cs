using Cinemachine;
using LessonIsMath.Input;
using UnityEngine;

namespace LessonIsMath.CameraSystems
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class CharacterCameraController : MonoBehaviour
    {
        CinemachineFreeLook cinemachineFreeLook;
        Vector2 camSpeed;

        void Awake()
        {
            cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
            camSpeed.y = cinemachineFreeLook.m_YAxis.m_MaxSpeed;
            camSpeed.x = cinemachineFreeLook.m_XAxis.m_MaxSpeed;
        }

        void OnEnable()
        {
            cinemachineFreeLook.m_Transitions.m_OnCameraLive.AddListener(OnCameraLive);
            InputManager.CharacterMovement.OnEnable += OnCharacterMovementEnabled;
            InputManager.CharacterMovement.OnDisable += OnCharacterMovementDisabled;
        }

        void OnDisable()
        {
            cinemachineFreeLook.m_Transitions.m_OnCameraLive.RemoveListener(OnCameraLive);
            InputManager.CharacterMovement.OnEnable -= OnCharacterMovementEnabled;
            InputManager.CharacterMovement.OnDisable -= OnCharacterMovementDisabled;
        }

        void OnCameraLive(ICinemachineCamera incoming, ICinemachineCamera outgoing)
        {
            if (incoming == cinemachineFreeLook)
            {
                var rotation = cinemachineFreeLook.Follow.rotation;
                Vector3 offset = rotation * new Vector3(0, 10f, -10);
                cinemachineFreeLook.ForceCameraPosition(cinemachineFreeLook.Follow.position + offset, rotation);
            }
        }

        void OnCharacterMovementEnabled()
        {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = camSpeed.y;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = camSpeed.x;
        }

        void OnCharacterMovementDisabled()
        {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
        }
    }
}
