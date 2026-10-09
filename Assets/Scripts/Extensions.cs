using UnityEngine;
using UnityEngine.UI;

namespace JPO
{
    public static class Extensions
    {
        /// <summary>
        /// Set the alpha of a UI element color
        /// </summary>
        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            var color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }

        /// <summary>
        /// Set the alpha of a sprite renderer color
        /// </summary>
        public static void SetAlpha(this SpriteRenderer rend, float alpha)
        {
            var color = rend.color;
            color.a = alpha;
            rend.color = color;
        }
    }
}