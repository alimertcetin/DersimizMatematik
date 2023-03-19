using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LessonIsMath.InteractionSystems
{
    public abstract class InteractionHandlerBase : MonoBehaviour
    {
        protected List<Collider> others = new List<Collider>(8);
        protected IInteractor interactor;

        public virtual void Init(IInteractor interactor)
        {
            this.interactor = interactor;
        }

        public virtual void TriggerEnter(Collider other)
        {
            others.Add(other);
        }

        public virtual void TriggerExit(Collider other)
        {
            others.Remove(other);
        }
        
        public abstract IEnumerator OnInteractionStart(IInteractable interactable);
        public abstract IEnumerator OnInteractionEnd(IInteractable interactable);
    }
}