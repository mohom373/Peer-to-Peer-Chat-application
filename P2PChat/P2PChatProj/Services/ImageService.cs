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

        public static string BitmapImageToString(BitmapImage bitmapImage)
        {
            Bitmap bitmap = BitmapImageToBitmap(bitmapImage);

            byte[] bitmapBytes = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

            return Convert.ToBase64String(bitmapBytes);
        }

        public static BitmapImage StringToBitmapImage(string dataString)
        {
            byte[] bitmapBytes = Convert.FromBase64String(dataString);

            Bitmap bitmap = (Bitmap)converter.ConvertFrom(bitmapBytes);

            return BitmapToBitmapImage(bitmap);
        }

        private static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
