using Microsoft.Win32;
using P2PChatProj.Models;
using P2PChatProj.Services;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
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

        public DelegateCommand SendImageButtonCommand { get; private set; }

        public DelegateCommand BuzzButtonCommand { get; private set; }

        // Lists of messages
        public ObservableCollection<ChatMessage> RemoteMessages { get; set; }

        public ObservableCollection<ChatMessage> UserMessages { get; set; }

        #endregion

        public ChatViewModel(OnlineViewModel onlineViewModel, User user, Connection connection)
        {
            User = user;
            Connection = connection;
            Connection.AddRemoteMessage = AddReceivedMessage;
            Connection.PrepareChat = CloseHistoryMode;
            OnlineViewModel = onlineViewModel;

            UserMessages = new ObservableCollection<ChatMessage>();
            RemoteMessages = new ObservableCollection<ChatMessage>();

            SendTextCommand = new DelegateCommand(SendTextMessage, CanUseChatButtons);
            SendImageButtonCommand = new DelegateCommand(SendImageMessage, CanUseChatButtons);
            BuzzButtonCommand = new DelegateCommand(SendBuzz, CanUseChatButtons);
        }

        private void CloseHistoryMode()
        {
            HistoryMode = false;
            UserMessages.Clear();
            RemoteMessages.Clear();
        }

        public void AddMessage(ChatMessage visibleMessage, ChatMessage hiddenMessage, bool remote = false)
        {
            if (remote)
            {
                Console.WriteLine($"STATUS: Adding a remote text message");
                RemoteMessages.Add(visibleMessage);
                UserMessages.Add(hiddenMessage);
            }
            else
            {
                Console.WriteLine($"STATUS: Adding a user text message");
                UserMessages.Add(visibleMessage);
                RemoteMessages.Add(hiddenMessage);
            }
        }

        private async void SendTextMessage()
        {
            NetworkData networkMessage = new NetworkData(User, NetworkDataType.Message, InputMessage);
            InputMessage = "";
            TextChatMessage visibleMessage = new TextChatMessage(networkMessage.User.UserName, networkMessage.Date,
                                                                     networkMessage.Data);
            TextChatMessage hiddenMessage = new TextChatMessage(networkMessage.User.UserName, networkMessage.Date,
                                                                networkMessage.Data, Visibility.Hidden);

            Application.Current.Dispatcher.Invoke(() =>
            {
                AddMessage(visibleMessage, hiddenMessage);
            });

            Console.WriteLine($"STATUS: Sending a message > {networkMessage.Data}");
            await Connection.SendNetworkData(networkMessage);
        }

        private async void SendImageMessage()
        {
            Console.WriteLine("STATUS: Sending a picture");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg)|*.jpg;*.jpeg;";
            openFileDialog.RestoreDirectory = true;

            openFileDialog.ShowDialog();
            string filePath = openFileDialog.FileName;
         
            if (!String.IsNullOrEmpty(filePath))
            {
                Bitmap bitmap = new Bitmap(filePath);
                string fileName = Path.GetFileName(filePath);
                string imagePath = await FileService.SaveImage(bitmap, fileName);

                string bitmapString = ImageService.BitmapToString(bitmap);
                NetworkData networkImage = new NetworkData(User, NetworkDataType.Image, fileName + " " + bitmapString);
                ImageChatMessage visibleImage = new ImageChatMessage(networkImage.User.UserName, networkImage.Date,
                                                                       imagePath);
                ImageChatMessage hiddenImage = new ImageChatMessage(networkImage.User.UserName, networkImage.Date,
                                                                      imagePath, Visibility.Hidden);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AddMessage(visibleImage, hiddenImage);
                });

                await Connection.SendNetworkData(networkImage);
            }
        }

        private async void SendBuzz()
        {
            Console.WriteLine("STATUS: Sending a buzz");
            NetworkData buzz = new NetworkData(User, NetworkDataType.Response, "", ResponseType.Buzz);
            await Connection.SendNetworkData(buzz);
        }

        private async void AddReceivedMessage(NetworkData networkMessage)
        {
            if (networkMessage.DataType == NetworkDataType.Message)
            {
                TextChatMessage visibleMessage = new TextChatMessage(networkMessage.User.UserName, networkMessage.Date,
                                                                    networkMessage.Data);
                TextChatMessage hiddenMessage = new TextChatMessage(networkMessage.User.UserName, networkMessage.Date,
                                                                    networkMessage.Data, Visibility.Hidden);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    AddMessage(visibleMessage, hiddenMessage, true);
                });
            }
            else if (networkMessage.DataType == NetworkDataType.Image)
            {
                string fileName = networkMessage.Data.Split(new char[] { ' ' }, 2)[0];
                string imageData = networkMessage.Data.Split(new char[] { ' ' }, 2)[1];
                Console.WriteLine(fileName);
                Console.WriteLine(imageData);
                Bitmap receivedBitmap = ImageService.StringToBitmap(imageData);

                string imagePath = await FileService.SaveImage(receivedBitmap, fileName, false);

                ImageChatMessage visibleImage = new ImageChatMessage(networkMessage.User.UserName, networkMessage.Date,
                                                                     imagePath);
                ImageChatMessage hiddenImage = new ImageChatMessage(networkMessage.User.UserName, networkMessage.Date,
                                                                    imagePath, Visibility.Hidden);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AddMessage(visibleImage, hiddenImage, true);
                });
            }
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

            UserMessages = new ObservableCollection<ChatMessage>(chatData.UserMessages);
            RaisePropertyChanged("UserMessages");
            RemoteMessages = new ObservableCollection<ChatMessage>(chatData.RemoteMessages);
            RaisePropertyChanged("RemoteMessages");
        }

        private void SaveToChatHistory()
        {
            Console.WriteLine("STATUS: Saving chat to history");
            ChatData chatData = new ChatData(Connection.LocalUser, Connection.RemoteUser, 
                                                       UserMessages.ToList(), RemoteMessages.ToList(),
                                                       DateTime.Now.ToString());
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
