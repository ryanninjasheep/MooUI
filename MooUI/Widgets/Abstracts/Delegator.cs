using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets.Abstracts
{
    /// <summary>
    /// A container that protects content from outside interaction.  Defaults to immediately delegating all input to child.
    /// </summary>
    public abstract class Delegator<T> : Container where T : MooWidget
    {
        protected T Content { get; set; }

        public Delegator(int width, int height) : base(width, height) { }

        protected virtual void SetContent(T w)
        {
            Content?.SetParent(null);
            Content = w;
            w.SetParent(this);

            w.Render();
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
