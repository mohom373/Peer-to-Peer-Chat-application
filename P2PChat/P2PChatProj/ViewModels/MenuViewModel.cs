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
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    public class MenuViewModel
    {
        private User user;
        private string inputIp;
        private int inputPort;
        public MenuViewModel(User user, IPAddress localIp)
        { 
            this.user = user;
            IpAddress = localIp.ToString();
            ReceivedRequestsList = new ObservableCollection<Request>();

            // Setting up listener for chat requests
            Progress<Request> requestAdder = new Progress<Request>();
            requestAdder.ProgressChanged += addRequestToList;
            Task.Run(() => RequestListener.ListenForRequests(localIp, user.PortNumber, requestAdder));

            RequestCommand = new SendRequestCommand(this);
        }

        public ICommand RequestCommand
        {
            get;
            private set;
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

        public void addRequestToList(object sender, Request request)
        {
            ReceivedRequestsList.Add(request);
        }


        public string IpAddress { get; set; }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public ObservableCollection<Request> ReceivedRequestsList
        {
            get;
            set;
        }
    }
}
