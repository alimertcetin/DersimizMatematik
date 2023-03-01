using System;
using System.Collections.Generic;
using LessonIsMath.Input;
using LessonIsMath.InteractionSystems;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV;
using XIV.SaveSystems;
using XIV.XIVMath;

namespace LessonIsMath.PlayerSystems
{
    public class PlayerController : MonoBehaviour, PlayerControls.ICharacterMovementActions, ISaveable
    {
        [SerializeField] float inputSensitivity = 2f;
        [SerializeField] float moveSpeed = 2.1f;
        [SerializeField] float runSpeed = 5;
        [SerializeField] float jumpForce = 3.2f;
        [SerializeField] float gravityScale = .7f;
        [SerializeField] float maxGravityForce = -3;
        PlayerAnimationController playerAnimationController;
        CharacterController charaacterController;
        Camera mainCam;

        float turnVelocity;
        float storedVerticalAcceleration;
        float speed;
        float normalizedSpeed => speed > moveSpeed ? speed / runSpeed : (speed / moveSpeed) * 0.5f;
        const float SPEED_THRESHOLD = 0.2f;
        
        bool movingTowardsTarget;
        InteractionData interactionData;
        Vector2 input;

        bool jumpPressed { get; set; }
        bool runPressed { get; set; }
        bool playJump;

        void Awake()
        {
            charaacterController = GetComponent<CharacterController>();
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
            InputManager.CharacterMovement.SetCallbacks(this);
            mainCam = Camera.main;
        }

        void OnEnable() => InputManager.CharacterMovement.Enable();
        void OnDisable() => InputManager.CharacterMovement.Disable();

        void FixedUpdate()
        {
            if (movingTowardsTarget == false)
            {
                HandleMovement();
                if (input.magnitude > Mathf.Epsilon) transform.rotation = GetRotation(input);
                return;
            }

            // TODO : Implement path finding
            Vector3 transformPosition = transform.position;
            Vector3 startPos = interactionData.targetData.startPos;
            Vector3 endPos = interactionData.targetData.targetPosition;
            Vector3 middlePos = endPos + (interactionData.targetData.targetForwardDirection * 0.2f);
#if UNITY_EDITOR
            XIVDebug.DrawBezier(startPos, middlePos, middlePos, endPos, Color.green, 2f, 20);
#endif
            var t = BezierMath.GetTime(transformPosition, startPos, middlePos, middlePos, endPos);
            t += 0.1f;
            if (t < 1)
            {
                var targetPos = BezierMath.GetPoint(startPos, middlePos, middlePos, endPos, t);
                input = GetRequiredInput((targetPos - transformPosition).normalized);
            }
            else
            {
                // Distance is valid but we have to make sure rotation is also valid
                var forward = transform.forward;
                var dot = Vector3.Dot(forward, -interactionData.targetData.targetForwardDirection);
                if (dot < 0.6f)
                {
#if UNITY_EDITOR
                    Debug.Log("Interactable is not in front of player");
                    XIVDebug.DrawSphere(interactionData.targetData.targetPosition, 0.25f, 0.25f);
#endif
                    input = GetRequiredInput((interactionData.targetData.targetPosition - transformPosition).normalized);
                    transform.rotation = GetRotation(input);
                    return;
                }

                movingTowardsTarget = false;
                interactionData.OnTargetReached?.Invoke();
                input = Vector2.zero;
            }
            
            HandleMovement();
            if (input.magnitude > Mathf.Epsilon) transform.rotation = GetRotation(input);
        }

        void Update()
        {
            HandleAnimation();
        }

        Vector3 GetMovementVector(Vector2 input)
        {
            return Quaternion.Euler(0f, GetTargetAngle(input), 0f) * Vector3.forward * speed;
        }

        float GetTargetAngle(Vector2 input)
        {
            return Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        }

        Quaternion GetRotation(Vector2 input)
        {
            var newAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, GetTargetAngle(input), ref turnVelocity, 0.15f);
            return Quaternion.Euler(0f, newAngle, 0f);
        }
        
        void HandleMovement()
        {
            Vector3 movement = Vector3.zero;
            
            if (input.magnitude > Mathf.Epsilon && playerAnimationController.IsJumpPlaying() == false)
            {
                speed = Mathf.MoveTowards(speed, runPressed ? runSpeed : moveSpeed, Time.deltaTime * inputSensitivity);
                if (speed > SPEED_THRESHOLD) movement = GetMovementVector(input);
            }
            else
            {
                if (speed > 0)
                {
                    speed = Mathf.MoveTowards(speed, 0, Time.deltaTime * (inputSensitivity * 2f));
                    movement = GetMovementVector(GetRequiredInput(transform.forward));
                }
            }
            
            movement.y = storedVerticalAcceleration;
            movement.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            if (movement.y < maxGravityForce) movement.y = maxGravityForce;

            if (charaacterController.isGrounded && jumpPressed && playerAnimationController.IsJumpPlaying() == false)
            {
                movement.y = jumpForce;
                playJump = true;
            }

            charaacterController.Move(movement * Time.deltaTime);
            storedVerticalAcceleration = movement.y;
            jumpPressed = false;
        }

        void HandleAnimation()
        {
            if (playJump)
            {
                playerAnimationController.PlayJump();
                playJump = false;
            }
            else
            {
                playerAnimationController.PlayLocomotion(normalizedSpeed);
            }
        }
        
        Vector2 GetRequiredInput(Vector3 direction)
        {
            if (direction.magnitude > Mathf.Epsilon)
            {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg - Camera.main.transform.eulerAngles.y;
                return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            }

            return Vector2.zero;
        }

        public void SetTarget(InteractionData interactionData)
        {
            CancelTarget();
            this.interactionData = interactionData;
            movingTowardsTarget = true;
        }

        public void CancelTarget()
        {
            if (movingTowardsTarget == false) return;
            
            interactionData.OnMovementCanceled?.Invoke();
            movingTowardsTarget = false;
            input = Vector2.zero;
        }

        void PlayerControls.ICharacterMovementActions.OnMove(InputAction.CallbackContext context)
        {
            input = context.ReadValue<Vector2>();
            if (movingTowardsTarget) interactionData.OnMovementCanceled?.Invoke();
            movingTowardsTarget = false;
        }

        void PlayerControls.ICharacterMovementActions.OnJump(InputAction.CallbackContext context)
        {
            if (context.performed == false) return;
            jumpPressed = true;
        }

        void PlayerControls.ICharacterMovementActions.OnRun(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                runPressed = true;
                return;
            }
            if (context.canceled) runPressed = false;
        }

        #region Save

        public object CaptureState()
        {
            var position = gameObject.transform.position;
            return new SaveData
            {
                positionX = position.x,
                positionY = position.y,
                positionZ = position.z,
            };
        }

        public void RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            Vector3 position = new Vector3(saveData.positionX, saveData.positionY, saveData.positionZ);
            gameObject.transform.position = position;
        }

        [System.Serializable]
        private struct SaveData
        {
            public float positionX;
            public float positionY;
            public float positionZ;
        }

        #endregion

    }
}