using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace P2PChatProj.Models
{
    public abstract class ChatMessage
    {
        public string TextMessage { get; set; } = "";

        //public BitmapImage PictureSource { get; set; } = new BitmapImage();

        public string Name { get; set; }

        public string Date { get; set; }

        public Visibility Visibility { get; set; }

        //public Visibility TextVisibility { get; set; }

        //public Visibility PictureVisibility { get; set; }
    }
}
