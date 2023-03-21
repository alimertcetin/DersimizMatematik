using System;
using LessonIsMath.InteractionSystems;
using UnityEngine;
using XIV.EventSystem;
using XIV.EventSystem.Events;

namespace LessonIsMath.PlayerSystems
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] PlayerFootStepSound footStepSound;
        bool isJumping;
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayLocomotion(float speed)
        {
            footStepSound.OnLocomotion();
            animator.SetFloat(AnimationConstants.AJ.Parameters.AJ_Speed_Float, speed);
        }

        public void PlayJump()
        {
            var duration = 0f;
            for (var i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationClip animationClip = animator.runtimeAnimatorController.animationClips[i];
                if (animationClip.name != AnimationConstants.AJ.Clips.AJ_Jump) continue;
                duration = animationClip.length;
            }

            isJumping = true;
            animator.SetBool(AnimationConstants.AJ.Parameters.AJ_Jump_Bool, true);
            XIVEventSystem.SendEvent(new InvokeAfterEvent(duration).OnCompleted(() =>
            {
                animator.SetBool(AnimationConstants.AJ.Parameters.AJ_Jump_Bool, false);
            }));
            XIVEventSystem.SendEvent(new InvokeAfterEvent(duration + 0.05f).OnCompleted(() =>
            {
                isJumping = false;
            }));
        }

        public bool IsJumpPlaying() => isJumping;

        public void BendRightHandFingers(float t)
        {
            if (t > 0.5f)
            {
                animator.SetBool(AnimationConstants.AJ.Parameters.AJ_RightHandRelease_Bool, false);
                animator.SetBool(AnimationConstants.AJ.Parameters.AJ_RightHandHold_Bool, true);
            }
            else
            {
                animator.SetBool(AnimationConstants.AJ.Parameters.AJ_RightHandRelease_Bool, true);
                animator.SetBool(AnimationConstants.AJ.Parameters.AJ_RightHandHold_Bool, false);
            }
            animator.SetLayerWeight(AnimationConstants.AJ.Layers.AJ_Right_Hand_Override_Layer, t);
        }

        public void BendLeftHandFingers(float t)
        {
        }

        public void HandleInteractionAnimation(IInteractable interactable, Action<IInteractable> onAnimationEnd = null)
        {
            Debug.LogWarning("Animation is not implemented : " + interactable);
            onAnimationEnd?.Invoke(interactable);
        }

    }
}