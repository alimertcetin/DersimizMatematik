using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LessonIsMath.UI
{
    public class CustomButton : Button
    {
        UnityAction onClickAction;
        UnityAction onPointerUp;

        public CustomButton RegisterOnClick(Action action)
        {
            this.onClickAction = () => action.Invoke();
            return this;
        }

        public CustomButton UnregisterOnClick()
        {
            this.onClickAction = null;
            return this;
        }

        public CustomButton RegisterOnPointerUp(Action action)
        {
            this.onPointerUp = () => action.Invoke();
            return this;
        }

        public CustomButton UnregisterOnPointerUp()
        {
            this.onPointerUp = null;
            return this;
        }

        // TODO : Learn what changes when overriding below methods
        public override void OnPointerDown(PointerEventData eventData)
        {
            onClickAction?.Invoke();
            // if we call base.OnPointerDown(eventData) state is not changing correctly, dont know why
            DoStateTransition(SelectionState.Pressed, false);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke();
            // if we call base.OnPointerUp(eventData) state is not changing correctly, dont know why
            DoStateTransition(SelectionState.Normal, false);
        }
    }
}
