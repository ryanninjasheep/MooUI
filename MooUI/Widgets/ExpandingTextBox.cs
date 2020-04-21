using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    public class ExpandingTextBox : Abstracts.Delegator<ScrollBar<TextBox>>
    {
        public enum ExpansionDirection { RIGHT, DOWN }
        public ExpansionDirection Direction { get; private set; }
        public int MinSize { get; private set; }

        public string Text { get => TB.Text; }
        public ScrollBar<TextBox> ScrollBar { get => Content; }
        public TextBox TB { get => Content.GetContent(); }

        public ExpandingTextBox(int displayWidth, int minWidth)
            : base(displayWidth, 2, new ScrollBar<TextBox>(displayWidth, 2, new TextBox(minWidth, 1)))
        {
            Direction = ExpansionDirection.RIGHT;
            MinSize = minWidth;
        }
        public ExpandingTextBox(int width, int displayHeight, int minHeight)
            : base(width + 1, displayHeight, new ScrollBar<TextBox>(width + 1, displayHeight, new TextBox(width, minHeight)))
        {
            Direction = ExpansionDirection.DOWN;
            MinSize = minHeight;
        }

        public override void OnKeyDown()
        {
            if (KeyboardState.KeyIsChar)
            {
                if (Direction == ExpansionDirection.RIGHT && Text.Length == TB.Width - 1)
                {
                    TB.Resize(TB.Width + 1, 1);

                    ScrollBar.MaxScroll();
                }
                else if (Direction == ExpansionDirection.DOWN && (Text.Length + 1) >= TB.Width * TB.Height)
                {
                    TB.Resize(TB.Width, TB.Height + 1);

                    ScrollBar.MaxScroll();
                }

                TB.SetText(Text + KeyboardState.GetCharInput(KeyboardState.LastKeyPressed));
            }
            else if (KeyboardState.LastKeyPressed == System.Windows.Input.Key.Back && Text.Length > 0)
            {
                if (Direction == ExpansionDirection.RIGHT && TB.Width > MinSize)
                {
                    TB.Resize(TB.Width - 1, 1);

                    ScrollBar.MaxScroll();
                }
                else if (Direction == ExpansionDirection.DOWN && Text.Length % TB.Width == 0 && TB.Height > MinSize)
                {
                    TB.Resize(TB.Width, TB.Height - 1);

                    ScrollBar.MaxScroll();
                }

                TB.SetText(Text.Substring(0, Text.Length - 1));
            }
        }
    }
}
