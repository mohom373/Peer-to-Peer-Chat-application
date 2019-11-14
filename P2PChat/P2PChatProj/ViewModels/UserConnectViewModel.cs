using P2PChatProj.Model;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.Views;
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
        private MainWindow mainWindow;

        public UserConnectViewModel(MainWindow mainWindow)
        {
            user = new User();
            ConnectCommand = new UserConnectCommand(this);

            this.mainWindow = mainWindow;
            

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
            UserOnlineViewModel userOnlineViewModel = new UserOnlineViewModel(user);
            mainWindow.DataContext = userOnlineViewModel;
        }
    }
}
