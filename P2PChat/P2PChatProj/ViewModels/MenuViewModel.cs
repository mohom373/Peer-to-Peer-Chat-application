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
    public class MenuViewModel : INotifyPropertyChanged
    {
        public enum State
        {
            Listening,
            Connecting,
            Waiting,
            Responding,
            Chatting,
        }

        // Private backup variables
        private string activeChatInfo;
        private State activeChatState;
        private Visibility exitVisibility = Visibility.Collapsed;
        private Visibility acceptVisibility = Visibility.Collapsed;
        private Visibility declineVisibility = Visibility.Collapsed;
            
        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        public MenuViewModel(User user, IPAddress localIp)
        { 
            User = user;
            IpAddress = localIp;
            ChatHistoryList = new ObservableCollection<Request>();

            RequestButtonCommand = new SendRequestCommand(this);
            ExitButtonCommand = new ExitActiveChatCommand(this);
            AcceptButtonCommand = new AcceptChatCommand(this);
            DeclineButtonCommand = new DeclineChatCommand(this);

            ActiveChatState = State.Listening;
        }

        #region Request Listening

        public async Task StartListeningForRequest(IPAddress localIp, int localPort)
        {
            bool listening = true;
            
            while (listening)
            {
                ReceivedRequest = await RequestService.ListenForRequestAsync(localIp, localPort);

                if (ReceivedRequest != null)
                {
                    ActiveChatState = State.Responding;
                    listening = false;
                }
                else if (ReceivedRequest == null && ActiveChatState != State.Listening)
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

        }

        public void CancelListeningForResponse()
        {

        }
        #endregion

        #region Request Sending

        public async Task SendRequest()
        {
            ActiveChatState = State.Connecting;
            //Progress<State> updateState = new Progress<State>();
            //updateState.ProgressChanged += SenderReport;
            bool requestSent = await RequestService.SendRequestAsync(new Request(InputIp, InputPort, User.UserName));//, updateState);

            if (requestSent)
            {
                ActiveChatState = State.Waiting;
            }
            else
            {
                MessageBox.Show($"Could not connect to user!",
                    "Connection failed", MessageBoxButton.OK);
                ActiveChatState = State.Listening;
            }
        }

        #endregion

        #region Response Sending

        public async void ExitActiveChat()
        {
            RequestResponse exitResponse = new RequestResponse(Response.Exit);
            bool responseSent = await RequestService.SendResponse(exitResponse);
            ActiveChatState = State.Listening;
        }

        public async void DeclineChatRequest()
        {
            RequestResponse declineResponse = new RequestResponse(Response.Decline); 
            bool responseSent = await RequestService.SendResponse(declineResponse);
            ActiveChatState = State.Listening;
        }

        public void AcceptChatRequest()
        {
            // setup chat view

            // Meddela andra part om accept

        }

        #endregion

        #region Properties
        // Commands
        public ICommand RequestButtonCommand
        {
            get;
            private set;
        }

        public ICommand ExitButtonCommand
        {
            get;
            private set;
        }

        public ICommand AcceptButtonCommand
        {
            get;
            private set;
        }

        public ICommand DeclineButtonCommand
        {
            get;
            private set;
        }

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

        public State ActiveChatState 
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

        public Request ReceivedRequest { get; set; }
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

        public ObservableCollection<Request> ChatHistoryList { get; set; }

        #endregion

        private void UpdateActiveChatState()
        {
            switch (ActiveChatState)
            {
                case State.Listening:
                    Task.Run(() => StartListeningForRequest(IpAddress, User.PortNumber));
                    ExitVisibility = Visibility.Collapsed;
                    AcceptVisibility = Visibility.Collapsed;
                    DeclineVisibility = Visibility.Collapsed;
                    ActiveChatInfo = "No Active Chat";
                    break;

                case State.Connecting:
                    CancelListeningForRequest();
                    ActiveChatInfo = "Connecting...";
                    break;

                case State.Waiting:
                    Task.Run(() => StartListeningForResponse());
                    ExitVisibility = Visibility.Visible;
                    ActiveChatInfo = "Waiting...";
                    break;

                case State.Responding:
                    Task.Run(() => StartListeningForResponse());
                    AcceptVisibility = Visibility.Visible;
                    DeclineVisibility = Visibility.Visible;
                    ActiveChatInfo = ReceivedRequest.UserName;
                    break;

                case State.Chatting:
                    AcceptVisibility = Visibility.Collapsed;
                    DeclineVisibility = Visibility.Collapsed;
                    ExitVisibility = Visibility.Visible;
                    ActiveChatInfo = ReceivedRequest.UserName;
                    break;

                default:
                    break;
            }
        }


        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
