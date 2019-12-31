using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    class Accordion : Container
    {
        protected List<MooWidget> Children { get; set; }

        private MooWidget HoveredElement { get; set; }

        public Accordion(int width, int height) : base(width, height)
        {
            Children = new List<MooWidget>();
        }

        #region CONTAINER

        public void AddChild(MooWidget w)
        {
            Children.Add(w);
            w.SetParent(this);
            w.Render();
        }
        public override void RemoveChild(MooWidget w)
        {
            if(Children.Contains(w))
            {
                Children.Remove(w);
                w.SetParent(null);

                Render();
            }
        }
        public override void Replace(MooWidget oldW, MooWidget newW)
        {
            if (Children.Contains(oldW))
            {
                RemoveChild(oldW);
                AddChild(newW);
            }
        }
        public override IEnumerable<MooWidget> GetChildren()
        {
            return Children;
        }

        public override void RefreshStyle()
        {
            base.RefreshStyle();

            foreach (MooWidget w in Children)
            {
                w.SetStyle(Style, false);
            }
        }

        #endregion

        #region SELECTION

        public void SetHoveredElement(MooWidget w)
        {
            if (HoveredElement != w)
            {
                HoveredElement?.OnMouseLeave();
                HoveredElement = w;
                HoveredElement?.OnMouseEnter();
            }
        }

        #endregion

        #region INPUT

        public override void OnMouseMove(CellEventArgs e)
        {
            int runningHeight = 0;
            foreach (MooWidget w in Children)
            {
                if (runningHeight + w.Height > e.Y)
                {
                    if (e.X < w.Width)
                    {
                        SetHoveredElement(w);
                        w.OnMouseMove(new CellEventArgs(e.X, e.Y - runningHeight));
                        return;
                    }
                    break;
                }

                runningHeight += w.Height;
            }

            // if nothing is hovered over
            SetHoveredElement(null);
        }
        public override void OnMouseLeave()
        {
            SetHoveredElement(null);
        }

        public override void OnLeftDown()
        {
            HoveredElement?.OnLeftDown();
            SetFocus(HoveredElement);
        }
        public override void OnRightDown()
        {
            HoveredElement?.OnRightDown();
            SetFocus(HoveredElement);
        }

        public override void OnMouseWheel(int delta)
        {
            HoveredElement?.OnMouseWheel(delta);
        }

        #endregion

        public override void Draw()
        {
            int runningHeight = 0;
            foreach (MooWidget w in Children)
            {
                Visual.Merge(w.Visual, 0, runningHeight);

                runningHeight += w.Height;
                if (runningHeight > Height)
                {
                    break;
                }
            }
        }
    }
}
