using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LessonIsMath.UI.Components
{
    public class CustomButton : Button
    {
        UnityAction onClickAction;
        UnityAction onPointerUp;

        public CustomButton RegisterOnClick(Action action)
        {
            onClickAction = () => action.Invoke();
            return this;
        }

        public CustomButton UnregisterOnClick()
        {
            onClickAction = null;
            return this;
        }

        public CustomButton RegisterOnPointerUp(Action action)
        {
            onPointerUp = () => action.Invoke();
            return this;
        }

        public CustomButton UnregisterOnPointerUp()
        {
            onPointerUp = null;
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
