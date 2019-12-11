using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChatProj.Models
{
    public class SavedChatMessage
    {
        public string Name { get; set; }

        public string Date { get; set; }

        public Visibility Visibility { get; set; }

        public string TextMessage { get; set; }

        public bool Image { get; set; }

        public string ImageData { get; set; }

        public SavedChatMessage(string name, string date, Visibility visibility, string textMessage, bool image = false, string imageData = "")
        {
            Name = name;
            Date = date;
            Visibility = visibility;
            TextMessage = textMessage;
            Image = image;
            ImageData = imageData;
        }
    }
}
