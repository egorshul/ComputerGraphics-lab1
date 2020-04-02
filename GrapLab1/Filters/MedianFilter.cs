using System.Drawing;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace GrapLab1
{
    class MedianFilter : Filters
    {

        const int size = 3;

        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourceColor = sourseImage.GetPixel(x, y);
            int index_median = size * size / 2;

            int[] local_R = new int[9];
            int[] local_G = new int[9];
            int[] local_B = new int[9];

            int k = 0;
           for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++, k++)
                {
                    Color currColor = sourseImage.GetPixel(Clamp(x + i, 0, sourseImage.Width - 1), Clamp(y + j, 0, sourseImage.Height - 1));
                    local_R[k] = (int)currColor.R; ;
                    local_G[k] = (int)currColor.G; ;
                    local_B[k] = (int)currColor.B; ;
                }
            Array.Sort(local_R);
            Array.Sort(local_G);
            Array.Sort(local_B);


            Color resultColor = Color.FromArgb(local_R[index_median], local_G[index_median], local_B[index_median]);
            return resultColor;
        }
    }
}
