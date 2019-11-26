using P2PChatProj.Models;
using P2PChatProj.Services;
using P2PChatProj.ViewModels.Commands;
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
        // Private backup variables
        private string activeChatName;
        private Visibility exitVisibility = Visibility.Collapsed;
        private Visibility acceptVisibility = Visibility.Collapsed;
        private Visibility declineVisibility = Visibility.Collapsed;
            
        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        public MenuViewModel(User user, IPAddress localIp)
        { 
            User = user;
            activeChatName = "No Active Chat";
            IpAddress = localIp;
            ChatHistoryList = new ObservableCollection<Request>();

            Task.Run(() => ListenForRequest(localIp, user.PortNumber));

            RequestButtonCommand = new SendRequestCommand(this);
            ExitButtonCommand = new ExitActiveChatCommand(this);
            AcceptButtonCommand = new AcceptChatCommand(this);
            DeclineButtonCommand = new DeclineChatCommand(this);
        }

        public void ListenForRequest(IPAddress localIp, int localPort)
        {
            Request receivedRequest = RequestService.StartListening(localIp, localPort);

            //Har vi redan en aktiv chat
            if (receivedRequest != null)
            {
                PauseListener();

                AcceptVisibility = Visibility.Visible;
                DeclineVisibility = Visibility.Visible;

                ActiveChatName = receivedRequest.UserName;
            }
        }

        public void PauseListener()
        {
            RequestService.StopListening();
        }

        public async Task SendRequestAsync()
        {
            PauseListener();
            Connecting = true;
            ActiveChatName = "Connecting...";
            
            bool sendSuccessful = await Task.Run(() => ChatService.SendRequest(new Request(InputIp, InputPort, User.UserName)));
            
            if (sendSuccessful)
            {
                Waiting = true;
                Connecting = false;
                ExitVisibility = Visibility.Visible;
                ActiveChatName = "Waiting...";
            }
            else
            {
                Connecting = false;
                Task.Run(() => ListenForRequest(IpAddress, User.PortNumber));
                ActiveChatName = "No Active Chat";
            }
            
        }

        public void ExitActiveChat()
        {
            Task.Run(() => ListenForRequest(IpAddress, User.PortNumber));
            ActiveChatName = "No Active Chat";
            ExitVisibility = Visibility.Collapsed;
            Waiting = false;

            /*
            if (Waiting)
            {
                ActiveChatName = "No Active Chat";
                ExitVisibility = Visibility.Collapsed;
            } 
            else if (Chatting)
            {
                ActiveChatName = "No Active Chat";
                ExitVisibility = Visibility.Collapsed;
            }
            */
        }

        public void AcceptChatRequest()
        {
            // setup chat view

            // Meddela andra part om accept

        }

        public void DeclineChatRequest()
        {  
            Task.Run(() => ListenForRequest(IpAddress, User.PortNumber));
            ActiveChatName = "No Active Chat";
            AcceptVisibility = Visibility.Collapsed;
            DeclineVisibility = Visibility.Collapsed;

            // Meddela andra part om decline
        }

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

        public string ActiveChatName
        {
            get
            {
                return activeChatName;
            }
            set
            {
                activeChatName = value;
                RaisePropertyChanged("ActiveChatName");
            }
        }

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

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
