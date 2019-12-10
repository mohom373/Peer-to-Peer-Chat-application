using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BitmapJson.Models
{
    public class ImageData : ChatData
    {
        public ImageData(string name, BitmapImage image, Visibility visibility = Visibility.Visible)
        {
            Name = name;
            ImageSource = image;
            Visibility = visibility;
            TextMessage = "";

            TextVisibility = Visibility.Collapsed;
            ImageVisibility = Visibility.Visible;
        }
    }
}
