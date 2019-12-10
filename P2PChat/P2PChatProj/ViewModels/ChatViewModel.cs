using Microsoft.Win32;
using P2PChatProj.Models;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace P2PChatProj.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        // Private backing fields
        private string inputMessage = "";

        #region Properties
        public User User { get; set; }

        public Connection Connection { get; private set; }

        public bool HistoryMode { get; set; } = false;

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

        // Commands
        public DelegateCommand SendTextCommand { get; private set; }

        public DelegateCommand SendPictureButtonCommand { get; private set; }

        public DelegateCommand BuzzButtonCommand { get; private set; }

        // Lists of messages
        public ObservableCollection<ChatMessage> RemoteMessages { get; set; }

        public ObservableCollection<ChatMessage> UserMessages { get; set; }

        #endregion

        public ChatViewModel(OnlineViewModel onlineViewModel, User user, Connection connection)
        {
            User = user;
            Connection = connection;
            Connection.AddRemoteMessage = AddMessage;
            Connection.PrepareChat = CloseHistoryMode;
            OnlineViewModel = onlineViewModel;

            UserMessages = new ObservableCollection<ChatMessage>();
            RemoteMessages = new ObservableCollection<ChatMessage>();

            SendTextCommand = new DelegateCommand(SendMessage, CanUseChatButtons);
            SendPictureButtonCommand = new DelegateCommand(SendPicture, CanUseChatButtons);
            BuzzButtonCommand = new DelegateCommand(SendBuzz, CanUseChatButtons);
        }

        private void CloseHistoryMode()
        {
            HistoryMode = false;
            UserMessages.Clear();
            RemoteMessages.Clear();
        }

        public void AddMessage(NetworkData message, bool remote = false)
        {
            if (message.DataType == NetworkDataType.Message)
            {
                ChatTextMessage visibleMessage = new ChatTextMessage(message.Data, message.User.UserName,
                                                             message.Date);
                ChatTextMessage hiddenMessage = new ChatTextMessage(message.Data, message.User.UserName,
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
            else if (message.DataType == NetworkDataType.Picture)
            {
                ChatPictureMessage visibleMessage = new ChatPictureMessage(message.Picture, message.User.UserName,
                                                                           message.Date);
                ChatPictureMessage hiddenMessage = new ChatPictureMessage(message.Picture, message.User.UserName,
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
            else
            {
                Console.WriteLine("ERROR: Cannot add other data than picture and text messages");
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
            Console.WriteLine($"STATUS: Sending a message > {message.Data}");
            await Connection.SendNetworkData(message);
        }

        private async void SendPicture()
        {
            Console.WriteLine("STATUS: Sending a picture");
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "Pictures";
            openFileDialog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            string fileName = "";
            BitmapImage bitmap = new BitmapImage();

            await Task.Run(() =>
            {
                openFileDialog.ShowDialog(Application.Current.MainWindow);
                fileName = openFileDialog.FileName;
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fileName);
                bitmap.EndInit();
            });

            NetworkData picture = new NetworkData(User, NetworkDataType.Picture, "", 
                ResponseType.None, DateTime.Now.ToString(), bitmap);

            Application.Current.Dispatcher.Invoke(() =>
            {
                AddMessage(picture);
            });
        }

        private async void SendBuzz()
        {
            Console.WriteLine("STATUS: Sending a buzz");
            NetworkData buzz = new NetworkData(User, NetworkDataType.Response, "", ResponseType.Buzz);
            await Connection.SendNetworkData(buzz);
        }

        private bool CanUseChatButtons()
        {
            return Connection.State == ConnectionState.Chatting;
        }

        internal void CloseChat()
        {
            SaveToChatHistory();
            Console.WriteLine("STATUS: Closing active chat");
            UserMessages.Clear();
            RemoteMessages.Clear();
        }

        public void LoadChatFromHistory(ChatData chatData)
        {
            if (HistoryMode)
            {
                UserMessages.Clear();
                RemoteMessages.Clear();
            }
            Console.WriteLine("STATUS: Loading chat from history");
            HistoryMode = true;
            foreach (ChatTextMessage message in chatData.UserMessages)
            {
                UserMessages.Add(message);
            }
            foreach (ChatTextMessage message in chatData.RemoteMessages)
            {
                RemoteMessages.Add(message);
            }
        }

        private void SaveToChatHistory()
        {
            Console.WriteLine("STATUS: Saving chat to history");
            ChatData chatData = new ChatData(Connection.LocalUser, Connection.RemoteUser, UserMessages.ToList(), 
                                             RemoteMessages.ToList(), DateTime.Now.ToString());
            OnlineViewModel.AddToHistory(chatData);
        }

        internal void ClosingApp(object sender, CancelEventArgs e)
        {
            if (Connection.State == ConnectionState.Chatting)
            {
                CloseChat();
            }
            Console.WriteLine("STATUS: ChatViewModel closing");
        }
    }
}
