using UnityEngine;
using XIV.Core.Utils;
using XIV.Core.XIVMath;

namespace XIV.InventorySystem.Utils
{
    public struct ScrollSelector
    {
        public Transform contentParent;
        public int CurrentSelection { get; private set; }
        public Vector3 CurrentPos { get; private set; }

        int newSelection;
        Vector2 startPos;
        Vector2 endPos;
        Timer timer;

        public ScrollSelector(Transform contentParent, float duration)
        {
            this.contentParent = contentParent;
            this.CurrentSelection = 0;
            this.CurrentPos = contentParent.GetChild(0).position;
            
            this.newSelection = 0;
            this.startPos = CurrentPos;
            this.endPos = CurrentPos;
            this.timer = new Timer(duration);
        }

        public bool IsInTransition() => newSelection != CurrentSelection;

        public void Update(float deltaTime, Vector2 scrollInput)
        {
            UpdateInput(scrollInput);
            if (newSelection == CurrentSelection) return;
            timer.Update(deltaTime);
            CurrentPos = Vector3.Lerp(startPos, endPos, timer.NormalizedTime);

            if (timer.IsDone == false) return;
            CurrentSelection = newSelection;
            timer.Restart();
        }
        
        void UpdateInput(Vector2 scrollInput)
        {
            newSelection -= (int)scrollInput.y;
            newSelection = XIVMathInt.Repeat(newSelection, contentParent.childCount);
            if (newSelection == CurrentSelection) return;
            
            startPos = contentParent.GetChild(CurrentSelection).position;
            endPos = contentParent.GetChild(newSelection).position;
        }
    }
}