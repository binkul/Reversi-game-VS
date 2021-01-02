using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Reversi
{
    static class ColorMixer
    {
        public static Color Lerp(this Color color, Color otherColor, double balance)
        {
            byte r = (byte)(balance * color.R + (1 - balance) * otherColor.R);
            byte g = (byte)(balance * color.G + (1 - balance) * otherColor.G);
            byte b = (byte)(balance * color.B + (1 - balance) * otherColor.B);
            return Color.FromRgb(r, g, b);
        }

        public static SolidColorBrush Lerp(this SolidColorBrush brush, SolidColorBrush otherBrush, double balance)
        {
            return new SolidColorBrush(Lerp(brush.Color, otherBrush.Color, balance));
        }
    }
}
