using System;
using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LessonIsMath.PlayerSystems
{
    public class PlayerInputProvider : MonoBehaviour, PlayerControls.ICharacterMovementActions
    {
        AutoMovementInput autoMovementInput;
        Vector2 input;
        Camera mainCam;

        public bool JumpPressed { get; set; }
        public bool RunPressed { get; private set; }
        float turnVelocity;

        void Awake()
        {
            autoMovementInput = GetComponent<AutoMovementInput>();
            mainCam = Camera.main;
            InputManager.CharacterMovement.SetCallbacks(this);
        }

        Vector2 GetRequiredInput(Vector3 direction)
        {
            if (direction.magnitude > Mathf.Epsilon)
            {
                float angle = Mathf.Atan2(direction.x, direction.z) - Camera.main.transform.eulerAngles.y * Mathf.Deg2Rad;
                return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            }

            return Vector2.zero;
        }

        public Vector3 GetMovementVector()
        {
            if (autoMovementInput.hasTarget) return autoMovementInput.GetMovementVector(transform.position);
            
            return this.input.magnitude < Mathf.Epsilon ? Vector3.zero : Quaternion.Euler(0f, GetTargetAngle(this.input), 0f) * Vector3.forward;
        }

        public Vector3 GetMovementVector(Vector3 direction)
        {
            Vector2 input = GetRequiredInput(direction);
            return Quaternion.Euler(0f, GetTargetAngle(input), 0f) * Vector3.forward;
        }

        float GetTargetAngle(Vector2 input)
        {
            return Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        }

        public Quaternion GetRotation()
        {
            var input = this.input;
            if (autoMovementInput.hasTarget)
            {
                input = GetRequiredInput(autoMovementInput.GetRotationDirection(transform.position, transform.forward));
            }
            
            if (input.magnitude < Mathf.Epsilon) return transform.rotation;
            
            var newAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, GetTargetAngle(input), ref turnVelocity, 0.15f);
            return Quaternion.Euler(0f, newAngle, 0f);
        }
        
        void PlayerControls.ICharacterMovementActions.OnMove(InputAction.CallbackContext context)
        {
            input = context.ReadValue<Vector2>();
            if (autoMovementInput.hasTarget && context.performed) autoMovementInput.CancelTarget();
        }

        void PlayerControls.ICharacterMovementActions.OnJump(InputAction.CallbackContext context)
        {
            if (context.performed == false) return;
            JumpPressed = true;
        }

        void PlayerControls.ICharacterMovementActions.OnRun(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RunPressed = true;
                return;
            }
            if (context.canceled) RunPressed = false;
        }
    }
}