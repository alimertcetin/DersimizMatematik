using UnityEngine;

namespace LessonIsMath.PlayerSystems
{
    public struct VelocityCalculator
    {
        public float magnitude { get; private set; }
        public Vector3 currentVelocity => velocity;
        
        Vector3 previousPosition;
        Vector3 velocity;

        public VelocityCalculator(Vector3 currentPosition)
        {
            previousPosition = currentPosition;
            velocity = Vector3.zero;
            magnitude = 0f;
        }
        
        public void Update(Vector3 currentPosition)
        {
            velocity = (currentPosition - previousPosition) / Time.deltaTime;
            previousPosition = currentPosition;
            magnitude = velocity.magnitude;
        }
    }
}