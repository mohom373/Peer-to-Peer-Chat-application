using P2PChatProj.Models;
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
    public class ChatViewModel
    {
        private User user;

        public ChatViewModel(OnlineViewModel onlineViewModel, User user)
        {
            this.user = user;
            OnlineViewModel = onlineViewModel;

            SendTextButtonCommand = new SendTextCommand(this);
            SendPictureButtonCommand = new SendPictureCommand(this);
            BuzzButtonCommand = new BuzzCommand(this);
        }

        #region Properties

        public User User 
        { 
            get { return user; } 
            set { user = value; }
        }

        public string InputMessage { get; set; } = "";

        public ICommand SendTextButtonCommand { get; private set; }

        public ICommand SendPictureButtonCommand { get; private set; }
        
        public ICommand BuzzButtonCommand { get; private set; }

        public bool ChatRunning { get; set; } = false;

        public ObservableCollection<string> remoteMessages { get; set; }
        
        public ObservableCollection<string> userMessages { get; set; }

        public OnlineViewModel OnlineViewModel { get; set; }

        #endregion

        public async Task AppClosing()
        {
            Console.WriteLine("ChatView closing");
        }

        internal void SetupChat()
        {
            ChatRunning = true;
            Console.WriteLine("Setting up chat...");
        }

        internal void CloseChat()
        {
            ChatRunning = false;
            Console.WriteLine("Closing chat...");
        }
    }
}
