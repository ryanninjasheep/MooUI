using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    public class TextBox : TextBlock
    {
        public string HintText { get; private set; }

        public TextBox(int width, int height) : base(width, height, "") 
        {
            HintText = "";
        }
        public TextBox(int width, int height, string hintText) : this(width, height)
        {
            HintText = hintText;
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

        public override void Focus()
        {
            base.Focus();

            Render();
        }
        public override void Unfocus()
        {
            base.Unfocus();

            Render();
        }

        public override void OnKeyDown()
        {
            base.OnKeyDown();

            if (KeyboardState.KeyIsChar && Text.Length < Width * Height)
            {
                SetText(Text + KeyboardState.GetCharInput(KeyboardState.LastKeyPressed));
            }
            else if(KeyboardState.LastKeyPressed == System.Windows.Input.Key.Back && Text.Length > 0)
            {
                SetText(Text.Substring(0, Text.Length - 1));
            }
        }

        public override void Draw()
        {
            if (Text.Length == 0 && HintText != null)
            {
                Visual.FillForeColor(Style.GetColor("HintFore"));
                Visual.SetText(HintText);
            }
            else
            {
                Visual.FillForeColor(Style.GetColor("DefaultFore"));
                Visual.SetText(Text);
            }

            if (IsFocused)
            {
                Visual.FillBackColor(Style.GetColor("ActiveBack"));
            }
            else
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
}
