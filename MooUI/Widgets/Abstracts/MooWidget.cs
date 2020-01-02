using MooUI.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI
{
    public abstract class MooWidget
    {
        public MooVisual Visual { get; set; }
        public int Width { get => Visual.Width; }
        public int Height { get => Visual.Height; }

        protected MooStyle Style { get; set; }
        protected bool IsDefaultStyle { get; set; }

        protected Container Parent { get; set; }

        protected bool IsMouseOver { get; set; }
        protected bool IsFocused { get; set; }

        protected MooWidget(int width, int height)
        {
            Style = new MooStyle();
            IsDefaultStyle = true;

            IsMouseOver = false;
            IsFocused = false;

            Visual = new MooVisual(width, height);
            Visual.FillForeColor(Style.GetColor("DefaultFore"));
            Visual.FillBackColor(Style.GetColor("DefaultBack"));
        }

        internal void SetParent(Container c)
        {
            if (Parent is Widgets.Abstracts.MultiContainer m)
            {
                m.RemoveChild(this);
            }

            Parent = c;

            if (Parent != null)
            {
                SetStyle(Parent.Style, false);
            }
        }

        public event EventHandler OnResize;
        public void Resize(int width, int height)
        {
            Visual = new MooVisual(width, height);
            RefreshStyle();

            EventHandler handler = OnResize;
            handler?.Invoke(this, EventArgs.Empty);

            Render();
        }

        public MooWindow FindWindow()
        {
            if (Parent == null)
            {
                return null;
            }
            else if (Parent is MooWindow m)
            {
                return m;
            }
            else
            {
                return Parent.FindWindow();
            }
        }

        #region STYLE

        public void SetStyle(MooStyle s, bool forceChange)
        {
            if (IsDefaultStyle || forceChange)
            {
                Style = s;

                if (forceChange)
                {
                    IsDefaultStyle = false;
                }

                RefreshStyle();
                Render();
            }
        }
        public virtual void RefreshStyle()
        {
            Visual.FillForeColor(Style.GetColor("DefaultFore"));
            Visual.FillBackColor(Style.GetColor("DefaultBack"));
            Visual.FillChar(' ');
        }

        #endregion

        #region RENDERING

        public virtual void Draw() { }

        public void Render()
        {
            Draw();

            Parent?.Render();
        }

        #endregion

        #region INPUT

        public virtual void OnKeyDown() { }
        public virtual void OnKeyUp() { }

        public virtual void OnMouseMove(CellEventArgs e) { }

        public virtual void OnMouseEnter()
        {
            IsMouseOver = true;
        }
        public virtual void OnMouseLeave()
        {
            IsMouseOver = false;
        }

        public virtual void OnLeftDown() { }
        public virtual void OnRightDown() { }
        public virtual void OnLeftUp() { }
        public virtual void OnRightUp() { }

        public virtual void OnMouseWheel(int delta) { }

        public virtual void Focus()
        {
            if (IsFocused)
                return;
            IsFocused = true;
        }
        public virtual void Unfocus()
        {
            if (!IsFocused)
                return;
            IsFocused = false;
        }

        #endregion
    }
}
