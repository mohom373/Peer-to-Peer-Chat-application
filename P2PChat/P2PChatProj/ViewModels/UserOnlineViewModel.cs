using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    public class UserOnlineViewModel
    {
        private User user;
        

        public UserOnlineViewModel(User user)
        {
            this.user = user;
            setLocalIp();
            Menu = new MenuViewModel(user, LocalIp);
            Chat = new ChatViewModel(user);
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public IPAddress LocalIp { get; set; }

        public MenuViewModel Menu { get; set; }

        public ChatViewModel Chat { get; set; }

        public void setLocalIp()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = (IPEndPoint) socket.LocalEndPoint;
                LocalIp = endPoint.Address;
            }
        }
    }
}
