using System.Drawing;
using System.ComponentModel;
using System;

namespace GrapLab1
{
    class CorrectionWithReferenceColor : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            double Rsrc = 124, Gsrc = 149, Bsrc = 171;  // Опорный цвет [для Image3]

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).R * (255 - Math.Abs(Rsrc - calculateNewPixelColor(sourceImage, i, j).R)) / Rsrc), 0, 255);
                    int newG = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).G * (255 - Math.Abs(Gsrc - calculateNewPixelColor(sourceImage, i, j).G)) / Gsrc), 0, 255);
                    int newB = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).B * (255 - Math.Abs(Bsrc - calculateNewPixelColor(sourceImage, i, j).B)) / Bsrc), 0, 255);
                    resultImage.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return resultImage;
        }
    }
}
