using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BitmapJson.Models
{
    public abstract class ChatData
    {
        public string Name { get; set; }

        public BitmapImage ImageSource { get; set; }

        public string TextMessage { get; set; }

        public Visibility Visibility { get; set; } = Visibility.Visible;

        public Visibility TextVisibility { get; set; }

        public Visibility ImageVisibility { get; set; }
    }
}
