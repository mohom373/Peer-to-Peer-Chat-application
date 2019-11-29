using P2PChatProj.Models;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    public class OfflineViewModel : BaseViewModel
    {
        private User user;

        public OfflineViewModel(MainWindowViewModel mainWindowViewModel)
        {
            user = new User();
            MainWindowViewModel = mainWindowViewModel;
            GoOnlineButtonCommand = new GoOnlineCommand(this);
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public ICommand GoOnlineButtonCommand { get; private set; }
        public MainWindowViewModel MainWindowViewModel { get; set; }

        internal void AppClosing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("OfflineView Closing");
        }

        public void GoOnline()
        {
            MainWindowViewModel.ChangeToOnlineView(User);
        }
    }
}
