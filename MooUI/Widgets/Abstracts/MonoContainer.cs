using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets.Abstracts
{
    /// <summary>
    /// Can contain a single MooWidget.
    /// </summary>
    public abstract class MonoContainer : Container
    {
        public MooWidget Content { get; protected set; }

        public MonoContainer(int width, int height) : base(width, height) { }

        public virtual void SetContent(MooWidget w)
        {
            Content?.SetParent(null);
            Content = w;
            w.SetParent(this);

            w.Render();
        }

        public override void RemoveChild(MooWidget w)
        {
            if (Content == w)
            {
                Content?.SetParent(null);
                Content = null;

                Render();
            }
        }
        public void RemoveChild()
        {
            if (Content != null)
            {
                RemoveChild(Content);
            }
        }

        public override void Replace(MooWidget oldW, MooWidget newW)
        {
            if (oldW == Content)
            {
                SetContent(newW);
            }
        }

        public override void RefreshStyle()
        {
            base.RefreshStyle();

            Content?.SetStyle(Style, false);
        }

        #region INPUT (DEFAULT)

        public override void OnKeyDown()
        {
            base.OnKeyDown();

            Content?.OnKeyDown();
        }
        public override void OnKeyUp()
        {
            base.OnKeyUp();

            Content?.OnKeyUp();
        }

        public override void OnMouseMove(CellEventArgs e)
        {
            base.OnMouseMove(e);

            Content?.OnMouseMove(e);
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();

            Content?.OnMouseEnter();
        }
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            Content?.OnMouseLeave();
        }

        public override void OnLeftDown()
        {
            base.OnLeftDown();

            Content?.OnLeftDown();
        }
        public override void OnRightDown()
        {
            base.OnRightDown();

            Content?.OnRightDown();
        }
        public override void OnLeftUp()
        {
            base.OnLeftUp();

            Content?.OnLeftUp();
        }
        public override void OnRightUp()
        {
            base.OnRightUp();

            Content?.OnRightUp();
        }

        public override void OnMouseWheel(int delta)
        {
            base.OnMouseWheel(delta);

            Content?.OnMouseWheel(delta);
        }

        public override void Focus()
        {
            Content?.Focus();
        }
        public override void Unfocus()
        {
            Content?.Unfocus();
        }

        #endregion
    }
}
