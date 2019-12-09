using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChatProj.Models
{
    public class ChatMessage
    {
        public string Message { get; set; } = "";

        public string Name { get; set; } = "";

        public string Date { get; set; }

        public Visibility Visibility { get; set; }

        public ChatMessage(string message, string name, string date, Visibility visibility = Visibility.Visible)
        {
            Message = message;
            Name = name;
            Date = date;
            Visibility = visibility;
        }

        

        

        
    }
}
