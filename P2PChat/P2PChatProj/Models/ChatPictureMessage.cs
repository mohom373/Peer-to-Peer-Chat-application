using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace P2PChatProj.Models
{
    public class ChatPictureMessage : ChatMessage
    {
        public ChatPictureMessage(BitmapImage source, string name, string date, Visibility visibility = Visibility.Visible)
        {
            PictureSource = source;
            Name = name;
            Date = date;
            Visibility = visibility;
            TextVisibility = Visibility.Collapsed;
            PictureVisibility = Visibility.Visible;
        }
    }
}
