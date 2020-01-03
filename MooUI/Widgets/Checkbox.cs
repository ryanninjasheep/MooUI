using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    public class Checkbox : MooWidget
    {
        public bool Checked { get; private set; }
        public string Text { get; private set; }

        private Checkbox(int width, int height) : base(width, height)
        {
            Checked = false; // default state
        }
        public Checkbox(string text) : this(text.Length + 2, 1)
        {
            SetText(text);
        }

        public void SetText(string s)
        {
            Text = s;
            Render();
        }
        public void Check()
        {
            Checked = true;
            Render();
        }
        public void Uncheck()
        {
            Checked = false;
            Render();
        }

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

        public override void OnLeftDown()
        {
            base.OnLeftDown();

            FindWindow().PushModal(new Alert(30, 10, "Checkbox clicked!"), 30, 10);

            if (Checked)
            {
                Uncheck();
            }
            else
            {
                Check();
            }
        }

        public override void Draw()
        {
            if (IsMouseOver)
            {
                Visual.BackColors[0, 0] = Style.GetColor("HoverBack");
            }
            else
            {
                Visual.BackColors[0, 0] = Style.GetColor("InteractableBack");
            }

            if (Checked)
            {
                Visual.SetText("x " + Text);
            }
            else
            {
                Visual.SetText("  " + Text);
            }
        }
    }
}
