using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    /// <summary>
    /// Controls the menu and chat views and handles communication between them
    /// </summary>
    public class OnlineViewModel
    {
        #region Properties

        public User User { get; private set; }

        public Connection Connection { get; private set; }

        // Parent viewmodel
        public MainWindowViewModel MainWindowViewModel { get; private set; }

        // Child viewmodels
        public MenuViewModel MenuViewModel { get; private set; }

        public ChatViewModel ChatViewModel { get; private set; }

        #endregion

        /// <summary>
        /// OnlineViewModel constructor
        /// </summary>
        /// <param name="mainWindowViewModel">Parent viewmodel</param>
        /// <param name="user">User information</param>
        public OnlineViewModel(MainWindowViewModel mainWindowViewModel, User user)
        {
            User = user;
            MainWindowViewModel = mainWindowViewModel;

            Connection = new Connection(User);
            Connection.ExitChat = ExitChat;

            MenuViewModel = new MenuViewModel(this, User, Connection);
            ChatViewModel = new ChatViewModel(this, User, Connection);
        }

        public void ExitChat()
        {
            ChatViewModel.CloseChat();
        }

        /// <summary>
        /// Event handler for when the app is closing
        /// </summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">Arguments associated with the closing event</param>
        internal void ClosingApp(object sender, CancelEventArgs e)
        {
            Console.WriteLine("STATUS: OnlineViewModel closing");
        }
    }
}
