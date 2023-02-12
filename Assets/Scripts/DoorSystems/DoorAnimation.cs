using UnityEngine;

namespace LessonIsMath
{
    [DisallowMultipleComponent]
    public class DoorAnimation : MonoBehaviour
    {
        [Header("Eğer bu ikili bir kapıysa bu alana LEFT HOLDER'ı atayın..")]
        [SerializeField] bool IsThisLeftSide = true;
        [SerializeField] DoorAnimation otherDoorAnimation = null;
        [SerializeField] Animator animator;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip[] DoorOpeningClips = null;
        [SerializeField] AudioClip[] DoorClosingClips = null;

        float movementSpeed;

        public void Interact(bool isOpen)
        {
            if (IsThisLeftSide)
            {
                LeftSideMovement(!isOpen);
                if (otherDoorAnimation != null)
                {
                    otherDoorAnimation.RightSideMovement(!isOpen);
                }
            }
            else
            {
                RightSideMovement(!isOpen);
                if (otherDoorAnimation != null)
                {
                    otherDoorAnimation.LeftSideMovement(!isOpen);
                }
            }
        }

        private void RightSideMovement(bool doorIsOpen)
        {
            if (doorIsOpen == false)
            {
                StopAnimation(animator, "RightSide_Close");
                PlayAnimation(animator, "RightSide_Open");
            }
            else
            {
                StopAnimation(animator, "RightSide_Open");
                PlayAnimation(animator, "RightSide_Close");
            }
        }

        private void LeftSideMovement(bool doorIsOpen)
        {
            if (doorIsOpen == false)
            {
                StopAnimation(animator, "LeftSide_Close");
                PlayAnimation(animator, "LeftSide_Open");
            }
            else
            {
                StopAnimation(animator, "LeftSide_Open");
                PlayAnimation(animator, "LeftSide_Close");
            }
        }

        void PlayAnimation(Animator animController, string animationName)
        {
            movementSpeed = Random.Range(0.5f, 1.5f);
            animController.SetFloat("rndAnimSpeed", movementSpeed);
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