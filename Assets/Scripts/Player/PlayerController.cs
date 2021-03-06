using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISaveable
{
    private CharacterController charController;
    private Transform cam;
    private float targetAngle;
    private float Angle;
    private float TurnSmoothVelocity;
    private float TurnSmoothTime = 0.1f;

    [SerializeField]
    private float moveSpeed = 2.1f, runSpeed = 5, jumpForce = 3.2f, gravityScale = .7f, MaxGravityForce = -3;
    private float storedVerticalAcceleration;
    private float Vertical, Horizontal;

    public bool IsGrounded { get => charController.isGrounded; }

    private Vector3 moveDir;

    public bool JumpPressed { get; private set; }
    public bool RunPressed { get; private set; }


    private void Awake()
    {
        cam = Camera.main.transform;

        charController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Move.performed += Move_performed;
        InputManager.PlayerControls.Gameplay.Move.canceled += Move_canceled;

        InputManager.PlayerControls.Gameplay.Jump.performed += Jump_performed;

        InputManager.PlayerControls.Gameplay.Run.performed += Run_performed;
        InputManager.PlayerControls.Gameplay.Run.canceled += Run_canceled;

        InputManager.GamePlay.Enable();
    }

    private void OnDisable()
    {
        InputManager.PlayerControls.Gameplay.Move.performed -= Move_performed;
        InputManager.PlayerControls.Gameplay.Move.canceled -= Move_canceled;

        InputManager.PlayerControls.Gameplay.Jump.performed -= Jump_performed;

        InputManager.PlayerControls.Gameplay.Run.performed -= Run_performed;

        InputManager.GamePlay.Disable();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        moveDir = new Vector3(Horizontal, 0f, Vertical);

        //Karakterin bakması gereken yeri belirle.
        targetAngle = Mathf.Atan2(Horizontal, Vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;

        //Karakteri yumuşak bir şekilde y ekseninde döndür
        Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime);

        if (moveDir.magnitude > 1f)
        {
            moveDir.Normalize();
        }

        if (Horizontal != 0 || Vertical != 0)
        {
            transform.rotation = Quaternion.Euler(0f, Angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            charController.Move(moveDir * Time.deltaTime);
        }

        //IsRunning
        moveDir = RunPressed ? moveDir * runSpeed : moveDir * moveSpeed;

        moveDir.y = storedVerticalAcceleration;
        moveDir.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        if (moveDir.y < MaxGravityForce)
            moveDir.y = MaxGravityForce;

        if (IsGrounded && JumpPressed)
            moveDir.y = jumpForce;

        charController.Move(moveDir * Time.deltaTime);
        storedVerticalAcceleration = moveDir.y;
        JumpPressed = false;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        Vector2 vector = obj.ReadValue<Vector2>();
        Horizontal = vector.x;
        Vertical = vector.y;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        Horizontal = 0;
        Vertical = 0;
    }

    private void Jump_performed(InputAction.CallbackContext obj) => JumpPressed = true;

    private void Run_performed(InputAction.CallbackContext obj) => RunPressed = true;

    private void Run_canceled(InputAction.CallbackContext obj) => RunPressed = false;

    #region Data Saving -----------

    public object CaptureState()
    {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        float z = gameObject.transform.position.z;
        return new SaveData
        {
            positionX = x,
            positionY = y,
            positionZ = z,
        };
    }

    public void RestoreState(object state)
    {
        SaveData saveData = (SaveData)state;
        Vector3 position;
        position.x = saveData.positionX;
        position.y = saveData.positionY;
        position.z = saveData.positionZ;
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
