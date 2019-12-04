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
        public ChatMessage(string message, Visibility visibility = Visibility.Visible,  string name = "")
        {
            Message = message;
            Visibility = visibility;
            Name = name;
            Date = DateTime.Now.ToString();
        }

        public string Message { get; set; } = "";

        public string Name { get; set; } = "";

        public Visibility Visibility { get; set; }

        public string Date { get; set; }
    }
}
