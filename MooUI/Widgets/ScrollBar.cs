using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    class ScrollBar : Container
    {
        public enum HoverRegion
        {
            NONE,
            UP,
            DOWN,
            LEFT,
            RIGHT,
            CONTENT,
            UDGRIP,
            LRGRIP,
            UDTRACK,
            LRTRACK
        }
        public HoverRegion Region { get; private set; }

        public bool XScrollBarVisible { get; private set; }
        public bool YScrollBarVisible { get; private set; }
        public int XStart { get; private set; }
        public int YStart { get; private set; }
        private int XTrackSize { get; set; }
        private int YTrackSize { get; set; }
        private int XGripSize { get; set; }
        private int YGripSize { get; set; }

        public ScrollBar(int width, int height) : base(width, height)
        {
            Region = HoverRegion.NONE;

            XScrollBarVisible = false;
            YScrollBarVisible = false;
            XStart = 0;
            YStart = 0;
            XTrackSize = Width;
            YTrackSize = Height;
            XGripSize = 0;
            YGripSize = 0;
        }

        public void ScrollX(int amount)
        {
            XStart += amount;

            if (FocusedElement != null && XStart + Width > FocusedElement.Width)
            {
                XStart = FocusedElement.Width - Width;
            }
            if (XStart < 0)
            {
                XStart = 0;
            }

            Render();
        }
        public void ScrollY(int amount)
        {
            YStart += amount;

            if (FocusedElement != null && YStart + Height > FocusedElement.Height)
            {
                YStart = FocusedElement.Height - Height;
            }
            if (YStart < 0)
            {
                YStart = 0;
            }

            Render();
        }
        public void MaxScroll()
        {
            XStart = FocusedElement.Width - Width;
            if (XStart < 0)
            {
                XStart = 0;
            }
            YStart = FocusedElement.Height - Height;
            if (YStart < 0)
            {
                YStart = 0;
            }
        }
        public void MinScroll()
        {
            XStart = 0;
            YStart = 0;
        }

        private void Content_OnResize(object sender, EventArgs e)
        {
            CalculateScrollInfo();
        }
        public void CalculateScrollInfo()
        {
            YScrollBarVisible = (FocusedElement.Height > Height);
            if (YScrollBarVisible)
            {
                YTrackSize = Height - 2;

                float windowContentRatio = (float)Height / FocusedElement.Height;
                YGripSize = (int)(YTrackSize * windowContentRatio);

                if (YGripSize < 1)
                    YGripSize = 1;
                if (YGripSize > YTrackSize)
                    YGripSize = YTrackSize;
            }

            XScrollBarVisible = (FocusedElement.Width > Width);
            if (XScrollBarVisible)
            {
                XTrackSize = YScrollBarVisible? Width - 3 : Width - 2;

                float windowContentRatio = (float)Width / FocusedElement.Width;
                XGripSize = (int)(XTrackSize * windowContentRatio);

                if (XGripSize < 1)
                    XGripSize = 1;
                if (XGripSize > XTrackSize)
                    XGripSize = XTrackSize;
            }

            RefreshStyle();
        }

        #region CONTAINER

        public void AddChild(MooWidget w)
        {
            if (FocusedElement != null)
            {
                RemoveChild(FocusedElement);
            }

            FocusedElement = w;
            w.SetParent(this);

            w.OnResize += Content_OnResize; ;

            CalculateScrollInfo();

            w.Render();
        }

        public override void RemoveChild(MooWidget w)
        {
            if(FocusedElement == w && FocusedElement != null)
            {
                FocusedElement.SetParent(null);
                FocusedElement = null;

                w.OnResize -= Content_OnResize;

                Render();
            }
        }
        public override void Replace(MooWidget oldW, MooWidget newW)
        {
            RemoveChild(oldW);
            AddChild(newW);
        }
        public override IEnumerable<MooWidget> GetChildren()
        {
            return new MooWidget[] { FocusedElement };
        }

        public override void RefreshStyle()
        {
            base.RefreshStyle();

            FocusedElement?.SetStyle(Style, false);
        }

        #endregion

        protected void DrawTrack()
        {
            if (YScrollBarVisible)
            {
                Visual.FillForeColor(Style.GetColor("DefaultFore"), Width - 1, 0, 1, Height);
                Visual.FillBackColor(Style.GetColor("DefaultBack"), Width - 1, 1, 1, Height - 1);
                Visual.FillChar('│', Width - 1, 1, 1, Height - 1);

                Visual.BackColors[Width - 1, 0] = Style.GetColor("InteractableBack");
                Visual.BackColors[Width - 1, Height - 1] = Style.GetColor("InteractableBack");

                Visual.Chars[Width - 1, 0] = '˄';
                Visual.Chars[Width - 1, Height - 1] = '˅';
            }

            if (XScrollBarVisible)
            {
                Visual.FillForeColor(Style.GetColor("DefaultFore"), 0, Height - 1, Width, 1);

                if (YScrollBarVisible)
                {
                    Visual.FillBackColor(Style.GetColor("DefaultBack"), 1, Height - 1, Width - 3, 1);
                    Visual.FillChar('─', 1, Height - 1, Width - 3, 1);

                    Visual.BackColors[0, Height - 1] = Style.GetColor("InteractableBack");
                    Visual.BackColors[Width - 2, Height - 1] = Style.GetColor("InteractableBack");

                    Visual.Chars[0, Height - 1] = '˂';
                    Visual.Chars[Width - 2, Height - 1] = '˃';
                }
                else
                {
                    Visual.FillBackColor(Style.GetColor("DefaultBack"), 1, Height - 1, Width - 2, 1);
                    Visual.FillChar('─', 1, Height - 1, Width - 1, 1);

                    Visual.BackColors[0, Height - 1] = Style.GetColor("InteractableBack");
                    Visual.BackColors[Width - 1, Height - 1] = Style.GetColor("InteractableBack");

                    Visual.Chars[0, Height - 1] = '˂';
                    Visual.Chars[Width - 1, Height - 1] = '˃';
                }
            }

            switch (Region)
            {
                case HoverRegion.UP:
                    Visual.BackColors[Width - 1, 0] = Style.GetColor("HoverBack");
                    break;
                case HoverRegion.DOWN:
                    Visual.BackColors[Width - 1, Height - 1] = Style.GetColor("HoverBack");
                    break;
                case HoverRegion.LEFT:
                    Visual.BackColors[0, Height - 1] = Style.GetColor("HoverBack");
                    break;
                case HoverRegion.RIGHT:
                    if (YScrollBarVisible)
                    {
                        Visual.BackColors[Width - 2, Height - 1] = Style.GetColor("HoverBack");
                    }
                    else
                    {
                        Visual.BackColors[Width - 1, Height - 1] = Style.GetColor("HoverBack");
                    }
                    break;
            }

            DrawGrip();
        }
        protected void DrawGrip()
        {
            if (XScrollBarVisible)
            {
                int scrollAreaSize = FocusedElement.Width - Width;
                float windowPositionRatio = (float)XStart / scrollAreaSize;
                int trackScrollAreaSize = XTrackSize - XGripSize;
                int gripPositionOnTrack = (int)(trackScrollAreaSize * windowPositionRatio);

                Visual.FillChar('█', gripPositionOnTrack + 1, Height - 1, XGripSize, 1);
            }
            if (YScrollBarVisible)
            {
                int scrollAreaSize = FocusedElement.Height - Height;
                float windowPositionRatio = (float)YStart / scrollAreaSize;
                int trackScrollAreaSize = YTrackSize - YGripSize;
                int gripPositionOnTrack = (int)(trackScrollAreaSize * windowPositionRatio);

                Visual.FillChar('█', Width - 1, gripPositionOnTrack + 1, 1, YGripSize);
            }
        }

        public override void Draw()
        {
            if(FocusedElement != null)
            {
                Visual.Merge(FocusedElement.Visual, 0, 0, XStart, YStart, Width, Height);

                DrawTrack();
            }
        }

        #region INPUT

        protected void SetHoverRegion(HoverRegion r)
        {
            if (r != Region)
            {
                if (r == HoverRegion.CONTENT)
                {
                    FocusedElement?.OnMouseEnter();
                }
                else
                {
                    if (Region == HoverRegion.CONTENT)
                        FocusedElement?.OnMouseLeave();
                }

                Region = r;

                Render();
            }
        }

        public override void OnMouseMove(CellEventArgs e)
        {
            if (YScrollBarVisible)
            {
                if (e.X == Width - 1 && e.Y == 0)
                {
                    SetHoverRegion(HoverRegion.UP);
                    return;
                }
                else if (e.X == Width - 1 && e.Y == Height - 1)
                {
                    SetHoverRegion(HoverRegion.DOWN);
                    return;
                }
                else if (e.X == Width - 1)
                {
                    SetHoverRegion(HoverRegion.UDTRACK);
                    return;
                }
            }
            if (XScrollBarVisible)
            {
                if (e.X == 0 && e.Y == Height - 1)
                {
                    SetHoverRegion(HoverRegion.LEFT);
                    return;
                }
                else if (YScrollBarVisible && e.X == Width - 2 && e.Y == Height - 1)
                {
                    SetHoverRegion(HoverRegion.RIGHT);
                    return;
                }
                else if (!YScrollBarVisible && e.X == Width - 1 && e.Y == Height - 1)
                {
                    SetHoverRegion(HoverRegion.RIGHT);
                    return;
                }
                else if (e.Y == Height - 1)
                {
                    SetHoverRegion(HoverRegion.LRTRACK);
                    return;
                }
            }

            SetHoverRegion(HoverRegion.CONTENT);
        }

        public override void OnMouseEnter() { }
        public override void OnMouseLeave()
        {
            Region = HoverRegion.NONE;
            Render();

            FocusedElement?.OnMouseLeave();
        }

        public override void OnLeftDown()
        {
            switch (Region)
            {
                case HoverRegion.CONTENT:
                    FocusedElement?.OnLeftDown();
                    break;
                case HoverRegion.UP:
                    ScrollY(-1);
                    break;
                case HoverRegion.DOWN:
                    ScrollY(1);
                    break;
                case HoverRegion.LEFT:
                    ScrollX(-1);
                    break;
                case HoverRegion.RIGHT:
                    ScrollX(1);
                    break;
            }
        }

        public override void OnMouseWheel(int delta)
        {
            if (KeyboardState.Shift || Region == HoverRegion.LRTRACK || !YScrollBarVisible)
            {
                if (delta < 0)
                {
                    ScrollX(-1);
                }
                else
                {
                    ScrollX(1);
                }
            }
            else
            {
                if (delta < 0)
                {
                    ScrollY(1);
                }
                else
                {
                    ScrollY(-1);
                }
            }
        }

        #endregion
    }
}
