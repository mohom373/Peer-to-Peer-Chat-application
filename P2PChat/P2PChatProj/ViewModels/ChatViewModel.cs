using P2PChatProj.Models;
using P2PChatProj.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private User user;

        public ChatViewModel(OnlineViewModel onlineViewModel, User user)
        {
            this.user = user;
            OnlineViewModel = onlineViewModel;

            SendTextButtonCommand = new SendTextCommand(this);
            SendPictureButtonCommand = new SendPictureCommand(this);
            BuzzButtonCommand = new BuzzCommand(this);

            FillMessages();
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

        public ObservableCollection<ChatMessage> RemoteMessages { get; set; } = new ObservableCollection<ChatMessage>();

        public ObservableCollection<ChatMessage> UserMessages { get; set; } = new ObservableCollection<ChatMessage>();

        public OnlineViewModel OnlineViewModel { get; set; }

        #endregion

        public async Task AppClosing()
        {
            Console.WriteLine("ChatView closing");
        }

        public void SetupChat()
        {
            ChatRunning = true;
            Console.WriteLine("Setting up chat...");
        }

        public void CloseChat()
        {
            ChatRunning = false;
            Console.WriteLine("Closing chat...");
        }

        public void FillMessages()
        {
            string message = "Hejhej!";
            RemoteMessages.Add(new ChatMessage(message, Visibility.Visible));
            UserMessages.Add(new ChatMessage(message, Visibility.Hidden));

            message = "Tjena!";
            UserMessages.Add(new ChatMessage(message, Visibility.Visible));
            RemoteMessages.Add(new ChatMessage(message, Visibility.Hidden));

            message = "Fin app va?";
            UserMessages.Add(new ChatMessage(message, Visibility.Visible));
            RemoteMessages.Add(new ChatMessage(message, Visibility.Hidden));

            message = "Mycket fin må jag säga";
            RemoteMessages.Add(new ChatMessage(message, Visibility.Visible));
            UserMessages.Add(new ChatMessage(message, Visibility.Hidden));

            message = "Utomordentligt finfin skulle jag vilja påstå";
            UserMessages.Add(new ChatMessage(message, Visibility.Visible));
            RemoteMessages.Add(new ChatMessage(message, Visibility.Hidden));
        }
    }
}
