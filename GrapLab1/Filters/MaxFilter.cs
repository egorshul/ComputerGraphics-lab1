using System.ComponentModel;
using System.Drawing;


namespace GrapLab1
{
    class MaxFilter : Filters
    {
        protected int avgR;
        protected int avgG;
        protected int avgB;
        protected int[] avg_r;
        protected int[] avg_g;
        protected int[] avg_b;
        protected const int size = 9;
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            avg_r = new int[size];
            avg_g = new int[size];
            avg_b = new int[size];
            int r = 0;
            int g = 0;
            int b = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    Color currColor = sourceImage.GetPixel(Clamp(x + i, 0, sourceImage.Width - 1), Clamp(y + j, 0, sourceImage.Height - 1));
                    avg_r[r++] = (int)currColor.R;
                    avg_g[g++] = (int)currColor.G;
                    avg_b[b++] = (int)currColor.B;
                }
            Color sourceColor = sourceImage.GetPixel(x, y);
            avgR = Sort(avg_r);
            avgG = Sort(avg_g);
            avgB = Sort(avg_b);
            Color resultColor = Color.FromArgb(Clamp(avgR, 0, 255), Clamp(avgG, 0, 255), Clamp(avgB, 0, 255));

            return resultColor;
        }

        protected int Sort(int[] arr)
        {
            for (int i = 0; i < size - 1; i++)
                for (int j = i + 1; j < size; j++)
                {
                    if (arr[i] > arr[j])
                    {
                        int tmp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = tmp;
                    }
                }

            return arr[size-1];
        }
    }
}
