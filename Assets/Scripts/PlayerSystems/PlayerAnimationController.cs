using System;
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
        [SerializeField] TwoBoneIKConstraint rightHandIKConstraint;
        [SerializeField] MultiAimConstraint splineIKConstraint;
        [SerializeField] AudioClip[] stepSound = null;
        AudioSource audioSource;

        float pitch;
        float defaultPitch;
        bool isJumping;
        Animator animator;
        const float DOOR_REACH_DURATION = 1f;

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
            var timedEvent = new XIVTimedEvent(duration);
            timedEvent.OnCompleted = () =>
            {
                animator.SetBool(AnimationConstants.AJ.AJ_Jump_Bool, false);
                isJumping = false;
            };
            XIVEventSystem.SendEvent(timedEvent);
        }

        public bool IsJumpPlaying() => isJumping;

        public void HandleInteractionAnimation(IInteractable interactable, Action<IInteractable> onAnimationEnd = null)
        {
            void SetHandIKPositionAndRotation(Vector3 handlePos, Quaternion handleRot)
            {
                var direction = handlePos - rightHandIKConstraint.data.tip.position;
                var ikTargetPos = handlePos - direction.normalized * 0.2f;
                ikTargetPos += new Vector3(0, 0.001f, 0);
                rightHandIKConstraint.data.target.position = ikTargetPos;
                var lookRotation = Quaternion.LookRotation(direction.normalized);
                // Hand forward = transform.left, -90 around y axis matches target forward and hand forward
                rightHandIKConstraint.data.target.rotation = lookRotation * Quaternion.Euler(0, -90, 0) * handleRot;
            }
            
            void SetIKWeights(float normalizedTime)
            {
                rightHandIKConstraint.weight = EasingFunction.SmoothStop3(normalizedTime);
                splineIKConstraint.weight = EasingFunction.SmoothStop3(normalizedTime, 0, 0.5f);
            }
            
            if (interactable is Door door)
            {
                if (door.GetState().HasFlag(DoorState.Unlocked) == false)
                {
                    onAnimationEnd?.Invoke(interactable);
                    return;
                }
                
                cameraTransitionChannel.RaiseEvent(CameraType.SideViewLeft);
                animator.SetBool(AnimationConstants.AJ.AJ_RightHandHold_Bool, true);
                var increaseWeightEvent = new XIVInvokeUntilEvent(DOOR_REACH_DURATION, (Timer timer) =>
                {
                    var normalizedTime = timer.NormalizedTime;
                    animator.SetLayerWeight(AnimationConstants.AJ.AJ_Right_Hand_Override_Layer, timer.NormalizedTime);
                    door.RotateDoorHandle(normalizedTime, out var handleRot);
                    SetIKWeights(normalizedTime);
                    SetHandIKPositionAndRotation(door.GetHandlePosition(), handleRot);
                }).OnCompleted(() =>
                {
                    animator.SetBool(AnimationConstants.AJ.AJ_RightHandRelease_Bool, true);
                    animator.SetBool(AnimationConstants.AJ.AJ_RightHandHold_Bool, false);
                    var decreaseWeightEvent = new XIVInvokeUntilEvent(DOOR_REACH_DURATION, (Timer timer) =>
                    {
                        var normalizedTime = 1 - timer.NormalizedTime;
                        animator.SetLayerWeight(AnimationConstants.AJ.AJ_Right_Hand_Override_Layer, normalizedTime);
                        door.RotateDoorHandle(normalizedTime, out var handleRot);
                        SetIKWeights(normalizedTime);
                        SetHandIKPositionAndRotation(door.GetHandlePosition(), handleRot);
                    }).OnCompleted(() => animator.SetBool(AnimationConstants.AJ.AJ_RightHandRelease_Bool, false));
                    
                    cameraTransitionChannel.RaiseEvent(CameraType.Character);
                    onAnimationEnd?.Invoke(interactable);
                    XIVEventSystem.SendEvent(decreaseWeightEvent);
                });
                XIVEventSystem.SendEvent(increaseWeightEvent);
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