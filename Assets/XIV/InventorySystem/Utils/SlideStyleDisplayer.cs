using System;
using UnityEngine;
using UnityEngine.UI;
using XIV.XIVMath;

namespace XIV.Utils
{
    struct SlotData
    {
        public Image slot;
        public Quaternion rotation;
        public Vector3 position;
        public Vector3 scale;

        public SlotData(Image slot, bool isLocal)
        {
            this.slot = slot;
            var rectTransform = slot.rectTransform;
            if (isLocal)
            {
                rotation = rectTransform.localRotation;
                position = rectTransform.localPosition;
                scale = rectTransform.localScale;
            }
            else
            {
                rotation = rectTransform.rotation;
                position = rectTransform.position;
                scale = rectTransform.lossyScale;
            }
        }
    }
    
    public struct SlideStyleDisplayer
    {
        int slotsLength;
        int spritesLength;
        
        SlotData[] slotDatas;
        Sprite[] sprites;
        int spriteIndex;
        bool movingNext;
        bool switched;
        Timer timer;

        public SlideStyleDisplayer(Image[] slots, Sprite[] sprites, float slideDuration, bool displayImmediate = true)
        {
            this.slotsLength = slots.Length;
            this.spritesLength = sprites.Length;
            
            this.slotDatas = new SlotData[slotsLength];
            for (int i = 0; i < slotsLength; i++)
            {
                this.slotDatas[i] = new SlotData(slots[i], true);
            }
            this.sprites = sprites;
            this.spriteIndex = 0;
            this.movingNext = true;
            this.switched = false;
            this.timer = new Timer(slideDuration);

            if (displayImmediate) ResetDisplay();
        }

        public void ResetDisplay()
        {
            spriteIndex = 0;
            for (int i = 0; i < slotsLength; i++)
            {
                slotDatas[i].slot.sprite = sprites[spriteIndex];
                spriteIndex = XIVMathInt.Repeat(spriteIndex + 1, spritesLength);
            }
        }

        public bool UpdateNext(float deltaTime)
        {
            bool isDone = timer.Update(deltaTime);
            var normalizedTime = timer.NormalizedTime;
            var normalizedTimePingPong = timer.NormalizedTimePingPong;

            if (switched == false && normalizedTime > 0.5f)
            {
                switched = true;
                ShiftLeft();
                slotDatas[^1].slot.sprite = sprites[spriteIndex];
                spriteIndex = XIVMathInt.Repeat(spriteIndex + 1, spritesLength);
            }
            
            if (normalizedTime < 0.5f)
            {
                ScaleForward(normalizedTimePingPong);
                MoveForward(normalizedTimePingPong);
            }
            else
            {
                ScaleBack(normalizedTimePingPong);
                MoveBack(normalizedTimePingPong);
            }

            if (isDone == false) return false;
            
            switched = false;
            timer.Restart();
            return true;
        }

        void MoveForward(float normalizedTime)
        {
            for (int i = 0; i < slotsLength - 1; i++)
            {
                int moveToIndex = i + 1;
                var slotData = slotDatas[i];
                var rectTransform = slotData.slot.rectTransform;
                rectTransform.localPosition = Vector3.Lerp(slotData.position, slotDatas[moveToIndex].position, normalizedTime);
            }
        }

        void MoveBack(float normalizedTime)
        {
            for (int i = 0; i < slotsLength - 1; i++)
            {
                int moveToIndex = i + 1;
                var slotData = slotDatas[i];
                var rectTransform = slotData.slot.rectTransform;
                rectTransform.localPosition = Vector3.Lerp(slotDatas[moveToIndex].position, slotData.position, normalizedTime);
            }
        }

        void ScaleForward(float normalizedTime)
        {
            for (int i = 0; i < slotsLength; i++)
            {
                var rectTransform = slotDatas[i].slot.rectTransform;
                rectTransform.localScale = Vector3.Lerp(slotDatas[i].scale, Vector3.zero, normalizedTime);
            }
        }

        void ScaleBack(float normalizedTime)
        {
            for (int i = 0; i < slotsLength; i++)
            {
                var rectTransform = slotDatas[i].slot.rectTransform;
                rectTransform.localScale = Vector3.Lerp(Vector3.zero, slotDatas[i].scale, normalizedTime);
            }
        }

        // public void MovePrevious()
        // {
        //     if (movingNext)
        //     {
        //         movingNext = false;
        //         spriteIndex = XIVMathInt.Repeat(spriteIndex - slotsLength - 1, spritesLength);
        //     }
        //     
        //     ShiftRight();
        //     slotDatas[0].slot.sprite = sprites[spriteIndex];
        //     spriteIndex = XIVMathInt.Repeat(spriteIndex - 1, spritesLength);
        // }

        void ShiftLeft()
        {
            for (int i = 0; i < slotsLength - 1; i++)
            {
                slotDatas[i].slot.sprite = slotDatas[i + 1].slot.sprite;
            }

            slotDatas[^1].slot.sprite = slotDatas[0].slot.sprite;
        }

        void ShiftRight()
        {
            for (int i = slotsLength - 1; i > 0; i--)
            {
                slotDatas[i].slot.sprite = slotDatas[i - 1].slot.sprite;
            }

            slotDatas[0].slot.sprite = slotDatas[^1].slot.sprite;
        }
        
    }
}