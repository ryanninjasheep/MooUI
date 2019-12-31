using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    class ExpandingTextBox : TextBox
    {
        public bool ExpandRight { get; set; }
        public int MaxDisplayDimension { get; private set; }

        public ExpandingTextBox(int width, int maxDisplayWidth) : base(width, 1) 
        {
            ExpandRight = true;
            MaxDisplayDimension = maxDisplayWidth;
        }
        public ExpandingTextBox(int width, int height, int maxDisplayHeight) : base(width, height)
        {
            ExpandRight = false;
            MaxDisplayDimension = maxDisplayHeight;
        }

        public override void OnKeyDown()
        {
            if (KeyboardState.KeyIsChar)
            {
                if (ExpandRight)
                {
                    if (Text.Length == Width - 1)
                    {
                        Resize(Width + 1, 1);

                        if (Width >= MaxDisplayDimension)
                        {
                            if (Parent is ScrollBar s)
                            {
                                s.MaxScroll();
                            }
                            else
                            {
                                ScrollBar t = new ScrollBar(MaxDisplayDimension, 2);
                                Parent?.Replace(this, t);
                                t.SetContent(this);
                            }
                            
                        }
                    }
                }
                else
                {
                    if ((Text.Length + 1) % Width == 0)
                    {
                        Resize(Width, Height + 1);

                        if (Height >= MaxDisplayDimension)
                        {
                            if (Parent is ScrollBar s)
                            {
                                s.MaxScroll();
                            }
                            else
                            {
                                ScrollBar t = new ScrollBar(Width + 1, MaxDisplayDimension);
                                Parent?.Replace(this, t);
                                t.SetContent(this);
                            }

                        }
                    }
                }

                SetText(Text + KeyboardState.GetCharInput(KeyboardState.LastKeyPressed));
            }
            else
            {
                if (KeyboardState.LastKeyPressed == System.Windows.Input.Key.Back && Text.Length > 0)
                {
                    if (ExpandRight)
                    {
                        Resize(Width - 1, 1);

                        if (Parent is ScrollBar s)
                        {
                            s.MaxScroll();
                        }
                    }
                    else
                    {
                        if (Text.Length % Width == 0)
                        {
                            Resize(Width, Height - 1);

                            if (Parent is ScrollBar s)
                            {
                                s.MaxScroll();
                            }
                        }
                    }
                    SetText(Text.Substring(0, Text.Length - 1));
                }
            }
        }
    }
}
