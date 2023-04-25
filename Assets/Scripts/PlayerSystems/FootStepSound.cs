using UnityEngine;
using XIV;
using XIV.Core;
using XIV.Core.XIVMath;

namespace LessonIsMath.PlayerSystems
{
    [System.Serializable]
    public class FootStepSound
    {
        [SerializeField] GameObject foot;
        [SerializeField] AudioClip[] stepSounds;
        [SerializeField] AudioSource audioSource;
        [SerializeField] float distance;
        [SerializeField, Range(-3f, 3f)] float minPitch;
        [SerializeField, Range(-3f, 3f)] float maxPitch;
        
        static readonly RaycastHit[] raycastHitBuffer = new RaycastHit[4];
        VelocityCalculator velocityCalculator;
        bool isHit;
        
        public void Update()
        {
            var footTransform = foot.transform;
            var currentPos = footTransform.position;
            velocityCalculator.Update(currentPos);
            
            var ray = new Ray(currentPos, -footTransform.up);
#if UNITY_EDITOR
            XIVDebug.DrawLine(ray.origin, ray.origin + ray.direction * distance, 0.25f);
#endif
            int hitCount = Physics.RaycastNonAlloc(ray, raycastHitBuffer, distance, 1 << PhysicsConstants.GroundLayer);
            if (hitCount == 0)
            {
                isHit = false;
                return;
            }

            if (isHit == false)
            {
                var pitch = XIVMathf.RemapClamped(velocityCalculator.magnitude, 0, 10f, minPitch, maxPitch);
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
                isHit = true;
            }
        }
    }
}