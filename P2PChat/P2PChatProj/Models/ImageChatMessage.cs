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
        public string ImagePath { get; set; }

        public ImageChatMessage(string name, string date, string imagePath, Visibility visibility = Visibility.Visible)
        {
            Name = name;
            Date = date;
            ImagePath = imagePath;
            Visibility = visibility;
        }
    }
}
