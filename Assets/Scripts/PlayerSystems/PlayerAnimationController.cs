using UnityEngine;

namespace LessonIsMath.PlayerSystems
{
    [RequireComponent(typeof(AudioSource))]

    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] AudioClip[] stepSound = null;
        AudioSource audioSource;

        public bool IsRunning;
        Animator animator;

        float audioSourcePitch;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            audioSourcePitch = audioSource.pitch;
        }

        public void PlayJump()
        {
            animator.Play(AnimationConstants.AJ_Jumping);
        }

        public void PlayWalk(float speed)
        {
            animator.SetFloat(AnimationConstants.AJ_Speed_Float, speed, 0.1f, Time.deltaTime);
        }

        //Walk and run animation is using this method
        private void PlayAudio()
        {
            audioSource.pitch = IsRunning ? 1.5f : audioSourcePitch;
            audioSource.PlayOneShot(GetClip());
        }

        private AudioClip GetClip()
        {
            return stepSound[Random.Range(0, stepSound.Length)];
        }

    }
}