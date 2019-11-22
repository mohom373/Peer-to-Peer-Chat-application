using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    public class UserOnlineViewModel
    {
        private User user;

        public UserOnlineViewModel(User user)
        {
            this.user = user;
            Menu = new MenuViewModel(user);
            Chat = new ChatViewModel(user);
        }

        public User User 
        { 
            get { return user; }
            set { user = value; } 
        }

        public MenuViewModel Menu { get; set; }

        public ChatViewModel Chat { get; set; }
    }
}
