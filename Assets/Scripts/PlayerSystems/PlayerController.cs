using System;
using LessonIsMath.Input;
using UnityEngine;
using XIV;
using XIV.SaveSystems;

namespace LessonIsMath.PlayerSystems
{
    public class PlayerController : MonoBehaviour, ISaveable
    {
        [SerializeField] float slowDownSpeed = 4f;
        [SerializeField] float speedUpSpeed = 2f;
        [SerializeField] float moveSpeed = 2.1f;
        [SerializeField] float runSpeed = 5;
        [SerializeField] float jumpForce = 3.2f;
        [SerializeField] float gravityScale = .7f;
        [SerializeField] float maxGravityForce = -3;
        PlayerAnimationController playerAnimationController;
        CharacterController characterController;
        PlayerInputProvider playerInputProvider;

        float turnVelocity;
        float storedVerticalAcceleration;
        float speed;
        float normalizedSpeed => speed > moveSpeed ? speed / runSpeed : (speed / moveSpeed) * 0.5f;
        const float SPEED_THRESHOLD = 0.2f;

        bool isMovingByInput;
        bool playJump;
        Vector3 previousPos;
        Vector3 velocity;

        void Awake()
        {
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
            characterController = GetComponent<CharacterController>();
            playerInputProvider = GetComponent<PlayerInputProvider>();
            previousPos = transform.position;
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
            Vector3 movement = playerInputProvider.GetMovementVector();
            Vector3 currentPos = transform.position;
            float veloictyMagnitude = velocity.magnitude;

            speed = isMovingByInput ? 
                Mathf.MoveTowards(speed, playerInputProvider.RunPressed ? runSpeed : moveSpeed, Time.deltaTime * speedUpSpeed) :
                Mathf.MoveTowards(speed, 0f, Time.deltaTime * slowDownSpeed);

            if (movement.sqrMagnitude > Mathf.Epsilon && playerAnimationController.IsJumpPlaying() == false)
            {
                isMovingByInput = true;
                velocity = (currentPos - previousPos) / Time.deltaTime;
                if (speed > SPEED_THRESHOLD) movement *= speed;
                else movement = Vector3.zero;
            }
            else if (veloictyMagnitude > 0 || speed > 0)
            {
                isMovingByInput = false;
                velocity = Vector3.MoveTowards(velocity, Vector3.zero, Time.deltaTime * slowDownSpeed);
                if (speed > SPEED_THRESHOLD) movement = playerInputProvider.GetMovementVector(transform.forward) * veloictyMagnitude;
                else movement = Vector3.zero;
            }

            transform.rotation = playerInputProvider.GetRotation();
            
            movement.y = storedVerticalAcceleration;
            movement.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            if (movement.y < maxGravityForce) movement.y = maxGravityForce;

            if (characterController.isGrounded && playerInputProvider.JumpPressed && playerAnimationController.IsJumpPlaying() == false)
            {
                movement.y = jumpForce;
                playJump = true;
            }

            characterController.Move(movement * Time.deltaTime);
            storedVerticalAcceleration = movement.y;
            playerInputProvider.JumpPressed = false;
            previousPos = currentPos;
#if UNITY_EDITOR
            XIVDebug.DrawLine(currentPos, currentPos + velocity);
#endif
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

        #region Save

        public object CaptureState()
        {
            var position = transform.position;
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
            transform.position = position;
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