using UnityEngine;

namespace LessonIsMath
{
    [DisallowMultipleComponent]
    public class DoorAnimation : MonoBehaviour
    {
        [Tooltip("Leave empty if there is no other door")]
        [SerializeField] DoorAnimation otherDoorAnimation = null;
        [SerializeField] bool isThisRightSide = true;
        [SerializeField] Animator animator;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip[] DoorOpeningClips = null;
        [SerializeField] AudioClip[] DoorClosingClips = null;

        float movementSpeed;

        public void OpenDoor()
        {
            if (otherDoorAnimation != null) otherDoorAnimation.OpenDoor();
            if (isThisRightSide)
            {
                OpenRightSide();
                return;
            }
            OpenLeftSide();
        }

        public void CloseDoor()
        {
            if (otherDoorAnimation != null) otherDoorAnimation.CloseDoor();
            if (isThisRightSide)
            {
                CloseRightSide();
                return;
            }
            CloseLeftSide();
        }

        void OpenLeftSide()
        {
            StopAnimation(animator, AnimationConstants.Door.Door_LeftSide_Close_Bool);
            PlayAnimation(animator, AnimationConstants.Door.Door_LeftSide_Open_Bool);
        }

        void CloseLeftSide()
        {
            StopAnimation(animator, AnimationConstants.Door.Door_LeftSide_Open_Bool);
            PlayAnimation(animator, AnimationConstants.Door.Door_LeftSide_Close_Bool);
        }

        void OpenRightSide()
        {
            StopAnimation(animator, AnimationConstants.Door.Door_RightSide_Close_Bool);
            PlayAnimation(animator, AnimationConstants.Door.Door_RightSide_Open_Bool);
        }

        void CloseRightSide()
        {
            StopAnimation(animator, AnimationConstants.Door.Door_RightSide_Open_Bool);
            PlayAnimation(animator, AnimationConstants.Door.Door_RightSide_Close_Bool);
        }

        void PlayAnimation(Animator animController, string animationName)
        {
            movementSpeed = Random.Range(0.5f, 1.5f);
            animController.SetFloat(AnimationConstants.Door.Door_AnimationSpeed_Float, movementSpeed);
            animController.SetBool(animationName, true);

        }

        void StopAnimation(Animator animController, string animationName)
        {
            animController.SetBool(animationName, false);
        }

        public void PlayOpeningClip()
        {
            AudioClip clip = DoorOpeningClips[Random.Range(0, DoorOpeningClips.Length)];

            var temp = audioSource.pitch;
            audioSource.pitch = movementSpeed;
            audioSource.PlayOneShot(clip);
            audioSource.pitch = temp;
        }

        public void PlayClosingClip()
        {
            AudioClip clip = DoorClosingClips[Random.Range(0, DoorClosingClips.Length)];

            var temp = audioSource.pitch;
            audioSource.pitch = movementSpeed;
            audioSource.PlayOneShot(clip);
            audioSource.pitch = temp;
        }

    }
}