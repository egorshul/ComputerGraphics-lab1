using System;
using System.Drawing;


namespace GrapLab1
{
    class GlassFilter : Filters
    {
        private Random rand = new Random();
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int k, l;
            k = Clamp((int)(x + (rand.NextDouble() - 0.5) * 10), 0, sourceImage.Width - 1);
            l = Clamp((int)(y + (rand.NextDouble() - 0.5) * 10), 0, sourceImage.Height - 1);
            Color sourceColor = sourceImage.GetPixel(k, l);
            Color resultColor = Color.FromArgb(sourceColor.R, sourceColor.G, sourceColor.B);

            return resultColor;
        }
    }
}