using MooUI.Widgets.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    public class ScrollBar<T> : Delegator<T> where T : MooWidget
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

        public ScrollBar(int width, int height, T content) : base(width, height, content)
        {
            Region = HoverRegion.NONE;

            CalculateScrollInfo();
        }

        #region SCROLL

        public void CalculateScrollInfo()
        {
            YScrollBarVisible = (Content.Height > Height);
            if (YScrollBarVisible)
            {
                YTrackSize = Height - 2;

                float windowContentRatio = (float)Height / Content.Height;
                YGripSize = (int)(YTrackSize * windowContentRatio);

                if (YGripSize < 1)
                    YGripSize = 1;
                if (YGripSize > YTrackSize)
                    YGripSize = YTrackSize;
            }

            XScrollBarVisible = (Content.Width > Width);
            if (XScrollBarVisible)
            {
                XTrackSize = YScrollBarVisible ? Width - 3 : Width - 2;

                float windowContentRatio = (float)Width / Content.Width;
                XGripSize = (int)(XTrackSize * windowContentRatio);

                if (XGripSize < 1)
                    XGripSize = 1;
                if (XGripSize > XTrackSize)
                    XGripSize = XTrackSize;
            }

            RefreshStyle();
        }

        public void ScrollX(int amount)
        {
            XStart += amount;

            if (Content != null && XStart + Width > Content.Width)
            {
                XStart = Content.Width - Width;
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

            if (Content != null && YStart + Height > Content.Height)
            {
                YStart = Content.Height - Height;
            }
            if (YStart < 0)
            {
                YStart = 0;
            }

            Render();
        }
        public void MaxScroll()
        {
            XStart = Content.Width - Width;
            if (XStart < 0)
            {
                XStart = 0;
            }
            YStart = Content.Height - Height;
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

        #endregion

        #region CONTAINER

        protected override void SetContent(T w)
        {
            if (Content != null)
            {
                Content.OnResize -= Content_OnResize;
            }

            base.SetContent(w);

            w.OnResize += Content_OnResize;
        }

        private void Content_OnResize(object sender, EventArgs e)
        {
            CalculateScrollInfo();
        }

        public T GetContent()
        {
            return Content;
        }

        #endregion

        #region RENDERING

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
                int scrollAreaSize = Content.Width - Width;
                float windowPositionRatio = (float)XStart / scrollAreaSize;
                int trackScrollAreaSize = XTrackSize - XGripSize;
                int gripPositionOnTrack = (int)(trackScrollAreaSize * windowPositionRatio);

                Visual.FillChar('█', gripPositionOnTrack + 1, Height - 1, XGripSize, 1);
            }
            if (YScrollBarVisible)
            {
                int scrollAreaSize = Content.Height - Height;
                float windowPositionRatio = (float)YStart / scrollAreaSize;
                int trackScrollAreaSize = YTrackSize - YGripSize;
                int gripPositionOnTrack = (int)(trackScrollAreaSize * windowPositionRatio);

                Visual.FillChar('█', Width - 1, gripPositionOnTrack + 1, 1, YGripSize);
            }
        }

        public override void Draw()
        {
            if(Content != null)
            {
                Visual.Merge(Content.Visual, 0, 0, XStart, YStart, Width, Height);

                DrawTrack();
            }
        }

        #endregion

        #region INPUT

        protected void SetHoverRegion(HoverRegion r)
        {
            if (r != Region)
            {
                if (r == HoverRegion.CONTENT)
                {
                    Content?.OnMouseEnter();
                }
                else
                {
                    if (Region == HoverRegion.CONTENT)
                        Content?.OnMouseLeave();
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
            if (Content != null && e.X < Content.Width && e.Y < Content.Height)
            {
                SetHoverRegion(HoverRegion.CONTENT);
                return;
            }
            else
            {
                SetHoverRegion(HoverRegion.NONE);
                return;
            }
        }

        public override void OnMouseEnter()
        {
            // This is empty so that the base isn't called - we don't want the content to automatically be notified
        }
        public override void OnMouseLeave()
        {
            SetHoverRegion(HoverRegion.NONE);
        }

        public override void OnLeftDown()
        {
            switch (Region)
            {
                case HoverRegion.CONTENT:
                    Content?.OnLeftDown();
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
