using P2PChatProj.Models;
using P2PChatProj.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            OfflineViewModel = new OfflineViewModel(this);
            MainWindow.Closing += OfflineViewModel.AppClosing;
            MainWindow.DataContext = OfflineViewModel;
        }

        public MainWindow MainWindow { get; set; }
        public OfflineViewModel OfflineViewModel { get; set; }
        public OnlineViewModel OnlineViewModel { get; set; }

        public void ChangeToOnlineView(User user)
        {
            OnlineViewModel = new OnlineViewModel(user, this);
            MainWindow.Closing += OnlineViewModel.AppClosing;
            MainWindow.DataContext = OnlineViewModel;
        }
    }
}
