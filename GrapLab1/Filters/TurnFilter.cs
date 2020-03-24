using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace GrapLab1
{
    class TurnFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int x0 = (int)(sourseImage.Width / 2), y0 = (int)(sourseImage.Height / 2);
            double w = Math.PI / 2.0;
            int k = (int)((x - x0) * Math.Cos(w) - (y - y0) * Math.Sin(w) + x0);
            int l = (int)((x - x0) * Math.Sin(w) + (y - y0) * Math.Cos(w) + y0);

            if ((k < sourseImage.Width - 1) && (l < sourseImage.Height - 1) && (k > 0) && (l > 0))
            {
                Color resultColor = sourseImage.GetPixel(k, l);
                return resultColor;
            }
            return Color.Transparent;

        }
    }
}