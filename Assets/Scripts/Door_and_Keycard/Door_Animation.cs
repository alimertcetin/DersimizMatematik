using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class Door_Animation : Animation_Manager_Class, ISaveable
{
    [Header("Eğer bu ikili bir kapıysa bu alana LEFT HOLDER'ı atayın..")]
    [Tooltip("First you need to add a Door_Animation Component to the other HOLDER.")]
    [SerializeField] private Door_Animation otherDoor = null;
    private Animator animController;
    private float rnd_AnimSpeed;
    public float Rnd_AnimSpeed { get => rnd_AnimSpeed; set => rnd_AnimSpeed = value; }
    private bool triggered;
    [SerializeField] private bool IsThisLeftSide = true;
    private bool doorIsOpen = false;
    public bool DoorIsOpen { get => doorIsOpen; set => doorIsOpen = value; }

    private bool doorLocked, keycardsAreRemoved;
    public bool DoorLocked { get => doorLocked; set => doorLocked = value; }
    public bool KeycardsAreRemoved { get => keycardsAreRemoved; set => keycardsAreRemoved = value; }
    private Door_Is_Locked door;
    private DoorKeycard_Management Keycard;

    private void Start()
    {
        animController = GetComponentInChildren<Animator>();
        if (animController == null)
        {
            Debug.LogError("Couldnt find Door animator. Door will not open or close!");
        }

        TryGetComponent(out door);
        TryGetComponent(out Keycard);
    }

    private void OnEnable()
    {
        InputManager.PlayerControls.Gameplay.Interact.performed += Interact_performed;

        if (Keycard != null)
            Keycard.AllKeycardsRemoved += onKeycardsRemoved;
    }

    private void OnDisable()
    {
        if (Keycard != null)
            Keycard.AllKeycardsRemoved -= onKeycardsRemoved;

        InputManager.PlayerControls.Gameplay.Interact.performed -= Interact_performed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggered)
        {
            if (door == null)
            {
                doorLocked = false;
            }
            else
            {
                doorLocked = door.DoorLocked;
            }

            if (Keycard == null)
            {
                keycardsAreRemoved = true;
            }
            else
            {
                keycardsAreRemoved = Keycard.GerekenKeycardlar.Count == 0 || Keycard.GerekenKeycardlar == null ? true : false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
        }
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (triggered && !doorLocked && keycardsAreRemoved)
        {
            if (IsThisLeftSide)
            {
                LeftSideMovement(doorIsOpen);
                if (otherDoor != null)
                {
                    otherDoor.RightSideMovement(doorIsOpen);
                }
            }
            else
            {
                RightSideMovement(doorIsOpen);
                if (otherDoor != null)
                {
                    otherDoor.LeftSideMovement(doorIsOpen);
                }
            }
            doorIsOpen = !doorIsOpen;
        }
    }

    private void onKeycardsRemoved()
    {
        keycardsAreRemoved = true;
    }

    private void RightSideMovement(bool doorIsOpen)
    {
        if (!doorIsOpen)
        {
            AnimationToStop(animController, "RightSide_Close");
            AnimationToPlay(animController, "RightSide_Open");
        }
        else
        {
            AnimationToStop(animController, "RightSide_Open");
            AnimationToPlay(animController, "RightSide_Close");
        }
    }

    private void LeftSideMovement(bool doorIsOpen)
    {
        if (!doorIsOpen)
        {
            AnimationToStop(animController, "LeftSide_Close");
            AnimationToPlay(animController, "LeftSide_Open");
        }
        else
        {
            AnimationToStop(animController, "LeftSide_Open");
            AnimationToPlay(animController, "LeftSide_Close");
        }
    }

    protected override void AnimationToPlay(Animator animController, string animationName)
    {
        rnd_AnimSpeed = UnityEngine.Random.Range(0.5f, 1.5f);
        animController.SetFloat("rndAnimSpeed", rnd_AnimSpeed);
        animController.SetBool(animationName, true);

    }

    protected override void AnimationToStop(Animator animController, string animationName)
    {
        animController.SetBool(animationName, false);
    }

    public object CaptureState()
    {
        return new SaveData
        {
            keycardsAreRemoved = KeycardsAreRemoved,
            doorLocked = DoorLocked,
            doorIsOpen = DoorIsOpen,
            isThisLeftSide = IsThisLeftSide
        };
    }

    public void RestoreState(object state)
    {
        SaveData saveData = (SaveData)state;
        KeycardsAreRemoved = saveData.keycardsAreRemoved;
        DoorLocked = saveData.doorLocked;
        DoorIsOpen = saveData.doorIsOpen;
        IsThisLeftSide = saveData.isThisLeftSide;

        if (!DoorLocked && KeycardsAreRemoved)
        {
            if (IsThisLeftSide)
            {
                LeftSideMovement(!doorIsOpen);
                if (otherDoor != null)
                {
                    otherDoor.RightSideMovement(!doorIsOpen);
                }
            }
            else
            {
                RightSideMovement(!doorIsOpen);
                if (otherDoor != null)
                {
                    otherDoor.LeftSideMovement(!doorIsOpen);
                }
            }
        }
    }

    [System.Serializable]
    private struct SaveData
    {
        public bool keycardsAreRemoved;
        public bool doorLocked;
        public bool doorIsOpen;
        public bool isThisLeftSide;
    }
}
