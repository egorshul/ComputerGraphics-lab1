using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace GrapLab1
{
    class CarryFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int k, l;
            Color result;

            if (x >= sourceImage.Width - 50)
                return result = Color.FromArgb(0, 0, 0);

            k = Clamp(x + 50, 0, sourceImage.Width - 1);
            l = Clamp(y, 0, sourceImage.Height - 1);
            Color sourceColor = sourceImage.GetPixel(k, l);
            result = Color.FromArgb(sourceColor.R, sourceColor.G, sourceColor.B);

            return result;
        }
    }
}
