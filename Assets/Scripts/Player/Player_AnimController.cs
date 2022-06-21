using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]

public class Player_AnimController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] stepSound = null;

    private Animator anim;
    private PlayerController playerController;
    private AudioSource audioSource;

    private float audioSourcePitch;
    private float speed;

    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSourcePitch = audioSource.pitch;
        anim = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayTheAnim();
    }

    private void PlayTheAnim()
    {
        if (InputManager.PlayerControls.Gameplay.Move.phase == InputActionPhase.Started)
        {
            speed = playerController.RunPressed ? 1 : .5f;
        }
        else
        {
            speed = 0;
        }

        anim.SetFloat("Speed", speed, .2f, Time.deltaTime);

        if (playerController.JumpPressed && playerController.IsGrounded)
        {
            anim.SetTrigger("Jump");
        }
    }

    //Walk and run animation is using this method
    private void PlayAudio()
    {
        if (speed < .1f) return;

        if (playerController.RunPressed)
        {
            audioSource.pitch = 1.5f;
        }
        else
        {
            audioSource.pitch = audioSourcePitch;
        }
        audioSource.PlayOneShot(GetClip());
    }

    private AudioClip GetClip()
    {
        return stepSound[Random.Range(0, stepSound.Length)];
    }

}
