using P2PChatProj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    public class MenuViewModel
    {
        private User user;

        public MenuViewModel(User user)
        {
            this.user = user;
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }
    }
}
