using UnityEngine;

namespace LessonIsMath.PlayerSystems
{
    public class PlayerFootStepSound : MonoBehaviour
    {
        [SerializeField] FootStepSound leftFoot;
        [SerializeField] FootStepSound rightFoot;

        public void OnLocomotion()
        {
            leftFoot.Update();
            rightFoot.Update();
        }
    }
}