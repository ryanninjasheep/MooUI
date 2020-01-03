using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    class Alert : Abstracts.Modal
    {
        public Alert(int width, int height, string text) : base(width, height, false, true)
        {
            Accordion a = new Accordion(width, height);
            a.AddChild(new TextBlock(text));
            Button b = new Button("Got it!");
            b.Click += Button_Click;
            a.AddChild(b);

            SetContent(a);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
