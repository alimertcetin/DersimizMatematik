using UnityEngine;
using XIV.Core.Extensions;
using XIV.InventorySystem;

namespace LessonIsMath.InventorySystem.Items
{
    public enum KeycardType
    {
        None = 0,
        Green = 1,
        Yellow = 2,
        Red = 4,
    }

    [System.Serializable]
    public class KeycardItem : ItemBase
    {
        public KeycardType KeycardType;

        public string GetColoredCardString()
        {
            switch (KeycardType)
            {
                case KeycardType.Green:
                    return KeycardType.ToString().Green();
                case KeycardType.Yellow:
                    return KeycardType.ToString().Yellow();
                case KeycardType.Red:
                    return KeycardType.ToString().Red();

                default:
                    return "None";
            }
        }

        public override bool Equals(ItemBase other)
        {
            if (other is not KeycardItem otherItem) return false;

            return Object.Equals(otherItem, this);
        }
    }
}