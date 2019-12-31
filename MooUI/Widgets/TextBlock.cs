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
        public TextBlock(string text) : base(text.Length, 1) { }

        public void SetText(string s)
        {
            Text = s;

            Visual.SetText(Text);

            Render();
        }

        public override void RefreshStyle()
        {
            Visual.FillBackColor(Style.GetColor("StaticBack"));
        }
    }
}
