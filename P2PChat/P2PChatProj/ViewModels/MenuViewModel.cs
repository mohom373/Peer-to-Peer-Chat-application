using P2PChatProj.Models;
using P2PChatProj.Services;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {

        // Private backup variables
        private string activeChatInfo = "No Active Chat";
        private MenuState activeChatState = MenuState.Listening;
        private Visibility exitVisibility = Visibility.Collapsed;
        private Visibility acceptVisibility = Visibility.Collapsed;
        private Visibility declineVisibility = Visibility.Collapsed;

        // Events

        public MenuViewModel(OnlineViewModel onlineViewModel, User user, IPAddress localIp)
        { 
            User = user;
            IpAddress = localIp;
            OnlineViewModel = onlineViewModel;
            ChatHistoryList = new ObservableCollection<Request>();

            RequestButtonCommand = new SendRequestCommand(this);
            ExitButtonCommand = new ExitActiveChatCommand(this);
            AcceptButtonCommand = new AcceptChatCommand(this);
            DeclineButtonCommand = new DeclineChatCommand(this);

            Task.Run(() => StartListeningForRequest(IpAddress, User.PortNumber));
        }

        #region Request Listening

        public async Task StartListeningForRequest(IPAddress localIp, int localPort)
        {
            bool listening = true;
            
            while (listening)
            {
                RemoteUser = await RequestService.ListenForRequestAsync(localIp, localPort);

                if (RemoteUser != null)
                {
                    ActiveChatState = MenuState.Responding;
                    listening = false;
                }
                else if (RemoteUser == null && ActiveChatState != MenuState.Listening)
                {
                    listening = false;
                }
            }
        }

        public void CancelListeningForRequest()
        {
            RequestService.CancelRequestListener();
        }

        #endregion

        #region Response Listening

        public async Task StartListeningForResponse()
        {
            RequestResponse response = await RequestService.ListenForResponseAsync();

            if (response != null)
            {
                switch(response.ResponseType)
                {
                    case ResponseType.Accept:
                        RemoteUser = new Request(response.IpAddress, response.PortNumber, response.UserName);
                        ActiveChatState = MenuState.Chatting;
                        Task.Run(() => StartListeningForResponse());
                        MessageBox.Show("User has accepted your chat request");
                        OnlineViewModel.StartChat(RemoteUser);
                        break;

                    case ResponseType.Decline:
                        ActiveChatState = MenuState.Listening;
                        MessageBox.Show($"{response.UserName} declined your chat request");
                        break;

                    case ResponseType.Exit:
                        MenuState tempState = ActiveChatState;
                        ActiveChatState = MenuState.Listening;
                        if (tempState == MenuState.Chatting)
                        {
                            MessageBox.Show($"{response.UserName} left the chat");
                            OnlineViewModel.ExitChat();
                        }
                        else
                        {
                            MessageBox.Show($"{response.UserName} canceled their chat request");
                        }
                        break;

                    case ResponseType.Disconnect:
                        ActiveChatState = MenuState.Listening;
                        MessageBox.Show($"{response.UserName} disconnected from application");
                        break;

                    default:
                        break;
                }
            }

        }

        public void CancelListeningForResponse()
        {
            RequestService.CancelRequestListener();
        }

        #endregion

        #region Request Sending

        public async Task SendRequest()
        {
            ActiveChatState = MenuState.Connecting;
            bool requestSent = await RequestService.SendRequestAsync(new Request(InputIp, InputPort, User.UserName));

            if (requestSent)
            {
                ActiveChatState = MenuState.Waiting;
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, $"Could not connect to user!",
                    "Connection failed", MessageBoxButton.OK);
                ActiveChatState = MenuState.Listening;
            }
        }

        #endregion

        #region Response Sending

        public async Task ExitActiveChat()
        {
            RequestResponse exitResponse = new RequestResponse(ResponseType.Exit,
                    User.UserName, IpAddress.ToString(), User.PortNumber);
            bool responseSent = await RequestService.SendResponse(exitResponse);
            if (ActiveChatState == MenuState.Chatting)
            {
                OnlineViewModel.ExitChat();
            }
            ActiveChatState = MenuState.Listening;
        }

        public async Task DeclineChatRequest()
        {
            RequestResponse declineResponse = new RequestResponse(ResponseType.Decline,
                    User.UserName, IpAddress.ToString(), User.PortNumber);
            bool responseSent = await RequestService.SendResponse(declineResponse);
            ActiveChatState = MenuState.Listening;
        }

        public async Task AcceptChatRequest()
        {
            RequestResponse acceptResponse = new RequestResponse(ResponseType.Accept,
                    User.UserName, IpAddress.ToString(), User.PortNumber);
            bool responseSent = await RequestService.SendResponse(acceptResponse);
            ActiveChatState = MenuState.Chatting;
        }

        #endregion

        #region Properties
        // Commands
        public ICommand RequestButtonCommand { get; private set; }

        public ICommand ExitButtonCommand { get; private set; }

        public ICommand AcceptButtonCommand { get; private set; }

        public ICommand DeclineButtonCommand { get; private set; }


        // Input properties
        public string InputIp { get; set; }

        public int InputPort { get; set; }

        // Info
        public IPAddress IpAddress { get; set; }

        public User User { get; set; }

        public string ActiveChatInfo
        {
            get
            {
                return activeChatInfo;
            }
            set
            {
                activeChatInfo = value;
                RaisePropertyChanged("ActiveChatInfo");
            }
        }

        public MenuState ActiveChatState
        {
            get
            {
                return activeChatState;
            }
            set
            {
                activeChatState = value;
                UpdateActiveChatState();
            }
        }

        public Request RemoteUser { get; set; }
        
        // Validation booleans
        public bool Connecting { get; set; } = false;

        public bool Waiting { get; set; } = false;

        public bool Chatting { get; set; } = false;

        // Visibility values for controls
        public Visibility ExitVisibility
        {
            get { return exitVisibility; }
            set
            {
                exitVisibility = value;
                RaisePropertyChanged("ExitVisibility");
            }
        }

        public Visibility AcceptVisibility
        {
            get { return acceptVisibility; }
            set
            {
                acceptVisibility = value;
                RaisePropertyChanged("AcceptVisibility");
            }
        }

        public Visibility DeclineVisibility
        {
            get { return declineVisibility; }
            set
            {
                declineVisibility = value;
                RaisePropertyChanged("DeclineVisibility");
            }
        }

        public OnlineViewModel OnlineViewModel { get; set; }

        public ObservableCollection<Request> ChatHistoryList { get; set; }


        #endregion

        public async Task AppClosing()
        {
            if(!(ActiveChatState == MenuState.Listening || ActiveChatState == MenuState.Connecting))
            {
                RequestResponse disconnectResponse = new RequestResponse(ResponseType.Disconnect, 
                    User.UserName, IpAddress.ToString(), User.PortNumber);
                bool responseSent = await RequestService.SendResponse(disconnectResponse);
            } 

            RequestService.CancelRequestListener();
        }


        private void UpdateActiveChatState()
        {
            switch (ActiveChatState)
            {
                case MenuState.Listening:
                    CancelListeningForResponse();
                    Task.Run(() => StartListeningForRequest(IpAddress, User.PortNumber));
                    ExitVisibility = Visibility.Collapsed;
                    AcceptVisibility = Visibility.Collapsed;
                    DeclineVisibility = Visibility.Collapsed;
                    ActiveChatInfo = "No Active Chat";
                    break;

                case MenuState.Connecting:
                    CancelListeningForRequest();
                    ActiveChatInfo = "Connecting...";
                    break;

                case MenuState.Waiting:
                    Task.Run(() => StartListeningForResponse());
                    ExitVisibility = Visibility.Visible;
                    ActiveChatInfo = "Waiting...";
                    break;

                case MenuState.Responding:
                    Task.Run(() => StartListeningForResponse());
                    AcceptVisibility = Visibility.Visible;
                    DeclineVisibility = Visibility.Visible;
                    ActiveChatInfo = RemoteUser.UserName;
                    break;

                case MenuState.Chatting:
                    OnlineViewModel.StartChat(RemoteUser);
                    AcceptVisibility = Visibility.Collapsed;
                    DeclineVisibility = Visibility.Collapsed;
                    ExitVisibility = Visibility.Visible;
                    ActiveChatInfo = RemoteUser.UserName;
                    break;

                default:
                    break;
            }
        }
    }
}
