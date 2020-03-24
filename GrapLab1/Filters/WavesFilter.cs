using System;
using System.Drawing;

namespace GrapLab1
{
    class WavesFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int k = (int)(x + 20 * Math.Sin(2.0 * Math.PI * y / 60.0));
            int l = y;

            if ((k < sourseImage.Width - 1) && (l < sourseImage.Height - 1) && (k > 0) && (l > 0))
            {
                Color resultColor = sourseImage.GetPixel(k, l);
                return resultColor;
            }
            return Color.Transparent;
        }
    }
}
