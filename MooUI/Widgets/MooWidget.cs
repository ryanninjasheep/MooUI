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
        bool isDefaultStyle;

        protected Container Parent { get; set; }

        public bool IsMouseOver { get; private set; }
        public bool IsFocused { get; set; }

        public MooWidget(int width, int height)
        {
            Style = new MooStyle();
            isDefaultStyle = true;

            IsMouseOver = false;
            IsFocused = false;

            Visual = new MooVisual(width, height);
            Visual.FillForeColor(Style.GetColor("DefaultFore"));
            Visual.FillBackColor(Style.GetColor("DefaultBack"));
        }

        public void SetParent(Container c)
        {
            Parent?.RemoveChild(this);
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

        #region STYLE

        public void SetStyle(MooStyle s, bool forceChange)
        {
            if (isDefaultStyle || forceChange)
            {
                Style = s;

                if (forceChange)
                {
                    isDefaultStyle = false;
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

        public event EventHandler OnRender;
        public void Render()
        {
            Draw();

            Parent?.Render();

            EventHandler handler = OnRender;
            handler?.Invoke(this, EventArgs.Empty);
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
