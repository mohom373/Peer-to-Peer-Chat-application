using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace P2PChatProj.Services
{
    public static class ImageService
    {
        private static ImageConverter converter = new ImageConverter();

        public static string BitmapToString(Bitmap bitmap)
        {
            byte[] bitmapBytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
            return Convert.ToBase64String(bitmapBytes);
        }

        public static Bitmap StringToBitmap(string imageString)
        {
            byte[] bitmapBytes = Convert.FromBase64String(imageString);
            return (Bitmap)converter.ConvertFrom(bitmapBytes);
        }

    }
}
