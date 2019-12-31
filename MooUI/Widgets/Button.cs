using System;
using System.Collections.Generic;
using System.Text;
using MooUI;

namespace MooUI.Widgets
{
    class Button : TextBlock
    {
        public Button(int width, int height, string text) : base(width, height, text) { }
        public Button(string text) : base(text) { }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();

            Render();
        }
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            Render();
        }

        public event EventHandler Click;
        public override void OnLeftDown()
        {
            base.OnLeftDown();

            OnClick();
        }
        protected virtual void OnClick()
        {
            EventHandler handler = Click;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public override void Draw()
        {
            if (IsMouseOver)
            {
                Visual.FillBackColor(Style.GetColor("HoverBack"));
            }
            else
            {
                Visual.FillBackColor(Style.GetColor("InteractableBack"));
            }
        }

    }
}
