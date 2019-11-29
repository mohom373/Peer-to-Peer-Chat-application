using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.ViewModels
{
    public class OnlineViewModel
    {
        private User user;

        public OnlineViewModel(User user, MainWindowViewModel mainWindowViewModel)
        {
            this.user = user;
            MainWindowViewModel = mainWindowViewModel;
            setLocalIp();
            MenuViewModel = new MenuViewModel(user, LocalIp);
            ChatViewModel = new ChatViewModel(user);
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public MainWindowViewModel MainWindowViewModel { get; set; }

        public IPAddress LocalIp { get; set; }

        public MenuViewModel MenuViewModel { get; set; }

        public ChatViewModel ChatViewModel { get; set; }
        
        public async void AppClosing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("OnlineView Closing");
            await MenuViewModel.AppClosing();
            //await ChatViewModel.AppClosing();
        }

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
