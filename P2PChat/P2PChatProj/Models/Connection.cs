using P2PChatProj.Services;
using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public class Connection : BaseModel
    {
        // Private backing fields
        private User remoteUser;
        private ConnectionState state = ConnectionState.Listening;
        private string stateInfo = "No active chat";

        #region Properties
        
        // User info
        public User LocalUser { get; private set; }

        public User RemoteUser
        {
            get
            {
                return remoteUser;
            }
            set
            {
                remoteUser = value;
                RaisePropertyChanged("RemoteUser");
            }
        }

        // Sockets
        public Socket Listener { get; private set; }

        public Socket Receiver { get; private set; }

        public Socket Sender { get; private set; }

        // State info
        public ConnectionState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                UpdateConnectionState();
            }
        }

        public string StateInfo
        {
            get
            {
                return stateInfo;
            }
            set
            {
                stateInfo = value;
                RaisePropertyChanged("StateInfo");
            }
        }

        #endregion

        public Connection(User user)
        {
            LocalUser = user;

            SetupSockets();

            Task.Run(() => ListenForRemoteConnection());
        }

        public async Task ConnectToRemoteUser(string remoteIpAddress, int remotePortNumber)
        {
            Console.WriteLine("STATUS: Connecting to remote user");
            Sender = await NetworkService.ConnectToRemoteAsync(Sender, remoteIpAddress, remotePortNumber);

            if (Sender != null)
            {
                Console.WriteLine($"RESULT: Connection to {((IPEndPoint)Sender.RemoteEndPoint).Address} established");
            }
            else
            {
                Console.WriteLine("RESULT: Connection to remote failed");
            }
        }

        private async Task ListenForRemoteConnection()
        {
            bool listening = true;
            Console.WriteLine("STATUS: Listening for a connection");

            while (listening)
            {
                Receiver = await NetworkService.AcceptConnectionAsync(Listener);
                
                if (Receiver != null)
                {
                    Console.WriteLine($"RESULT: Connection request from {((IPEndPoint) Receiver.RemoteEndPoint).Address} accepted");
                    listening = false;
                }
                else
                {
                    Console.WriteLine("RESULT: Accept of connection failed");
                }
            }

            bool receiving = true;
            Console.WriteLine("STATUS: Waiting to receive data");

            while (receiving)
            {
                NetworkData networkData = await NetworkService.ReceiveDataAsync(Receiver);

                if (networkData != null)
                {
                    Console.WriteLine($"RESULT: Received network data: {networkData.Message} from {networkData.User.UserName}");
                }
                else
                {
                    Console.WriteLine("RESULT: Receive of data failed");
                }
            }
        }

        private void SetupSockets()
        {
            Console.WriteLine("STATUS: Setting up sockets");
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Receiver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(new IPEndPoint(IPAddress.Parse(LocalUser.IpAddress), LocalUser.PortNumber));
            Listener.Listen(100);
        }

        private void UpdateConnectionState()
        {
            Console.WriteLine("STATUS: Updating connection state");

            switch (State)
            {
                case ConnectionState.Listening:
                    break;

                case ConnectionState.Connecting:
                    break;

                case ConnectionState.Waiting:
                    break;

                case ConnectionState.Responding:
                    break;

                case ConnectionState.Chatting:
                    break;

                default:
                    Console.WriteLine("ERROR: Connection has no state");
                    break;
            }
        }
    }
}
