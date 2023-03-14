using System;
using UnityEngine;
using XIV.EventSystem;
using XIV.EventSystem.Events;

namespace LessonIsMath.Tween
{
    // TODO : Create a tween system
    /// <summary>
    /// Uses <see cref="XIVEventSystem"/> to apply tween
    /// </summary>
    public static class CustomTweenExtensions
    {
        public static void MoveTowardsTween(this Component component, Transform target, Vector3 offset, float moveSpeed, Action onCompleted)
        {
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() =>
            {
                Vector3 currentPosition = component.transform.position;
                Vector3 targetPosition = target.position + offset;
                Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
                component.transform.position = newPosition;
            }).AddCancelCondition(() =>
            {
                Vector3 currentPosition = component.transform.position;
                Vector3 targetPosition = target.position + offset;
                var distance = Vector3.Distance(currentPosition, targetPosition);
                bool isDone = distance < 0.01f;
                if (isDone)
                {
                    onCompleted?.Invoke();
                }
                return isDone;
            }));
        }
        
        public static void MoveTowardsTween(this Component component, Transform target, Func<Vector3> offsetFunc, float moveSpeed, Action onCompleted)
        {
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() =>
            {
                Vector3 currentPosition = component.transform.position;
                Vector3 targetPosition = target.position + offsetFunc.Invoke();
                Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
                component.transform.position = newPosition;
            }).AddCancelCondition(() =>
            {
                Vector3 currentPosition = component.transform.position;
                Vector3 targetPosition = target.position + offsetFunc.Invoke();
                var distance = Vector3.Distance(currentPosition, targetPosition);
                bool isDone = distance < 0.01f;
                if (isDone)
                {
                    onCompleted?.Invoke();
                }
                return isDone;
            }));
        }
        
        public static void MoveTowardsTween(this Component component, Vector3 targetPosition, float moveSpeed, Action onCompleted)
        {
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() =>
            {
                Vector3 currentPosition = component.transform.position;
                Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
                component.transform.position = newPosition;
            }).AddCancelCondition(() =>
            {
                Vector3 currentPosition = component.transform.position;
                var distance = Vector3.Distance(currentPosition, targetPosition);
                bool isDone = distance < 0.01f;
                if (isDone)
                {
                    onCompleted?.Invoke();
                }
                return isDone;
            }));
        }

        public static void MoveTowards(this Component component, Vector3 targetPos, float duration)
        {
            var startPos = component.transform.position;
            XIVEventSystem.SendEvent(new InvokeForSecondsEvent(duration).AddAction((timer) =>
            {
                var newPos = Vector3.Lerp(startPos, targetPos, timer.NormalizedTime);
                component.transform.position = newPos;
            }));
        }
        
        public static void RotateTowardsTween(this Component component, Quaternion targetRotation, float rotationSpeed)
        {
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() =>
            {
                component.transform.rotation = Quaternion.RotateTowards(component.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }).AddCancelCondition(() => Quaternion.Angle(component.transform.rotation, targetRotation) < 0.01f));
        }
        
        public static void LookTween(this Component component, Transform target, float rotationSpeed, Func<bool> stopCondition)
        {
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() =>
            {
                var transform = component.transform;
                var targetLook = Quaternion.LookRotation(-target.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetLook, rotationSpeed * Time.deltaTime);
            }).AddCancelCondition(stopCondition.Invoke));
        }
        
        public static void LookTween(this Component component, Transform target, float rotationSpeed)
        {
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() =>
            {
                var transform = component.transform;
                var targetRotation = Quaternion.LookRotation(-target.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }).AddCancelCondition(() =>
            {
                var targetRotation = Quaternion.LookRotation(-target.forward);
                return Quaternion.Angle(component.transform.rotation, targetRotation) < 0.01f;
            }));
        }

        public static void ScaleTween(this Component component, Vector3 targetScale, float duration)
        {
            var startScale = component.transform.localScale;
            XIVEventSystem.SendEvent(new InvokeForSecondsEvent(duration).AddAction((timer) =>
            {
                if (component == null) return;
                var newScale = Vector3.Lerp(startScale, targetScale, timer.NormalizedTime);
                component.transform.localScale = newScale;
            }).AddCancelCondition(() => component == null));
        }
    }
}