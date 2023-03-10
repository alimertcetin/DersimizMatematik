using System;
using LessonIsMath.InteractionSystems;
using UnityEngine;
using XIV;
using XIV.XIVMath;

namespace LessonIsMath.PlayerSystems
{
    struct BezierCurve
    {
        public Vector3 start;
        public Vector3 mid1;
        public Vector3 mid2;
        public Vector3 end;

        public float GetT(Vector3 currentPos)
        {
            return BezierMath.GetTime(currentPos, start, mid1, mid2, end);
        }

        public Vector3 GetPoint(float t)
        {
            return BezierMath.GetPoint(start, mid1, mid2, end, t);
        }

#if UNITY_EDITOR
        public void Draw()
        {
            XIVDebug.DrawBezier(start, mid1, mid2, end, 0.5f);
        }
        public void Draw(float t)
        {
            XIVDebug.DrawBezier(start, mid1, mid2, end, t, 0.5f);
        }
#endif
    }
    
    public class AutoMovementInput : MonoBehaviour
    {
        public bool hasTarget => needsMovement || needsRotation;
        bool needsMovement;
        bool needsRotation;
        InteractionData interactionData;
        BezierCurve bezierCurve;
        
        public Vector3 GetMovementVector(Vector3 currentPosition)
        {
            // TODO : Implement path finding
            float distance = Vector3.Distance(currentPosition, interactionData.targetData.targetPosition);
            if (distance < 0.25f)
            {
                if (needsRotation) return Vector3.zero;
                interactionData.OnTargetReached?.Invoke();
                Clear();
                return Vector3.zero;
            }
            
            float t = bezierCurve.GetT(currentPosition);
            t = Mathf.Clamp01(t + 0.1f);
            Vector3 targetPos = bezierCurve.GetPoint(t);
#if UNITY_EDITOR
            bezierCurve.Draw(t);
#endif
            Vector3 movementVector = targetPos - currentPosition;
            if (movementVector.magnitude < 1f) movementVector = movementVector.normalized;
            return movementVector;
        }

        public Vector3 GetRotationDirection(Vector3 currentPosition, Vector3 forward)
        {
            float dot = Vector3.Dot(forward, -interactionData.targetData.targetForwardDirection);
            if (dot > 0.9f)
            {
                needsRotation = false;
                return forward;
            }

            float t = bezierCurve.GetT(currentPosition);
            t = Mathf.Clamp01(t + 0.25f);
            Vector3 targetPos = bezierCurve.GetPoint(t);
#if UNITY_EDITOR
            Debug.Log("Interactable is not in front of player, Dot : " + dot);
            XIVDebug.DrawSphere(interactionData.targetData.targetPosition, 0.25f, 0.25f);
#endif
            return (targetPos - currentPosition).normalized;
        }
        
        public void SetTarget(InteractionData interactionData)
        {
            if (hasTarget) CancelTarget();
            this.interactionData = interactionData;
            needsMovement = true;
            needsRotation = true;
            
            Vector3 endPos = interactionData.targetData.targetPosition;
            Vector3 startPos = interactionData.targetData.startPos;
            Vector3 mid1 = endPos + interactionData.targetData.targetForwardDirection * 0.4f;
            Vector3 mid2 = endPos + interactionData.targetData.targetForwardDirection * 0.2f;
            bezierCurve = new BezierCurve
            {
                start = startPos,
                mid1 = mid1,
                mid2 = mid2,
                end = endPos,
            };
        }

        public void CancelTarget()
        {
            interactionData.OnMovementCanceled?.Invoke();
            Clear();
        }

        void Clear()
        {
            interactionData = default;
            needsMovement = false;
            needsRotation = false;
        }
    }
}