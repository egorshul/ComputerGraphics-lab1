using System.Drawing;
using System.ComponentModel;

namespace GrapLab1
{
    class GrayWorldFilter : Filters
    {
        protected int Avg;
        protected int R, G, B;
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color c = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(Clamp(c.R * Avg / R, 0, 255), Clamp(c.G * Avg / G, 0, 255), Clamp(c.B * Avg / B, 0, 255));
            return resultColor;
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            R = 0; G = 0; B = 0; Avg = 0;
            double progress = 0.0;

            for (int i = 0; i < sourceImage.Width; i++, progress += 0.5)
            {
                worker.ReportProgress((int)((float)progress / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color sourceColor = sourceImage.GetPixel(i, j);
                    R += sourceColor.R;
                    G += sourceColor.G;
                    B += sourceColor.B;
                }
            }

            R = R / (sourceImage.Width * sourceImage.Height);
            G = G / (sourceImage.Width * sourceImage.Height);
            B = B / (sourceImage.Width * sourceImage.Height);
            Avg = (R + G + B) / 3;

            for (int i = 0; i < sourceImage.Width; i++, progress += 0.5)
            {
                worker.ReportProgress((int)((float)progress / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
            }
            return resultImage;
        }
    }
}
