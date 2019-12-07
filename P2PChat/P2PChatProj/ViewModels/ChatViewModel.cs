using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    public class ChatViewModel
    {
        public User User { get; set; }

        public OnlineViewModel OnlineViewModel { get; set; }

        public ChatViewModel(OnlineViewModel onlineViewModel, User user)
        {
            User = user;
            OnlineViewModel = onlineViewModel;
        }
        internal void ClosingApp(object sender, CancelEventArgs e)
        {
            Console.WriteLine("STATUS: ChatViewModel closing");
        }
    }
}
