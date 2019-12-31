using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MooUI.Widgets
{
    class Spacer : MooWidget
    {
        public Spacer(int width, int height) : base(width, height) 
        {
            Visual.FillBackColor(Colors.Transparent);
            Visual.FillForeColor(Colors.Transparent);
        }
    }
}
