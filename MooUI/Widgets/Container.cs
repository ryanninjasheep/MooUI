using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    public abstract class Container : MooWidget
    {
        public MooWidget FocusedElement { get; protected set; }

        public Container(int width, int height) : base(width, height) { }

        public abstract void RemoveChild(MooWidget w);
        public abstract void Replace(MooWidget oldW, MooWidget newW);
        public abstract IEnumerable<MooWidget> GetChildren();

        public void SetFocus(MooWidget w)
        {
            if (FocusedElement != w)
            {
                FocusedElement?.Unfocus();
                FocusedElement = w;
                FocusedElement?.Focus();
            }
        }

        #region INPUT (DEFAULT)

        public override void OnKeyDown() 
        {
            base.OnKeyDown();

            FocusedElement?.OnKeyDown();
        }
        public override void OnKeyUp()
        {
            base.OnKeyUp();

            FocusedElement?.OnKeyUp();
        }

        public override void OnMouseMove(CellEventArgs e) 
        {
            base.OnMouseMove(e);

            FocusedElement?.OnMouseMove(e);
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();

            FocusedElement?.OnMouseEnter();
        }
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            FocusedElement?.OnMouseLeave();
        }

        public override void OnLeftDown() 
        {
            base.OnLeftDown();

            FocusedElement?.OnLeftDown();
        }
        public override void OnRightDown() 
        {
            base.OnRightDown();

            FocusedElement?.OnRightDown();
        }
        public override void OnLeftUp()
        {
            base.OnLeftUp();

            FocusedElement?.OnLeftUp();
        }
        public override void OnRightUp() 
        {
            base.OnRightUp();

            FocusedElement?.OnRightUp();
        }

        public override void OnMouseWheel(int delta)
        {
            base.OnMouseWheel(delta);

            FocusedElement?.OnMouseWheel(delta);
        }

        public override void Focus()
        {
            FocusedElement?.Focus();
        }
        public override void Unfocus()
        {
            FocusedElement?.Unfocus();
        }

        #endregion
    }
}
