using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI
{
    public class CellEventArgs
    {
        private int x;
        private int y;

        public int X { get => x; }
        public int Y { get => y; }

        public bool IsHandled { get; set; }

        public CellEventArgs(int _x, int _y)
        {
            x = _x;
            y = _y;
            IsHandled = false;
        }
    }
}
