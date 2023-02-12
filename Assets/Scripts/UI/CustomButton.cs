using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LessonIsMath.UI
{
    /// <summary>
    /// Allows onClick registration and unregistration of anonymous methods
    /// </summary>
    public class CustomButton : Button
    {
        UnityAction action;
        bool registered;

        public CustomButton Register(Action action)
        {
            Unregister();

            this.action = delegate { action(); };
            return Register();
        }

        CustomButton Register()
        {
            if (registered) return this;
            registered = true;
            this.onClick.AddListener(this.action);
            return this;
        }

        public CustomButton Unregister()
        {
            if (registered == false) return this;
            registered = false;
            this.onClick.RemoveListener(this.action);
            return this;
        }
    }
}
