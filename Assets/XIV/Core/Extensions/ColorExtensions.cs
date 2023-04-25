using UnityEngine;

namespace XIV.Core.Extensions
{
    public static class ColorExtensions
    {
        public static Color SetR(this Color color, float value)
        {
            color.r = value;
            return color;
        }
        
        public static Color SetG(this Color color, float value)
        {
            color.g = value;
            return color;
        }
        
        public static Color SetB(this Color color, float value)
        {
            color.b = value;
            return color;
        }
        
        public static Color SetA(this Color color, float value)
        {
            color.a = value;
            return color;
        }
    }
}