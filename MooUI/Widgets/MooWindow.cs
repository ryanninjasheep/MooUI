using MooUI.Widgets.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    public class MooWindow : MonoContainer
    {
        // TODO: Figure out how context menus and stuff work with this!

        public MooWindow(int width, int height) : base(width, height) { }

        public event EventHandler OnRender;
        public override void Draw()
        {
            if (Content != null)
            {
                Visual.Merge(Content.Visual, 0, 0);
            }

            EventHandler handler = OnRender;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
