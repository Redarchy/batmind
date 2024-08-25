using UnityEngine;

namespace Batmind.Utils
{
    public static class ColorExtensions
    {
        public static Color WithR(this Color color, float r)
        {
            color.r = r;
            return color;
        }
        
        public static Color WithG(this Color color, float g)
        {
            color.g = g;
            return color;
        }
        
        public static Color WithB(this Color color, float b)
        {
            color.b = b;
            return color;
        }
        
        public static Color WithA(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        public static Color FromInt(int r, int g, int b, int a = 255)
        {
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
        
    }
}