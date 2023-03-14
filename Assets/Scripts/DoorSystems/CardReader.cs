using System;
using UnityEngine;
using XIV.EventSystem;
using XIV.EventSystem.Events;

namespace LessonIsMath.DoorSystems
{
    public class CardReader : MonoBehaviour
    {
        [SerializeField] Transform screenTransform;
        [SerializeField] Transform cardInsertPosition;
        [SerializeField] Material lockedMaterial;
        [SerializeField] Material unlockedMaterial;
        Renderer screenRenderer;
        Material[] rendererMaterials;
        float currentProgress;
        float targetProgress;
        bool hasEvent;

        void Awake()
        {
            screenRenderer = screenTransform.GetComponent<Renderer>();
            var materials = screenRenderer.materials;
            int length = materials.Length;
            rendererMaterials = new Material[length];
            Array.Copy(materials, rendererMaterials, length);
        }

        public void UpdateVisual(float progressNormalized)
        {
            targetProgress = progressNormalized;
            if (Mathf.Abs(1 - targetProgress) < Mathf.Epsilon) currentProgress = 1f;
            if (hasEvent) return;
            hasEvent = true;
            XIVEventSystem.SendEvent(new InvokeUntilEvent().AddAction(() =>
            {
                currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, Time.deltaTime * 0.5f);
                rendererMaterials[0].Lerp(lockedMaterial, unlockedMaterial, currentProgress);
                screenRenderer.materials = rendererMaterials;
            }).AddCancelCondition(() =>
            {
                bool isDone = Mathf.Abs(currentProgress - targetProgress) < Mathf.Epsilon;
                if (isDone)
                {
                    currentProgress = targetProgress;
                    hasEvent = false;
                }

                return isDone;
            }));
        }

        public Vector3 GetCardInsertPosition() => cardInsertPosition.position;
    }
}