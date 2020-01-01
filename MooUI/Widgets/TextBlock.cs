using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    /// <summary>
    /// A big old dumb box.  Can't do anything except display text.
    /// </summary>
    class TextBlock : MooWidget
    {
        public string Text { get; set; }

        public TextBlock(int width, int height, string text) : base(width, height)
        {
            SetText(text);
        }
        public TextBlock(string text) : this(text.Length, 1, text) { }

        public void SetText(string s)
        {
            if (s.Length <= Width * Height)
            {
                Text = s;

                Visual.SetText(Text);

                Render();
            }
        }

        public override void RefreshStyle()
        {
            Visual.FillBackColor(Style.GetColor("StaticBack"));
        }
    }
}
