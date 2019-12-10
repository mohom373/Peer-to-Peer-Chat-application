using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BitmapJson.Models
{
    public class TextData : ChatData
    {
        public TextData(string name, string message, Visibility visibility = Visibility.Visible)
        {
            Name = name;
            TextMessage = message;
            Visibility = visibility;
            ImageSource = new BitmapImage();

            TextVisibility = Visibility.Visible;
            ImageVisibility = Visibility.Collapsed;
        }
    }
}
