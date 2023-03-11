using LessonIsMath.DoorSystems;
using LessonIsMath.PlayerSystems;
using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public class KeycardDoorInteraction
    {
        public bool hasTarget { get; private set; }
        
        DoorManager doorManager;
        Transform transform;
        PlayerAnimationController playerAnimationController;
        
        public void Init(Transform transform)
        {
            this.transform = transform;
            playerAnimationController = transform.GetComponentInChildren<PlayerAnimationController>();
        }

        public void Update()
        {
            // var cardReader = doorManager.GetCardReader();
        }

        public void SetTarget(DoorManager doorManager)
        {
            hasTarget = true;
            this.doorManager = doorManager;
        }

        public void ClearTarget()
        {
            hasTarget = false;
            doorManager = null;
        }
    }
}