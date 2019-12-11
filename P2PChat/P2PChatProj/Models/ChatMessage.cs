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
        public string Name { get; set; }

        public string Date { get; set; }

        public Visibility Visibility { get; set; }

        public string TextMessage { get; set; }

        public BitmapImage ImageSource { get; set; }

        public Visibility TextVisibility { get; set; }

        public Visibility ImageVisibility { get; set; }
    }
}
