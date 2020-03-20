using System.Drawing;
using System.ComponentModel;

namespace GrapLab1
{
    class LinearStretchingHistogram : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            int XminR = 0, XmaxR = 0, XmaxG = 0, XminG = 0, XmaxB = 0, XminB = 0;
            double progress = 0.0;

            for (int i = 0; i < sourceImage.Width; i++, progress += 0.5)
            {
                worker.ReportProgress((int)((float)progress / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color tmp = sourceImage.GetPixel(i, j);
                    if (XminR > tmp.R) 
                        XminR = tmp.R;

                    if (XmaxR < tmp.R) 
                        XmaxR = tmp.R;

                    if (XminG > tmp.G) 
                        XminG = tmp.G;

                    if (XmaxG < tmp.G) 
                        XmaxG = tmp.G;

                    if (XminB > tmp.B) 
                        XminB = tmp.B;

                    if (XmaxB < tmp.B) 
                        XmaxB = tmp.B;
                }
            }
            for (int i = 0; i < sourceImage.Width; i++, progress += 0.5)
            {
                worker.ReportProgress((int)((float)progress / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int R = sourceImage.GetPixel(i, j).R;
                    int G = sourceImage.GetPixel(i, j).G;
                    int B = sourceImage.GetPixel(i, j).B;
                    result.SetPixel(i, j, Color.FromArgb(Clamp(Clamp(((255 * (R - XminR)) / (XmaxR - XminR)), 0, 255) + R, 0, 255), 
                                                         Clamp(Clamp(((255 * (G - XminR)) / (XmaxG - XminG)), 0, 255) + G, 0, 255), 
                                                         Clamp(Clamp(((255 * (B - XminR)) / (XmaxB - XminB)), 0, 255) + B, 0, 255)));
                }
            }
            return result;
        }
    }
}
