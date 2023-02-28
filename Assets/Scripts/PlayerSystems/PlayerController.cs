using System;
using LessonIsMath.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using XIV;
using XIV.SaveSystems;
using XIV.XIVMath;

namespace LessonIsMath.PlayerSystems
{
    public struct TargetData
    {
        public Vector3 startPos;
        public Vector3 targetPosition;
        public Vector3 forward;
        public Action OnTargetReached;
        public Action OnMovementCanceled;
    }
    
    public class PlayerController : MonoBehaviour, PlayerControls.ICharacterMovementActions, ISaveable
    {
        CharacterController charController;
        
        [SerializeField] float inputSensitivity = 2f;
        [SerializeField] float moveSpeed = 2.1f;
        [SerializeField] float runSpeed = 5;
        [SerializeField] float jumpForce = 3.2f;
        [SerializeField] float gravityScale = .7f;
        [SerializeField] float maxGravityForce = -3;
        PlayerAnimationController playerAnimationController;

        float turnVelocity;
        float storedVerticalAcceleration;
        float speed;
        float normalizedSpeed => speed > moveSpeed ? speed / runSpeed : (speed / moveSpeed) * 0.5f;
        const float SPEED_THRESHOLD = 0.2f;
        
        bool movingTowardsTarget;
        TargetData targetData;
        Vector2 input;

        bool isGrounded => charController.isGrounded;
        bool jumpPressed { get; set; }
        bool runPressed { get; set; }

        void Awake()
        {
            charController = GetComponent<CharacterController>();
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
            InputManager.CharacterMovement.SetCallbacks(this);
        }

        void OnEnable() => InputManager.CharacterMovement.Enable();
        void OnDisable() => InputManager.CharacterMovement.Disable();

        void FixedUpdate()
        {
            HandleMovement();
        }
        
        void Update()
        {
            HandleAnimation();
        }
        
        void HandleMovement()
        {
            Vector3 movement = Vector3.zero;
            Vector3 transformPosition = transform.position;
            
            if (movingTowardsTarget)
            {
                Vector3 startPos = targetData.startPos;
                Vector3 endPos = targetData.targetPosition;
                Vector3 middlePos = endPos + targetData.forward;
#if UNITY_EDITOR
                XIVDebug.DrawBezier(startPos, middlePos, middlePos, endPos, Color.green, 2f, 20);
#endif
                var t = BezierMath.GetTime(transformPosition, startPos, middlePos, middlePos, endPos);
                t += 0.01f;
                if (t < 1)
                {
                    var targetPos = BezierMath.GetPoint(startPos, middlePos, middlePos, endPos, t);
                    input = GetRequiredInput((targetPos - transformPosition).normalized);
                }
                else
                {
                    movingTowardsTarget = false;
                    targetData.OnTargetReached?.Invoke();
                    input = Vector2.zero;
                }
            }

            void SetMovement()
            {
                var targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                var newAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.15f);
                transform.rotation = Quaternion.Euler(0f, newAngle, 0f);
                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
            }
            Debug.Log("IsJumping : "  + playerAnimationController.IsJumpPlaying() + " Input : " + input);
            if (input.magnitude > Mathf.Epsilon && playerAnimationController.IsJumpPlaying() == false)
            {
                speed = Mathf.MoveTowards(speed, runPressed ? runSpeed : moveSpeed, Time.deltaTime * inputSensitivity);
                if (speed > SPEED_THRESHOLD) SetMovement();
            }
            else
            {
                if (speed > 0)
                {
                    speed = Mathf.MoveTowards(speed, 0, Time.deltaTime * (inputSensitivity * 2f));
                    var temp = input;
                    input = GetRequiredInput(transform.forward);
                    SetMovement();
                    input = temp;
                }
            }
            
            movement.y = storedVerticalAcceleration;
            movement.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            if (movement.y < maxGravityForce) movement.y = maxGravityForce;

            if (isGrounded && jumpPressed && playerAnimationController.IsJumpPlaying()) movement.y = jumpForce;

            charController.Move(movement * Time.deltaTime);
            storedVerticalAcceleration = movement.y;
            jumpPressed = false;
        }

        void HandleAnimation()
        {
            if (jumpPressed)
            {
                if (playerAnimationController.IsJumpPlaying() == false) playerAnimationController.PlayJump();
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

        public void SetTarget(TargetData targetData)
        {
            CancelTarget();
            this.targetData = targetData;
            this.targetData.startPos = transform.position;
            movingTowardsTarget = true;
        }

        public void CancelTarget()
        {
            if (movingTowardsTarget == false) return;
            
            targetData.OnMovementCanceled?.Invoke();
            movingTowardsTarget = false;
            input = Vector2.zero;
        }

        void PlayerControls.ICharacterMovementActions.OnMove(InputAction.CallbackContext context)
        {
            input = context.ReadValue<Vector2>();
            if (movingTowardsTarget) targetData.OnMovementCanceled?.Invoke();
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