using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    static class BitmapConverter
    {
        public static double[] ToDoubles(this Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height= bitmap.Height;
            int length = width * height;
            double[] result = new double[width * height];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = 1.0 - (bitmap.GetPixel(i % width, i / width).GetBrightness() * 2.0);
            }
            return result;
        }

        public static Bitmap ToBitmap(this double[] doubles, int width)
        {
            if (width <= 0) throw new ArgumentException();
            int length = doubles.Length;
            int height = (length + width - 1) / width;

            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            for (int i = 0; i < length; i++)
            {
                result.SetPixel(i % width, i / width, ToPixel(doubles[i]));
            }
            return result;
        }

        private static Color ToPixel(double value)
        {
            double boundedValue = Math.Min(Math.Max(value + 2, 0), 4);
            byte pixelState = (byte)(boundedValue * 255.0 / 4.0);
            return Color.FromArgb(255, pixelState, pixelState, pixelState);
        }
    }
}
