using P2PChatProj.Models;
using P2PChatProj.Services;
using P2PChatProj.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    /// <summary>
    /// viewmodel on the top of the hierarchy. Handles switching between
    /// the offline and online state of the app.
    /// </summary>
    public class MainWindowViewModel
    {
        #region Properties
        
        public MainWindow MainWindow { get; set; }
        
        // Child viewmodels
        public OfflineViewModel OfflineViewModel { get; set; }
        
        public OnlineViewModel OnlineViewModel { get; set; }
        
        #endregion

        /// <summary>
        /// MainWindowViewModel constructor. Binds the MainWindow's closing event
        /// </summary>
        /// <param name="mainWindow">MainWindow of the app</param>
        public MainWindowViewModel(MainWindow mainWindow)
        {
            Task.Run(() => FileService.DirectorySetupCheck());
            MainWindow = mainWindow;
            OfflineViewModel = new OfflineViewModel(this);
            MainWindow.Closing += OfflineViewModel.ClosingApp;
            MainWindow.DataContext = OfflineViewModel;
        }

        /// <summary>
        /// Switches app state to the online view
        /// </summary>
        /// <param name="user">User object containing information chosen by the user</param>
        public void ChangeToOnlineView(User user)
        {
            Console.WriteLine("STATUS: Going online");
            OnlineViewModel = new OnlineViewModel(this, user);
            MainWindow.Closing += OnlineViewModel.MenuViewModel.ClosingApp;
            MainWindow.Closing += OnlineViewModel.ChatViewModel.ClosingApp;
            MainWindow.Closing += OnlineViewModel.ClosingApp;
            MainWindow.DataContext = OnlineViewModel;
        }
    }
}
