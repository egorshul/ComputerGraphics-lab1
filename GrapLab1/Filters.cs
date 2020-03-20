using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GrapLab1
{
    abstract class Filters
    {
        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y);

        public virtual Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for(int i=0; i<sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for(int j=0; j<sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }

    class InvertFilter: Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R,
                                               255 - sourceColor.G,
                                               255 - sourceColor.B);
            return resultColor;
        }
    }

    class GrayScaleFilter: Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)((float)0.299 * sourceColor.R + (float)0.587 * sourceColor.G + (float)0.114 * sourceColor.B);
            Color resultColor = Color.FromArgb(Intensity,
                                               Intensity,
                                               Intensity);
            return resultColor;
        }
    }

    class SepiaFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int k = 25;
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)((float)0.299 * sourceColor.R + (float)0.587 * sourceColor.G + (float)0.114 * sourceColor.B);
            Color resultColor = Color.FromArgb(Clamp(Intensity + 2 * k, 0, 255),
                                              Clamp((int)(Intensity + (float)0.5 * k), 0, 255),
                                              Clamp((Intensity - 1 * k), 0, 255));
            return resultColor;
        }
    }

    class IncreaseBrightness : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(Clamp(sourceColor.R + 10, 0, 255),
                                              Clamp(sourceColor.G + 10, 0, 255),
                                              Clamp(sourceColor.B + 10, 0, 255));
            return resultColor;
        }
    }

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
    class PerfectReflectorFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            double Rmax = 0, Gmax = 0, Bmax = 0;
            double progress = 0.0;

            for (int i = 0; i < sourceImage.Width; i++, progress += 0.5)
            {
                worker.ReportProgress((int)((float)progress / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    if (sourceImage.GetPixel(i, j).R > Rmax) Rmax = sourceImage.GetPixel(i, j).R;
                    if (sourceImage.GetPixel(i, j).G > Gmax) Gmax = sourceImage.GetPixel(i, j).G;
                    if (sourceImage.GetPixel(i, j).B > Bmax) Bmax = sourceImage.GetPixel(i, j).B;
                }
            }

            for (int i = 0; i < sourceImage.Width; i++, progress += 0.5)
            {
                worker.ReportProgress((int)((float)progress / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).R * 255 / Rmax), 0, 255);
                    int newG = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).G * 255 / Gmax), 0, 255);
                    int newB = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).B * 255 / Bmax), 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }
    




    class MatrixFilter : Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        {
            this.kernel = kernel;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            
            for(int l = -radiusY; l<=radiusY; l++)
                for(int k = -radiusX; k<=radiusX; k++)
                {
                    int idx = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idy = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighboorColor = sourceImage.GetPixel(idx, idy);
                    resultR += neighboorColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighboorColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighboorColor.B * kernel[k + radiusX, l + radiusY];
                }
            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255)
                );
        }
    }

    class BlurFilter : MatrixFilter
    {
        public BlurFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for(int i=0; i<sizeX; i++)
                for(int j=0; j<sizeY;j++)
                    kernel[i, j] = 1.0f / (float)(sizeX * sizeY);
        }
    }
    class GaussianFilter : MatrixFilter
    {
        public GaussianFilter()
        {
            createGaussianKernel(3, 2);
        }

        public void createGaussianKernel(int radius, float sigma)
        {
            int size = 2 * radius + 1;
            kernel = new float[size, size];
            float norm = 0;
            for(int i = -radius; i <= radius; i++)
                for(int j = -radius; j <= radius; j++)
                {
                    kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / (2 * sigma * sigma)));
                    norm += kernel[i + radius, j + radius];
                }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= norm;

        }
    }

    class SobelFilter: MatrixFilter
    {
        public SobelFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            kernel[0, 0] = -1;
            kernel[0, 1] = -2;
            kernel[0, 2] = -1;
            kernel[1, 0] = 0;
            kernel[1, 1] = 0;
            kernel[1, 2] = 0;
            kernel[2, 0] = 1;
            kernel[2, 1] = 2;
            kernel[2, 2] = 1;
        }
    }

    class IncreaseSharpness: MatrixFilter
    {
        public IncreaseSharpness()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            kernel[0, 0] = 0;
            kernel[0, 1] = -1;
            kernel[0, 2] = -0;
            kernel[1, 0] = -1;
            kernel[1, 1] = 5;
            kernel[1, 2] = -1;
            kernel[2, 0] = 0;
            kernel[2, 1] = -1;
            kernel[2, 2] = 0;
        }
    }
}
