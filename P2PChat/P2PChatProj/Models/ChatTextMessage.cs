using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChatProj.Models
{
    public class ChatTextMessage : ChatMessage
    { 
        public ChatTextMessage(string message, string name, string date, Visibility visibility = Visibility.Visible)
        {
            TextMessage = message;
            Name = name;
            Date = date;
            Visibility = visibility;
            TextVisibility = Visibility.Visible;
            PictureVisibility = Visibility.Collapsed;
        }
    }
}
