using Microsoft.Win32;
using P2PChatProj.Models;
using P2PChatProj.Services;
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
            Connection.AddRemoteMessage = AddMessage;
            Connection.PrepareChat = CloseHistoryMode;
            OnlineViewModel = onlineViewModel;

            UserMessages = new ObservableCollection<ChatMessage>();
            RemoteMessages = new ObservableCollection<ChatMessage>();

            SendTextCommand = new DelegateCommand(SendMessage, CanUseChatButtons);
            SendImageButtonCommand = new DelegateCommand(SendPicture, CanUseChatButtons);
            BuzzButtonCommand = new DelegateCommand(SendBuzz, CanUseChatButtons);
        }

        private void CloseHistoryMode()
        {
            HistoryMode = false;
            UserMessages.Clear();
            RemoteMessages.Clear();
        }

        public void AddMessage(NetworkData networkData, bool remote = false)
        {
            if (networkData.DataType == NetworkDataType.Message)
            {
                TextChatMessage visibleMessage = new TextChatMessage(networkData.User.UserName, networkData.Date,
                                                                     networkData.Data);
                TextChatMessage hiddenMessage = new TextChatMessage(networkData.User.UserName, networkData.Date,
                                                                    networkData.Data, Visibility.Hidden);

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
            else if (networkData.DataType == NetworkDataType.Image)
            {
                BitmapImage imageSource = ImageService.StringToBitmapImage(networkData.Data);

                ImageChatMessage visibleMessage = new ImageChatMessage(networkData.User.UserName, networkData.Date,
                                                                       imageSource);
                ImageChatMessage hiddenMessage = new ImageChatMessage(networkData.User.UserName, networkData.Date,
                                                                      imageSource, Visibility.Hidden);

                if (remote)
                {
                    Console.WriteLine($"STATUS: Adding a remote image");
                    RemoteMessages.Add(visibleMessage);
                    UserMessages.Add(hiddenMessage);
                }
                else
                {
                    Console.WriteLine($"STATUS: Adding a user image");
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
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg)|*.jpg;*.jpeg;";
            openFileDialog.RestoreDirectory = true;

            string bitmapString = "";
            BitmapImage bitmapImage = new BitmapImage();

            openFileDialog.ShowDialog();
            string fileName = openFileDialog.FileName;

            if (!String.IsNullOrEmpty(fileName))
            {
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(fileName);
                bitmapImage.EndInit();

                bitmapString = ImageService.BitmapImageToString(bitmapImage);
            }

            if (!String.IsNullOrEmpty(bitmapString))
            {
                NetworkData image = new NetworkData(User, NetworkDataType.Image, bitmapString);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    AddMessage(image);
                });

                await Connection.SendNetworkData(image);
            }
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

        public void LoadChatFromHistory(SavedChatData chatData)
        {
            if (HistoryMode)
            {
                UserMessages.Clear();
                RemoteMessages.Clear();
            }
            Console.WriteLine("STATUS: Loading chat from history");
            HistoryMode = true;

            UserMessages = HistoryHandler.HistoryListToObservable(chatData.UserMessages);
            RaisePropertyChanged("UserMessages");
            RemoteMessages = HistoryHandler.HistoryListToObservable(chatData.RemoteMessages);
            RaisePropertyChanged("RemoteMessages");
        }

        private void SaveToChatHistory()
        {
            Console.WriteLine("STATUS: Saving chat to history");
            SavedChatData chatData = new SavedChatData(Connection.LocalUser, Connection.RemoteUser, 
                                                       HistoryHandler.ObservableToHistoryList(UserMessages),
                                                       HistoryHandler.ObservableToHistoryList(RemoteMessages), 
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
