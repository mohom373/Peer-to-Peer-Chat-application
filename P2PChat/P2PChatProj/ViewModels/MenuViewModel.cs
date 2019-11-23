using P2PChatProj.Models;
using P2PChatProj.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    public class MenuViewModel
    {
        private User user;
        public MenuViewModel(User user, IPAddress localIp)
        { 
            this.user = user;
            IpAddress = localIp.ToString();
            ReceivedRequestsList = new ObservableCollection<Request>();

            Progress<Request> requestAdder = new Progress<Request>();
            requestAdder.ProgressChanged += addRequestToList;
            Task.Run(() => RequestListener.ListenForRequests(localIp, user.PortNumber, requestAdder));


        }

        private void addRequestToList(object sender, Request request)
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
