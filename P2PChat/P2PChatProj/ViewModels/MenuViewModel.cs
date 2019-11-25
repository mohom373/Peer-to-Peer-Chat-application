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
            IpAddress = localIp.ToString();
            ChatHistoryList = new ObservableCollection<Request>();

            Task.Run(() => ListenForRequest(localIp, user.PortNumber));

            RequestCommand = new SendRequestCommand(this);
            ExitActiveChatCommand = new ExitActiveChatCommand(this);
        }

        public void ListenForRequest(IPAddress localIp, int localPort)
        {
            Request receivedRequest = RequestListener.StartListening(localIp, localPort);

            //Har vi redan en aktiv chat
            if (receivedRequest == null)
            {
                Console.WriteLine("Shit went wrong");
            }
            else
            {
                AcceptVisibility = Visibility.Visible;
                DeclineVisibility = Visibility.Visible;

                ActiveChatName = receivedRequest.UserName;
                Console.WriteLine(ActiveChatName);
            }
        }

        public void PauseListener()
        {
            RequestListener.StopListening();
        }

        public async Task SendRequestAsync()
        {
            PauseListener();
            Connecting = true;
            ActiveChatName = "Connecting...";
            // lägg in bool om det gick bar eller inte
            await Task.Run(() => RequestSender.SendRequest(new Request(IPAddress.Parse(InputIp), InputPort, User.UserName)));
            Waiting = true;
            Connecting = false;
            ActiveChatName = "Waiting...";
        }


        #region Properties
        // Commands
        public ICommand RequestCommand
        {
            get;
            private set;
        }

        public ICommand ExitActiveChatCommand
        {
            get;
            private set;
        }

        // Input properties
        public string InputIp { get; set; }

        public int InputPort { get; set; }

        // Info
        public string IpAddress { get; set; }

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
