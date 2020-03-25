using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;



namespace GrapLab1
{
    class BinaryFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color s = sourceImage.GetPixel(x, y);
            if (s.R < 127 && s.G < 127 && s.B < 127)
                return Color.FromArgb(0, 0, 0);
            else
                return Color.FromArgb(255, 255, 255);
        }
    }
}
