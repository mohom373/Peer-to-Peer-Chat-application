using P2PChatProj.Models;
using P2PChatProj.Services;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private User user;
        private ChatState chatState = ChatState.Offline;

        public ChatViewModel(OnlineViewModel onlineViewModel, User user, IPAddress localIp)
        {
            this.user = user;
            LocalIp = localIp;
            OnlineViewModel = onlineViewModel;

            SendTextButtonCommand = new SendTextCommand(this);
            SendPictureButtonCommand = new SendPictureCommand(this);
            BuzzButtonCommand = new BuzzCommand(this);

            RemoteMessages = new ObservableCollection<ChatMessage>();
            UserMessages = new ObservableCollection<ChatMessage>();
        }

        #region Properties

        public User User 
        { 
            get { return user; } 
            set { user = value; }
        }

        public User RemoteUser { get; set; }

        public IPAddress LocalIp { get; set; }

        public string InputMessage { get; set; } = "";

        public ICommand SendTextButtonCommand { get; private set; }

        public ICommand SendPictureButtonCommand { get; private set; }
        
        public ICommand BuzzButtonCommand { get; private set; }

        public ChatState ChatState 
        { 
            get 
            { 
                return chatState; 
            } 
            set 
            {
                chatState = value;
                UpdateChatState();
            } 
        }

        public ObservableCollection<ChatMessage> RemoteMessages { get; set; }

        public ObservableCollection<ChatMessage> UserMessages { get; set; }

        public OnlineViewModel OnlineViewModel { get; set; }

        #endregion

        public async Task AppClosing()
        {
            Console.WriteLine("ChatView closing");
        }

        public void SetupChat(User remoteUser)
        {
            RemoteUser = remoteUser;
            ChatState = ChatState.Connecting;
        }

        public void CloseChat()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChatState = ChatState.Offline;
            });
            Console.WriteLine("Closing chat...");
        }

        public void FillMessages()
        {
            string message = "Hejhej!";
            Console.WriteLine(message);
            ChatMessage cm = new ChatMessage(message, Visibility.Visible, RemoteUser.UserName);
            Console.WriteLine(message);
            RemoteMessages.Add(cm);
            Console.WriteLine(message);
            UserMessages.Add(new ChatMessage(message, Visibility.Hidden, User.UserName));
            Console.WriteLine(message);

            message = "Tjena!";
            Console.WriteLine(message);
            UserMessages.Add(new ChatMessage(message, Visibility.Visible, User.UserName));
            RemoteMessages.Add(new ChatMessage(message, Visibility.Hidden, RemoteUser.UserName));

            message = "Fin app va?";
            Console.WriteLine(message);
            UserMessages.Add(new ChatMessage(message, Visibility.Visible, User.UserName));
            RemoteMessages.Add(new ChatMessage(message, Visibility.Hidden, RemoteUser.UserName));

            message = "Mycket fin må jag säga";
            Console.WriteLine(message);
            RemoteMessages.Add(new ChatMessage(message, Visibility.Visible, RemoteUser.UserName));
            UserMessages.Add(new ChatMessage(message, Visibility.Hidden, User.UserName));

            message = "Utomordentligt finfin skulle jag vilja påstå";
            Console.WriteLine(message);
            UserMessages.Add(new ChatMessage(message, Visibility.Visible, User.UserName));
            RemoteMessages.Add(new ChatMessage(message, Visibility.Hidden, RemoteUser.UserName));
        }

        private void UpdateChatState()
        {
            switch (ChatState)
            {
                case ChatState.Offline:
                    RemoteMessages.Clear();
                    UserMessages.Clear();
                    ChatService.CloseSockets();
                    break;

                case ChatState.Connecting:
                    StartChatConnection();
                    break;

                case ChatState.Online:
                    FillMessages();
                    FillMessages();
                    break;

                case ChatState.History:
                    break;

                default:
                    break;
            }
        }

        private async void StartChatConnection()
        {
            await Task.Run(() => ChatService.SetupSockets(LocalIp, (User.PortNumber + 1)));

            Progress<ChatMessage> messageReport = new Progress<ChatMessage>();
            messageReport.ProgressChanged += AddMessage;

            Task<bool> receiverConnecting = ChatService.ConnectToSender();

            bool senderConnected = await ChatService.ConnectToReceiver(RemoteUser);

            if (senderConnected)
            {
                Console.WriteLine("Sender Connected");
            }
            else
            {
                Console.WriteLine("Sender Connection FAILED");
            }

            bool receiverConnected = await receiverConnecting;

            if (receiverConnected)
            {
                Console.WriteLine("Receiver Connected");
            }
            else
            {
                Console.WriteLine("Receiver Connection FAILED");
            }

            if (senderConnected && receiverConnected)
            {
                ChatState = ChatState.Online;
            }
            else
            {
                ChatState = ChatState.Offline;
            }
        }

        private void AddMessage(object sender, ChatMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
