using UnityEngine;
using UnityEngine.UI;
using XIV.XIVMath;

namespace XIV.Utils
{
    public struct SlotStyleDisplayer
    {
        Image[] slots;
        Sprite[] sprites;
        int spriteIndex;
        bool movingNext;
        int slotsLength;
        int spritesLength;

        public SlotStyleDisplayer(Image[] slots, Sprite[] sprites, bool displayImmediate = true)
        {
            this.slots = slots;
            this.sprites = sprites;
            this.spriteIndex = 0;
            this.movingNext = true;
            this.slotsLength = this.slots.Length;
            this.spritesLength = this.sprites.Length;

            if (displayImmediate) ResetDisplay();
        }

        public void ResetDisplay()
        {
            spriteIndex = 0;
            for (int i = 0; i < slotsLength; i++)
            {
                slots[i].sprite = sprites[spriteIndex];
                spriteIndex = XIVMathInt.Repeat(spriteIndex + 1, spritesLength);
            }
        }
        
        public void MoveNext()
        {
            if (movingNext == false)
            {
                movingNext = true;
                spriteIndex = XIVMathInt.Repeat(spriteIndex + slotsLength + 1, spritesLength);
            }
            
            ShiftLeft();
            slots[^1].sprite = sprites[spriteIndex];
            spriteIndex = XIVMathInt.Repeat(spriteIndex + 1, spritesLength);
        }

        public void MovePrevious()
        {
            if (movingNext)
            {
                movingNext = false;
                spriteIndex = XIVMathInt.Repeat(spriteIndex - slotsLength - 1, spritesLength);
            }
            
            ShiftRight();
            slots[0].sprite = sprites[spriteIndex];
            spriteIndex = XIVMathInt.Repeat(spriteIndex - 1, spritesLength);
        }

        void ShiftLeft()
        {
            for (int i = 0; i < slotsLength - 1; i++)
            {
                slots[i].sprite = slots[i + 1].sprite;
            }

            slots[^1].sprite = slots[0].sprite;
        }

        void ShiftRight()
        {
            for (int i = slotsLength - 1; i > 0; i--)
            {
                slots[i].sprite = slots[i - 1].sprite;
            }

            slots[0].sprite = slots[^1].sprite;
        }
        
    }
}