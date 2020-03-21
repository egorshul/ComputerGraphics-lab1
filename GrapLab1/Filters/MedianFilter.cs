using System.Drawing;
using System.ComponentModel;
using System;

namespace GrapLab1
{
    class MedianFilter : Filters
    {
        protected int Avg;
        protected int[] newAvg;
        protected const int size = 9;
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            newAvg = new int[size];
            int k = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    Color currColor = sourceImage.GetPixel(Clamp(x + i, 0, sourceImage.Width - 1), Clamp(y + j, 0, sourceImage.Height - 1));
                    newAvg[k++] = (currColor.R + currColor.G + currColor.B) / 3;
                }
            Color sourceColor = sourceImage.GetPixel(x, y);
            Avg = qsort(newAvg, 0, size - 1);
            Color resultColor = Color.FromArgb(Clamp(Avg, 0, 255), Clamp(Avg, 0, 255), Clamp(Avg, 0, 255));
            return resultColor;
        }
        protected int qsort(int[] a, int l, int r)
        {
            int x = a[l + (r - l) / 2], i = l, j = r, temp;
            while (i <= j)
            {
                while (a[i] < x) i++;
                while (a[j] > x) j--;
                if (i <= j)
                {
                    temp = a[i]; a[i] = a[j]; a[j] = temp;
                    i++;
                    j--;
                }
            }
            if (i < r) qsort(a, i, r);
            if (l < j) qsort(a, l, j);
            return a[l + (r - l) / 2];
        }
    }
}