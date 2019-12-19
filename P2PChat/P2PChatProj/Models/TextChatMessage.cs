using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace P2PChatProj.Models
{
    public class TextChatMessage : ChatMessage
    {
        public string TextMessage { get; set; }

        public TextChatMessage(string name, string date, string textMessage, Visibility visibility = Visibility.Visible)
        {
            Name = name;
            Date = date;
            TextMessage = textMessage;
            Visibility = visibility;
        }
    }
}
