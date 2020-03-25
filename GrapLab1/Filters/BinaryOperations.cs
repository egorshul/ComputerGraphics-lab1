using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;


namespace GrapLab1
{
    class Morfology : Filters
    {
        protected bool isDilation;
        protected int[,] mask = null;
        protected Morfology()
        {
            mask = new int[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        }
        public Morfology(int[,] mask)
        {
            this.mask = mask;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = mask.GetLength(0) / 2;
            int radiusY = mask.GetLength(1) / 2;
            int minR = 255; int minG = 255; int minB = 255;
            int maxR = 0; int maxG = 0; int maxB = 0;
            for (int i = -radiusX; i <= radiusX; i++)
                for (int j = -radiusY; j <= radiusY; j++)
                {
                    if (isDilation)
                    {
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).R > maxR))
                            maxR = sourceImage.GetPixel(x + i, y + j).R;
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).G > maxG))
                            maxG = sourceImage.GetPixel(x + i, y + j).G;
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).B > maxB))
                            maxB = sourceImage.GetPixel(x + i, y + j).B;
                    }
                    else
                    {
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).R < minR))
                            minR = sourceImage.GetPixel(x + i, y + j).R;
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).G < minG))
                            minG = sourceImage.GetPixel(x + i, y + j).G;
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).B < minB))
                            minB = sourceImage.GetPixel(x + i, y + j).B;
                    }
                }
            if (isDilation)
                return Color.FromArgb(maxR, maxG, maxB);
            else
                return Color.FromArgb(minR, minG, minB);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            int radiusX = mask.GetLength(0) / 2;
            int radiusY = mask.GetLength(1) / 2;
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = radiusX; i < sourceImage.Width - radiusX; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = radiusY; j < sourceImage.Height - radiusY; j++)
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
            }
            return resultImage;
        }
    }

    class Dilation : Morfology 
    {
        public Dilation()
        {
            isDilation = true;
        }
        public Dilation(int[,] mask)
        {
            this.mask = mask;
            isDilation = true;
        }
    }

    class Erosion : Morfology 
    {
        public Erosion()
        {
            isDilation = false;
        }
        public Erosion(int[,] mask)
        {
            this.mask = mask;
            isDilation = false;
        }
    }

    class Opening : Morfology 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Erosion(mask);
            Bitmap result = filter1.processImage(sourceImage, worker);
            Filters filter2 = new Dilation(mask);
            result = filter2.processImage(result, worker);
            return result;
        }
    }

    class Closing : Morfology 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Dilation(mask);
            Bitmap result = filter1.processImage(sourceImage, worker);
            Filters filter2 = new Erosion(mask);
            result = filter2.processImage(result, worker);
            return result;
        }
    }

    class TopHat : Morfology
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Erosion(mask);
            Bitmap result1 = filter1.processImage(sourceImage, worker);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp(sourceImage.GetPixel(i, j).R - result1.GetPixel(i, j).R, 0, 255);
                    int newG = Clamp(sourceImage.GetPixel(i, j).G - result1.GetPixel(i, j).G, 0, 255);
                    int newB = Clamp(sourceImage.GetPixel(i, j).B - result1.GetPixel(i, j).B, 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }

    class BlackHat : Morfology
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Dilation(mask);
            Bitmap result1 = filter1.processImage(sourceImage, worker);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp(result1.GetPixel(i, j).R - sourceImage.GetPixel(i, j).R, 0, 255);
                    int newG = Clamp(result1.GetPixel(i, j).G - sourceImage.GetPixel(i, j).G, 0, 255);
                    int newB = Clamp(result1.GetPixel(i, j).B - sourceImage.GetPixel(i, j).B, 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }

    class Grad : Morfology 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Dilation(mask);
            Bitmap result1 = filter1.processImage(sourceImage, worker);
            Filters filter2 = new Erosion(mask);
            Bitmap result2 = filter2.processImage(sourceImage, worker);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp(result1.GetPixel(i, j).R - result2.GetPixel(i, j).R, 0, 255);
                    int newG = Clamp(result1.GetPixel(i, j).G - result2.GetPixel(i, j).G, 0, 255);
                    int newB = Clamp(result1.GetPixel(i, j).B - result2.GetPixel(i, j).B, 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }
}
