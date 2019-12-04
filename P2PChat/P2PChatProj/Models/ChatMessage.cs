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
        public ChatMessage(string message,/*bool received*/ Visibility visibility = Visibility.Visible,  string name = "")
        {
            Message = message;
            Visibility = visibility;
            Name = name;
        }

        public string Message { get; set; } = "";

        public string Name { get; set; } = "";

        public Visibility Visibility { get; set; }

        public DateTime Date { get; set; }
    }
}
