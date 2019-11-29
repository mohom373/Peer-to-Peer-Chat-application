using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    public class ChatViewModel
    {
        private User user;

        public ChatViewModel(User user)
        {
            this.user = user;
        }

        public User User 
        { 
            get { return user; } 
            set { user = value; }
        }

        internal static void AppClosing()
        {
            throw new NotImplementedException();
        }
    }
}
