using MooUI.Widgets.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    public class MooWindow : MonoContainer
    {
        private List<ModalWithLocation> Modals { get; set; }
        private Modal ActiveModal { get; set; }
        private bool IsModalHovered { get; set; }

        public MooWindow(int width, int height) : base(width, height) 
        {
            Modals = new List<ModalWithLocation>();
            ActiveModal = null;
            IsModalHovered = false;
        }

        #region MODAL INTERACTION

        public void PushModal(Modal m, int x, int y)
        {
            // TODO - Maybe add system to make sure there are no duplicates?

            Modals.Add(new ModalWithLocation(m, x, y));
            m.OnClose += Modal_OnClose;

            UpdateActiveModal();

            Render();
        }

        public void UpdateActiveModal()
        {
            if (Modals.Count > 0)
            {
                ActiveModal = Modals[Modals.Count - 1].Modal;
            }
            else
            {
                ActiveModal = null;
            }
        }

        private void Modal_OnClose(object sender, Modal e)
        {
            ModalWithLocation m = Modals.Find(m => m.Modal == e);

            if (m != null)
            {
                Modals.Remove(m);
                m.Modal.OnClose -= Modal_OnClose;

                UpdateActiveModal();

                Render();
            }
        }

        #endregion

        public event EventHandler OnRender;
        public override void Draw()
        {
            if (Content != null)
            {
                Visual.Merge(Content.Visual, 0, 0);
            }

            for (int i = 0; i < Modals.Count; i++)
            {
                if (i == Modals.Count - 1 && Modals[i].Modal.DarkenOutside)
                {
                    Visual.ApplyShader(MooShaders.Lighten(0.8), true, true);
                }

                Visual.Merge(Modals[i].Modal.Visual, Modals[i].X, Modals[i].Y);
            }

            EventHandler handler = OnRender;
            handler?.Invoke(this, EventArgs.Empty);
        }

        #region INPUT

        public override void OnKeyDown() 
        {
            if (ActiveModal != null)
            {
                ActiveModal.OnKeyDown();
            }
            else
            {
                base.OnKeyDown();
            }
        }
        public override void OnKeyUp()
        {
            if (ActiveModal != null)
            {
                ActiveModal.OnKeyUp();
            }
            else
            {
                base.OnKeyUp();
            }
        }

        public override void OnMouseMove(CellEventArgs e)
        {
            if (ActiveModal != null)
            {
                ModalWithLocation m = Modals[Modals.Count - 1];

                CellEventArgs relativeCell = new CellEventArgs(e.X - m.X, e.Y - m.Y);

                if (relativeCell.X < 0 || relativeCell.X > ActiveModal.Width || relativeCell.Y < 0 || relativeCell.Y > ActiveModal.Height)
                {
                    if (IsModalHovered)
                    {
                        IsModalHovered = false;
                        ActiveModal.OnMouseLeave();
                    }
                }
                else
                {
                    if (!IsModalHovered)
                        ActiveModal.OnMouseEnter();

                    ActiveModal.OnMouseMove(relativeCell);
                }
            }
            else
            {
                base.OnMouseMove(e);
            }
        }

        public override void OnMouseEnter()
        {
            if (ActiveModal == null)
                base.OnMouseEnter();
        }
        public override void OnMouseLeave()
        {
            if (ActiveModal == null)
                base.OnMouseLeave();
        }

        public override void OnLeftDown()
        {
            if (ActiveModal != null)
            {
                if (IsModalHovered)
                {
                    ActiveModal.OnLeftDown();
                }
                else if (ActiveModal.CanClickOutsideToClose)
                {
                    ActiveModal.Close();
                }
            }
            else
            {
                base.OnLeftDown();
            }
        }
        public override void OnRightDown()
        {
            if (ActiveModal != null && IsModalHovered)
            {
                ActiveModal.OnRightDown();
            }
            else
            {
                base.OnRightDown();
            }
        }
        public override void OnLeftUp()
        {
            if (ActiveModal != null && IsModalHovered)
            {
                ActiveModal.OnLeftUp();
            }
            else
            {
                base.OnLeftUp();
            }
        }
        public override void OnRightUp()
        {
            if (ActiveModal != null && IsModalHovered)
            {
                ActiveModal.OnRightUp();
            }
            else
            {
                base.OnRightUp();
            }
        }

        public override void OnMouseWheel(int delta)
        {
            if (ActiveModal != null)
            {
                ActiveModal.OnMouseWheel(delta);
            }
            else
            {
                base.OnMouseWheel(delta);
            }
        }

        public override void Focus()
        {
            if (ActiveModal != null)
            {
                ActiveModal.Focus();
            }
            else
            {
                base.Focus();
            }
        }
        public override void Unfocus()
        {
            if (ActiveModal != null)
            {
                ActiveModal.Unfocus();
            }
            else
            {
                base.Unfocus();
            }
        }

        #endregion

        private class ModalWithLocation
        {
            public Modal Modal { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }

            public ModalWithLocation(Modal modal, int x, int y)
            {
                Modal = modal;
                X = x;
                Y = y;
            }
        }
    }
}
