using P2PChatProj.Model;
using P2PChatProj.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    public class UserConnectViewModel
    {
        public UserConnectViewModel()
        {
            user = new User();
            //ConnectCommand = new UserConnectCommand();

        }

        private User user;

        public User User
        {
            get { return user; }

            set { user = value; }
        }

        public ICommand ConnectCommand
        {
            get;
            private set;
        }
    }
}
