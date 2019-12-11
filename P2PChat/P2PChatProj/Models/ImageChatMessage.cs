using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace P2PChatProj.Models
{
    public class ImageChatMessage : ChatMessage
    {
        public ImageChatMessage(string name, string date, BitmapImage imageSource, Visibility visibility = Visibility.Visible)
        {
            Name = name;
            Date = date;
            ImageSource = imageSource;
            Visibility = visibility;
            TextMessage = "";
            TextVisibility = Visibility.Collapsed;
            ImageVisibility = Visibility.Visible;
        }
    }
}
