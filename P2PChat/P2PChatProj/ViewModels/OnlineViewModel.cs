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
    public class OnlineViewModel : BaseViewModel
    {
        private User user;

        public OnlineViewModel(User user, MainWindowViewModel mainWindowViewModel)
        {
            this.user = user;
            MainWindowViewModel = mainWindowViewModel;
            setLocalIp();
            MenuViewModel = new MenuViewModel(this, user, LocalIp);
            ChatViewModel = new ChatViewModel(this, user);
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
            await MenuViewModel.AppClosing();
            await ChatViewModel.AppClosing();
            Console.WriteLine("OnlineView Closing");
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

        internal void StartChat(Request remoteUser)
        {
            ChatViewModel.SetupChat(remoteUser);
        }

        internal void ExitChat()
        {
            ChatViewModel.CloseChat();
        }
    }
}
