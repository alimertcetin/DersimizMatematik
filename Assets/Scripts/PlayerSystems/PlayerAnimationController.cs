using UnityEngine;
using XIV.EventSystem;
using Random = UnityEngine.Random;

namespace LessonIsMath.PlayerSystems
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] AudioClip[] stepSound = null;
        AudioSource audioSource;

        bool isRunning;
        bool isJumping;
        Animator animator;

        float audioSourcePitch;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            audioSourcePitch = audioSource.pitch;
        }

        public void PlayLocomotion(float speed)
        {
            animator.SetFloat(AnimationConstants.AJ.AJ_Speed_Float, speed);
        }

        public void PlayJump()
        {
            var duration = 0f;
            foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
            {
                if (animationClip.name != AnimationConstants.AJ.AJ_Jump) continue;
                duration = animationClip.length;
            }
            
            animator.SetBool(AnimationConstants.AJ.AJ_Jump_Bool, true);
            isJumping = true;
            var timedEvent = new XIVTimedEvent(duration);
            timedEvent.OnCompleted = () =>
            {
                animator.SetBool(AnimationConstants.AJ.AJ_Jump_Bool, false);
                isJumping = false;
            };
            XIVEventSystem.SendEvent(timedEvent);
        }

        public bool IsJumpPlaying() => isJumping;

        //Walk and run animation is using this method
        private void PlayAudio()
        {
            audioSource.pitch = isRunning ? 1.5f : audioSourcePitch;
            audioSource.PlayOneShot(GetClip());
        }

        private AudioClip GetClip()
        {
            return stepSound[Random.Range(0, stepSound.Length)];
        }

    }
}