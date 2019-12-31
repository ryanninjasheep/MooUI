using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MooUI
{
    public class MooStyle
    {
        Dictionary<string, Color> Style;

        public MooStyle()
        {
            Style = new Dictionary<string, Color>();
        }
        public MooStyle(Dictionary<string, Color> style)
        {
            Style = style;
        }

        public Color GetColor(string key)
        {
            if(Style.ContainsKey(key))
            {
                return Style[key];
            }
            else
            {
                return DefaultDark.Style[key];
            }
        }

        public static readonly MooStyle DefaultDark = new MooStyle(new Dictionary<string, Color>()
        {
            { "DefaultFore", Colors.White },
            { "DefaultBack", Colors.Black },
            { "StaticBack", Colors.Gray },
            { "InteractableBack", Colors.DarkSlateGray },
            { "ActiveBack", Colors.SlateBlue },
            { "HoverBack", Colors.SlateGray },
            { "HintFore", Colors.LightSlateGray },
        });
        public static readonly MooStyle Pastel = new MooStyle(new Dictionary<string, Color>()
        {
            { "DefaultFore", Colors.Black },
            { "DefaultBack", Colors.FloralWhite },
            { "StaticBack", Colors.White },
            { "InteractableBack", Colors.Lavender },
            { "ActiveBack", Colors.Thistle },
            { "HoverBack", Colors.Honeydew },
            { "HintFore", Colors.LightSlateGray },
        });
        public static readonly MooStyle Purple = new MooStyle(new Dictionary<string, Color>()
        {
            { "DefaultFore", Color.FromRgb(0xee, 0xee, 0xee) },
            { "DefaultBack", Color.FromRgb(0x35, 0x2f, 0x44) },
            { "StaticBack", Color.FromRgb(0x39, 0x3e, 0x46) },
            { "InteractableBack", Color.FromRgb(0x2a, 0x24, 0x38) },
            { "ActiveBack", Color.FromRgb(0x31, 0x0a, 0x5d) },
            { "HoverBack", Color.FromRgb(0x41, 0x1e, 0x8f) },
            { "HintFore", Colors.LightSlateGray },
        });
        public static readonly MooStyle Test = new MooStyle(new Dictionary<string, Color>()
        {
            { "DefaultFore", Color.FromRgb(0xee, 0xee, 0xee) },
            { "DefaultBack", Color.FromRgb(0x35, 0x2f, 0x44) },
            { "StaticBack", Color.FromRgb(0x39, 0x3e, 0x46) },
            { "InteractableBack", Color.FromRgb(0x2a, 0x24, 0x38) },
            { "ActiveBack", Color.FromRgb(0x31, 0x0a, 0x5d) },
            { "HoverBack", Color.FromRgb(0x41, 0x1e, 0x8f) },
            { "HintFore", Colors.LightSlateGray },
        });
    }
}
