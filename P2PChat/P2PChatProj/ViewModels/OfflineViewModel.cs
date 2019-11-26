using P2PChatProj.Models;
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
    public class OfflineViewModel
    {
        private User user;
        private MainWindow mainWindow;

        public OfflineViewModel(MainWindow mainWindow)
        {
            user = new User();
            GoOnlineButtonCommand = new GoOnlineCommand(this);

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

        public ICommand GoOnlineButtonCommand
        {
            get;
            private set;
        }

        public void GoOnline()
        {
            OnlineViewModel userOnlineViewModel = new OnlineViewModel(user);
            mainWindow.DataContext = userOnlineViewModel;
        }
    }
}
