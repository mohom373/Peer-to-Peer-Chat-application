using P2PChatProj.Model;
using P2PChatProj.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    public class UserConnectViewModel
    {
        private User user;
        
        public UserConnectViewModel()
        {
            user = new User();
            ConnectCommand = new UserConnectCommand(this);

        }

        /// <summary>
        /// Gets or sets a System.Boolean value indicating whether the User can connect
        /// </summary>
        public bool CanConnect 
        { 
            get
            {
                if (User == null)
                {
                    return false;
                }
                return !String.IsNullOrWhiteSpace(User.UserName) && (User.PortNumber > 1024 && User.PortNumber < 64000); 
            } 
        }


        /// <summary>
        /// Gets the user instance
        /// </summary>
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

        public void SaveChanges()
        {
            Debug.Assert(false, String.Format("{0} was updated. ", User.UserName));
        }
    }
}
