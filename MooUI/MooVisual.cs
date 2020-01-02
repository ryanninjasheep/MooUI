using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MooUI
{
    /// <summary>
    /// Contains all data for rendering a visual, as well as helper functions for merging visuals.
    /// </summary>
    public class MooVisual
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Color[,] ForeColors { get; set; }
        public Color[,] BackColors { get; set; }
        public char[,] Chars { get; set; }

        public MooVisual(int getWidth, int getHeight)
        {
            Width = getWidth;
            Height = getHeight;

            ForeColors = new Color[Width, Height];
            BackColors = new Color[Width, Height];
            Chars = new char[Width, Height];
            FillChar(' ');
        }

        #region HELPER FUNCTIONS
        public void FillForeColor(Color fill)
        {
            FillForeColor(fill, 0, 0, Width, Height);
        }
        public void FillBackColor(Color fill)
        {
            FillBackColor(fill, 0, 0, Width, Height);
        }
        public void FillChar(char fill)
        {
            FillChar(fill, 0, 0, Width, Height);
        }
        public void FillForeColor(Color fill, int xStart, int yStart, int width, int height)
        {
            ApplyShader(MooShaders.Fill(fill), true, false, xStart, yStart, width, height);
        }
        public void FillBackColor(Color fill, int xStart, int yStart, int width, int height)
        {
            ApplyShader(MooShaders.Fill(fill), false, true, xStart, yStart, width, height);
        }
        public void FillChar(char fill, int xStart, int yStart, int width, int height)
        {
            for (int i = xStart; i < Width && i - xStart < width; i++)
            {
                for (int j = yStart; j < Height && j - yStart < height; j++)
                {
                    Chars[i, j] = fill;
                }
            }
        }

        public void SetText(string s)
        {
            FillChar(' ');

            int x = 0;
            int y = 0;
            foreach (char c in s)
            {
                if (x >= Width)
                {
                    x = 0;
                    y++;
                    if (y >= Height)
                    {
                        return;
                    }
                }

                Chars[x, y] = c;
                x++;
            }
        }

        /// <summary>
        /// Overlays the a Visual at the specified location.
        /// </summary>
        public void Merge(MooVisual v, int xIndex, int yIndex)
        {
            Merge(v, xIndex, yIndex, 0, 0, v.Width, v.Height);
        }
        public void Merge(MooVisual v, int xIndex, int yIndex, int xStart, int yStart, int width, int height)
        {
            for (int j = 0; j + yStart < v.Height && j + yIndex < Height && j < height; j++)
            {
                for (int i = 0; i + xStart < v.Width && i + xIndex < Width && i < width; i++)
                {
                    if (v.BackColors[i + xStart, j + yStart] != Colors.Transparent)
                    {
                        BackColors[i + xIndex, j + yIndex] = v.BackColors[i + xStart, j + yStart];
                    }
                    if (v.ForeColors[i + xStart, j + yStart] != Colors.Transparent)
                    {
                        ForeColors[i + xIndex, j + yIndex] = v.ForeColors[i + xStart, j + yStart];
                        Chars[i + xIndex, j + yIndex] = v.Chars[i + xStart, j + yStart];
                    }
                }
            }
        }

        public void ApplyShader(Func<Color[,], int, int, Color> shader, bool fore, bool back)
        {
            ApplyShader(shader, fore, back, 0, 0, Width, Height);
        }
        public void ApplyShader(Func<Color[,], int, int, Color> shader, bool fore, bool back, int xStart, int yStart, int width, int height)
        {
            for (int i = xStart; i < Width && i - xStart < width; i++)
            {
                for (int j = yStart; j < Height && j - yStart < height; j++)
                {
                    if (fore)
                    {
                        ForeColors[i, j] = shader(ForeColors, i, j);
                    }
                    if (back)
                    {
                        BackColors[i, j] = shader(BackColors, i, j);
                    }
                }
            }
        }
        #endregion
    }
}
