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

        public string Data { get; set; }

        public bool Image { get; set; }

        public SavedChatMessage(string name, string date, Visibility visibility, string data, bool image = false)
        {
            Name = name;
            Date = date;
            Visibility = visibility;
            Data = data;
            Image = image;
        }
    }

}
