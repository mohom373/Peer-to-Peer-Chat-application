using P2PChatProj.Models;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChatProj.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        // Private backing fields
        private string inputMessage = "";

        #region Properties
        public User User { get; set; }

        public Connection Connection { get; private set; }

        public OnlineViewModel OnlineViewModel { get; set; }

        public string InputMessage
        {
            get
            {
                return inputMessage;
            }
            set
            {
                inputMessage = value;
                RaisePropertyChanged("InputMessage");
            }
        }

        public DelegateCommand SendTextCommand { get; private set; }

        public DelegateCommand SendPictureButtonCommand { get; private set; }

        public DelegateCommand BuzzButtonCommand { get; private set; }

        public ObservableCollection<ChatMessage> RemoteMessages { get; set; }

        public ObservableCollection<ChatMessage> UserMessages { get; set; }

        #endregion

        public ChatViewModel(OnlineViewModel onlineViewModel, User user, Connection connection)
        {
            User = user;
            Connection = connection;
            Connection.AddRemoteMessage = AddMessage;
            OnlineViewModel = onlineViewModel;

            UserMessages = new ObservableCollection<ChatMessage>();
            RemoteMessages = new ObservableCollection<ChatMessage>();

            SendTextCommand = new DelegateCommand(SendMessage, CanUseChatButtons);
            SendPictureButtonCommand = new DelegateCommand(SendPicture, CanUseChatButtons);
            BuzzButtonCommand = new DelegateCommand(SendBuzz, CanUseChatButtons);
        }

        public void AddMessage(NetworkData message, bool remote = false)
        {
            ChatMessage visibleMessage = new ChatMessage(message.Message, message.User.UserName,
                                                         message.Date);
            ChatMessage hiddenMessage = new ChatMessage(message.Message, message.User.UserName,
                                                         message.Date, Visibility.Hidden);
            if (remote)
            {
                Console.WriteLine($"STATUS: Adding a remote message");
                RemoteMessages.Add(visibleMessage);
                UserMessages.Add(hiddenMessage);
            }
            else
            {
                Console.WriteLine($"STATUS: Adding a user message");
                UserMessages.Add(visibleMessage);
                RemoteMessages.Add(hiddenMessage);
            }
        }

        private async void SendMessage()
        {
            NetworkData message = new NetworkData(User, NetworkDataType.Message, InputMessage);
            InputMessage = "";

            Application.Current.Dispatcher.Invoke(() =>
            {
                AddMessage(message);
            });
            Console.WriteLine($"STATUS: Sending a message > {message.Message}");
            await Connection.SendNetworkData(message);
            
        }

        private void SendPicture()
        {
            Console.WriteLine("STATUS: Sending a picture");
        }

        private void SendBuzz()
        {
            Console.WriteLine("STATUS: Sending a buzz");
        }

        private bool CanUseChatButtons()
        {
            return Connection.State == ConnectionState.Chatting;
        }

        internal void CloseChat()
        {
            UserMessages.Clear();
            RemoteMessages.Clear();
        }

        internal void ClosingApp(object sender, CancelEventArgs e)
        {
            Console.WriteLine("STATUS: ChatViewModel closing");
        }
    }
}
