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
        public MenuViewModel(User user)
        { 
            this.user = user;
            IpAddress = "10.253.242.214"; // getLocalIp().ToString();
            ReceivedRequestsList = new ObservableCollection<string>();
            Task.Run(() => RequestListener.ListenForRequests(ReceivedRequestsList, user.PortNumber));


        }

        public string IpAddress { get; set; }
        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public ObservableCollection<string> ReceivedRequestsList
        {
            get;
            set;
        }

        public IPAddress getLocalIp()
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in hostInfo.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address;
                }
            }
            return null;
        }
    }
}
