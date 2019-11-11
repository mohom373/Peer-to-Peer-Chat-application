using P2PChatProj.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModel
{
    public class UserConnectViewModel
    {
        public UserConnectViewModel()
        {
            user = new User();
        }

        private User user;

        public User User
        {
            get { return user; }

            set { user = value; }
        }
    }
}
