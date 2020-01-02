using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MooUI
{
    public static class MooShaders
    {
        /// <summary>
        /// Factor < 1 darkens, > 1 lightens
        /// </summary>
        public static Func<Color[,], int, int, Color> Lighten(float factor)
        {
            if (factor < 0)
            {
                throw new ArgumentOutOfRangeException("Factor must be greater than 0!");
            }
            else
            {
                return (colors, x, y) =>
                {
                    Color c = colors[x, y];

                    float r = ((c.R * factor) > 255) ? 255 : (c.R * factor);
                    float g = ((c.G * factor) > 255) ? 255 : (c.G * factor);
                    float b = ((c.B * factor) > 255) ? 255 : (c.B * factor);

                    return Color.FromRgb((byte)r, (byte)g, (byte)b);
                };
            }
        }

        /// <summary>
        /// Fills with one color
        /// </summary>
        public static Func<Color[,], int, int, Color> Fill(Color fill)
        {
            return (colors, x, y) =>
            {
                return fill;
            };
        }
    }
}
