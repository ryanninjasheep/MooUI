using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets.Abstracts
{
    public abstract class Modal : MooWidget
    {
        public bool CanClickOutsideToClose { get; protected set; }
        public bool DarkenOutside { get; protected set; }

        public Modal(int width, int height) : base(width, height)
        {
            CanClickOutsideToClose = true;
            DarkenOutside = true;
        }

        public event EventHandler<Modal> OnClose;
        public void Close()
        {
            EventHandler<Modal> handler = OnClose;
            handler?.Invoke(this, this);

            Render();
        }
    }
}
