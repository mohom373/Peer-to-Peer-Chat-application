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
        private string inputMessage = "";

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


        private void UpdateChatState()
        {
            Progress<ChatMessage> messageReporter = new Progress<ChatMessage>();
            messageReporter.ProgressChanged += AddReceivedMessage;

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
                    ChatService.ListenForMessages(messageReporter));
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

        private void AddReceivedMessage(object sender, ChatMessage message)
        {
            
        }

        public void SendMessage()
        {
            ChatMessage message = new ChatMessage(InputMessage, Visibility.Visible, User.UserName);
            UserMessages.Add(message);
            message.Visibility = Visibility.Hidden;
            RemoteMessages.Add(message);

            MessageData message = new MessageData(message.Message, message.Date, );

        }
    }
}
