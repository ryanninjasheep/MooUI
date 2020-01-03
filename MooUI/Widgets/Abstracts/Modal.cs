using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets.Abstracts
{
    public abstract class Modal : MonoContainer
    {
        public bool CanClickOutsideToClose { get; protected set; }
        public bool DarkenOutside { get; protected set; }

        public Modal(int width, int height, bool canClickOutsideToClose, bool darkenOutside) : base(width, height)
        {
            CanClickOutsideToClose = canClickOutsideToClose;
            DarkenOutside = darkenOutside;
        }

        public event EventHandler<Modal> OnClose;
        public void Close()
        {
            EventHandler<Modal> handler = OnClose;
            handler?.Invoke(this, this);

            Render();
        }

        public override void Draw()
        {
            if (Content != null)
            {
                Visual.Merge(Content.Visual, 0, 0);
            }
        }
    }
}
