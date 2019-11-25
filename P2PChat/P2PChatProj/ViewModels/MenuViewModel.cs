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
        private User user;
        private string inputIp;
        private int inputPort;
        private string activeChatName;
        private Visibility exitVisibility = Visibility.Collapsed;
        private Visibility acceptVisibility = Visibility.Collapsed;
        private Visibility declineVisibility = Visibility.Collapsed;
            

        public event PropertyChangedEventHandler PropertyChanged;

        public MenuViewModel(User user, IPAddress localIp)
        { 
            this.user = user;
            activeChatName = "No Active Chat";
            IpAddress = localIp.ToString();
            ChatHistoryList = new ObservableCollection<Request>();

            // Setting up listener for chat requests

            Task.Run(() => ListenForRequest(localIp, user.PortNumber));

            RequestCommand = new SendRequestCommand(this);
            ExitActiveChatCommand = new ExitActiveChatCommand(this);
        }


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

        public void PauseListenerClick()
        {
            RequestListener.StopListening();
        }

        public void SendRequestClick()
        {
            PauseListenerClick();
            RequestSender.SendRequest(new Request(IPAddress.Parse(InputIp), InputPort, User.UserName));
        }

        public string InputIp
        {
            get { return inputIp; }
            set { inputIp = value; }
        }

        public int InputPort
        {
            get { return inputPort; }
            set { inputPort = value; }
        }

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


        public string IpAddress { get; set; }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public ObservableCollection<Request> ChatHistoryList
        {
            get;
            set;
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
