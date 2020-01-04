using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using MooUI.Widgets;

namespace MooUI
{
    /// <summary>
    /// A MooViewer is the intermediary between MooUI elements and WPF.
    /// </summary>
    public class MooViewer : FrameworkElement
    {
        private readonly GlyphTypeface glyphTypeface;
        private readonly double fontSize = 13;
        private readonly double cellWidth = 7;
        private readonly double cellHeight = 15;

        public MooWindow Window { get; set; }

        // TODO: If MooViewer is resized, update content size
        private int MaxContentWidth { get; set; }
        private int MaxContentHeight { get; set; }

        public MooViewer()
        {
            FontFamily family = new FontFamily("Consolas");
            Typeface typeface = new Typeface(family,
                FontStyles.Normal,
                FontWeights.Normal,
                FontStretches.Normal);

            typeface.TryGetGlyphTypeface(out glyphTypeface);

            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent, new KeyEventHandler(OnKeyDown), true);
            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyUpEvent, new KeyEventHandler(OnKeyUp), true);

            //TEMP
            Width = 900;
            Height = 500;
            UpdateSize();

            Window = new MooWindow(MaxContentWidth, MaxContentHeight);
            Window.SetStyle(MooStyle.Test, false);
            Window.OnRender += Content_Render;

            Accordion a = new Accordion(MaxContentWidth, MaxContentHeight);

            a.AddChild(new ExpandingTextBox(20, 5));

            SetContent(a);
        }

        public void SetContent(MooWidget w)
        {
            Window.SetContent(w);
        }

        #region RENDERING
        private void Content_Render(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // Background
            for (int j = 0; j < Window.Height; j++) // Go row by row
            {
                int cursorX = 0;
                for (int i = 0; i < Window.Width; i++)
                {
                    if (Window.Visual.BackColors[i, j] != Window.Visual.BackColors[cursorX, j])
                    {
                        DrawBackground(cursorX, j, i - cursorX, dc);
                        cursorX = i;
                    }
                }
                DrawBackground(cursorX, j, Window.Width - cursorX, dc);
            }

            // Foreground
            for (int j = 0; j < Window.Height; j++) // Go row by row
            {
                int cursorX = 0;
                for (int i = 0; i < Window.Width; i++)
                {
                    if (Window.Visual.ForeColors[i, j] != Window.Visual.ForeColors[cursorX, j])
                    {
                        DrawGlyphRun(cursorX, j, i - cursorX, dc);
                        cursorX = i;
                    }
                }
                DrawGlyphRun(cursorX, j, Window.Width - cursorX, dc);
            }
        }
        private void DrawGlyphRun(int xIndex, int yIndex, int length, DrawingContext dc) // Assumes all glyphs are the same color
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = Window.Visual.Chars[xIndex + i, yIndex];
            }

            ushort[] charIndexes = new ushort[length];
            double[] advanceWidths = new double[length];
            for (int i = 0; i < length; i++)
            {
                charIndexes[i] = glyphTypeface.CharacterToGlyphMap[chars[i]];
                advanceWidths[i] = cellWidth;
            }

            GlyphRun g = new GlyphRun((float)1.25);
            ISupportInitialize isi = g;
            isi.BeginInit();
            {
                g.GlyphTypeface = glyphTypeface;
                g.FontRenderingEmSize = fontSize;
                g.GlyphIndices = charIndexes;
                g.AdvanceWidths = advanceWidths;
                g.BaselineOrigin = new Point(xIndex * cellWidth, yIndex * cellHeight + glyphTypeface.Baseline * fontSize);
            }
            isi.EndInit();

            dc.DrawGlyphRun(new SolidColorBrush(Window.Visual.ForeColors[xIndex, yIndex]), g);
        }
        private void DrawBackground(int xIndex, int yIndex, int length, DrawingContext dc) // Assumes all same color
        {
            dc.DrawRectangle(new SolidColorBrush(Window.Visual.BackColors[xIndex, yIndex]), null,
                new Rect(xIndex * cellWidth, yIndex * cellHeight, length * cellWidth + 1, cellHeight + 1));
        }

        public void UpdateSize()
        {
            MaxContentWidth = (int)Math.Floor(Width / cellWidth);
            MaxContentHeight = (int)Math.Floor(Height / cellHeight);
        }
        #endregion

        #region INPUT HANDLING

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyboardState.HandleKeyDown(e);
            Window.OnKeyDown();
        }
        protected void OnKeyUp(object sender, KeyEventArgs e)
        {
            KeyboardState.HandleKeyUp(e);
            Window.OnKeyUp();
        }

        CellEventArgs MouseLocation { get; set; }

        private CellEventArgs GetCellAtMousePosition()
        {
            Point p = Mouse.GetPosition(this);
            int x = (int)Math.Floor(p.X / cellWidth);
            int y = (int)Math.Floor(p.Y / cellHeight);
            return new CellEventArgs(x, y);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            MouseLocation = GetCellAtMousePosition();

            Window.OnMouseMove(MouseLocation);

        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            MouseLocation = null;

            Window.OnMouseLeave();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            CellEventArgs c = GetCellAtMousePosition();
            if (MouseLocation == null || c.X != MouseLocation.X || c.Y != MouseLocation.Y)
            {
                // If cell changed...

                MouseLocation = c;

                Window.OnMouseMove(MouseLocation);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            Window.OnLeftDown();
        }
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            Window.OnRightDown();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            Window.OnMouseWheel(e.Delta);
        }

        #endregion
    }
}
