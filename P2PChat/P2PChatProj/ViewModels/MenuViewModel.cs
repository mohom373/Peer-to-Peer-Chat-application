using P2PChatProj.Models;
using P2PChatProj.Services;
using P2PChatProj.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    public class MenuViewModel
    {
        private User user;
        private string inputIp;
        private int inputPort;
        private string activeChatName;
        public MenuViewModel(User user, IPAddress localIp)
        { 
            this.user = user;
            activeChatName = "No Active Chat";
            IpAddress = localIp.ToString();
            ChatHistoryList = new ObservableCollection<Request>();

            // Setting up listener for chat requests

            Task.Run(() => ListenForRequest(localIp, user.PortNumber));

            RequestCommand = new SendRequestCommand(this);
        }

        public ICommand RequestCommand
        {
            get;
            private set;
        }

        public void ListenForRequest(IPAddress localIp, int localPort)
        {
            RequestListener.SetupListener(localIp, localPort);
            Request receivedRequest = RequestListener.StartListening();

            //Har vi redan en aktiv chat
            if (receivedRequest == null)
            {
                Console.WriteLine("Shit went wrong");
            }
            else
            {
                ExitVisibility = Visibility.Visible;
                activeChatName = receivedRequest.UserName;
            }
        }

        public void SendRequestClick()
        {
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
            get { return activeChatName; }
            set { activeChatName = value; }
        }

        public Visibility ExitVisibility { get; set; } = Visibility.Hidden;
        public Visibility AcceptVisibility { get; set; } = Visibility.Hidden;
        public Visibility DeclineVisibility { get; set; } = Visibility.Hidden;

        //public void addRequestToList(object sender, Request request)
        //{
        //    ReceivedRequestsList.Add(request);
        //}


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
    }
}
