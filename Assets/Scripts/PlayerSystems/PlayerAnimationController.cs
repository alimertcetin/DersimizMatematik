﻿using System;
using LessonIsMath.DoorSystems;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using XIV.Easing;
using XIV.EventSystem;
using XIV.Utils;
using CameraType = LessonIsMath.CameraSystems.CameraType;
using Random = UnityEngine.Random;

namespace LessonIsMath.PlayerSystems
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] CameraTransitionEventChannelSO cameraTransitionChannel;
        [SerializeField] AudioClip[] stepSound = null;
        AudioSource audioSource;

        float pitch;
        float defaultPitch;
        bool isJumping;
        Animator animator;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            defaultPitch = audioSource.pitch;
        }

        public void PlayLocomotion(float speed)
        {
            pitch = speed;
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
            var timedEvent = new XIVTimedEvent(duration).OnCompleted(() =>
            {
                animator.SetBool(AnimationConstants.AJ.AJ_Jump_Bool, false);
                isJumping = false;
            });
            XIVEventSystem.SendEvent(timedEvent);
        }

        public bool IsJumpPlaying() => isJumping;

        public void BendRightHandFingers(float t)
        {
            if (t > 0.5f)
            {
                animator.SetBool(AnimationConstants.AJ.AJ_RightHandRelease_Bool, false);
                animator.SetBool(AnimationConstants.AJ.AJ_RightHandHold_Bool, true);
            }
            else
            {
                animator.SetBool(AnimationConstants.AJ.AJ_RightHandRelease_Bool, true);
                animator.SetBool(AnimationConstants.AJ.AJ_RightHandHold_Bool, false);
            }
            animator.SetLayerWeight(AnimationConstants.AJ.AJ_Right_Hand_Override_Layer, t);
        }

        public void BendLeftHandFingers(float t)
        {
        }

        public void HandleInteractionAnimation(IInteractable interactable, Action<IInteractable> onAnimationEnd = null)
        {
            if (interactable is DoorManager doorManager)
            {
                if (doorManager.GetState().HasFlag(DoorState.Unlocked) == false)
                {
                    onAnimationEnd?.Invoke(interactable);
                    return;
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("Animation is not implemented : " + interactable);
#endif
                cameraTransitionChannel.RaiseEvent(CameraType.Character);
                onAnimationEnd?.Invoke(interactable);
            }
        }

        //Walk and run animation is using this method
        void PlayAudio()
        {
            audioSource.pitch = pitch > 0 ? pitch : defaultPitch;
            audioSource.PlayOneShot(GetClip());
        }

        AudioClip GetClip()
        {
            return stepSound[Random.Range(0, stepSound.Length)];
        }

    }
}