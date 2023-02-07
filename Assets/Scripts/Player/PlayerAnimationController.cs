using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] AudioClip[] stepSound = null;
    AudioSource audioSource;

    public bool IsRunning;
    Animator animator;

    float audioSourcePitch;
    float speed;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audioSourcePitch = audioSource.pitch;
    }

    public void PlayJump()
    {
        animator.Play("Jump");
    }

    public void PlayWalk(float speed)
    {
        animator.SetFloat("Speed", speed, .2f, Time.deltaTime);
    }

    //Walk and run animation is using this method
    private void PlayAudio()
    {
        if (speed < 0.1f) return;

        audioSource.pitch = IsRunning ? 1.5f : audioSourcePitch;
        audioSource.PlayOneShot(GetClip());
    }

    private AudioClip GetClip()
    {
        return stepSound[Random.Range(0, stepSound.Length)];
    }

}
