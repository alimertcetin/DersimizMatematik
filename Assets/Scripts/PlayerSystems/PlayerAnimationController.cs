﻿using System;
using LessonIsMath.DoorSystems;
using LessonIsMath.InteractionSystems;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using XIV.Easing;
using XIV.EventSystem;
using XIV.EventSystem.Events;
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
            for (var i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationClip animationClip = animator.runtimeAnimatorController.animationClips[i];
                if (animationClip.name != AnimationConstants.AJ.AJ_Jump) continue;
                duration = animationClip.length;
            }

            isJumping = true;
            animator.SetBool(AnimationConstants.AJ.AJ_Jump_Bool, true);
            XIVEventSystem.SendEvent(new InvokeAfterEvent(duration).OnCompleted(() =>
            {
                animator.SetBool(AnimationConstants.AJ.AJ_Jump_Bool, false);
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
                if (doorManager.GetState().HasFlag(DoorState.RequiresKeycard))
                {
                    // TODO : Move to PlayerInteraction
                    cameraTransitionChannel.RaiseEvent(CameraType.SideViewLeft);
                    var invokeUntilEvent = XIVEventSystem.GetEvent<InvokeUntilEvent>() as IEvent;
                    XIVEventSystem.CancelEvent(invokeUntilEvent);
                    Timer transitionDuration = new Timer(2.5f);
                    XIVEventSystem.SendEvent(new InvokeUntilEvent()
                        .AddAction(() => transitionDuration.Update(Time.deltaTime))
                        .OnCompleted(() => cameraTransitionChannel.RaiseEvent(CameraType.Character))
                        .AddCancelCondition(() => interactable.IsInInteraction == false && transitionDuration.IsDone));
                    onAnimationEnd?.Invoke(interactable);
                    return;
                }
                onAnimationEnd?.Invoke(interactable);
                return;
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