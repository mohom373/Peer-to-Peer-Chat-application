using P2PChatProj.Services;
using P2PChatProj.ViewModels;
using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                RaisePropertyChanged("State");
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

        // Dialog window
        public DialogWindowViewModel InfoDisplay { get; private set; } = new DialogWindowViewModel();

        // Delegates
        public Action UpdateMenuButtons { get; set; }

        public Action<NetworkData, bool> AddRemoteMessage { get; set; }

        public Action ExitChat { get; set; }

        #endregion

        public Connection(User user)
        {
            LocalUser = user;

            SetupSockets();

            Task.Run(() => ListenForRemoteConnection());
        }

        #region Network Methods

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
                }
                else
                {
                    Console.WriteLine("RESULT: Accept of connection failed");
                    Receiver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }

                if (Receiver != null && Receiver.Connected)
                {
                    Console.WriteLine("STATUS: Waiting to receive request data");
                    NetworkData networkData = await NetworkService.ReceiveDataAsync(Receiver);

                    if (networkData != null && networkData.DataType == NetworkDataType.Request)
                    {
                        Console.WriteLine($"RESULT: Received request data from {networkData.User.UserName}");
                        RemoteUser = networkData.User;
                        listening = false;
                    }
                    else
                    {
                        Console.WriteLine("RESULT: Receive of request data failed");
                        Receiver.Shutdown(SocketShutdown.Both);
                    }

                    if (!Sender.Connected)
                    {
                        await ConnectToRemoteUser(RemoteUser.IpAddress, RemoteUser.PortNumber);
                        State = ConnectionState.Responding;
                    }
                    else
                    {
                        State = ConnectionState.Waiting;
                    }
                }
                else
                {
                    Console.WriteLine("STATUS: Listening for connection canceled");
                }
            }
        }

        public async Task ConnectToRemoteUser(string remoteIpAddress, int remotePortNumber)
        {
            Console.WriteLine("STATUS: Connecting to remote user");
            State = ConnectionState.Connecting;
            Sender = await NetworkService.ConnectToRemoteAsync(Sender, remoteIpAddress, remotePortNumber);

            if (Sender != null)
            {
                Console.WriteLine($"RESULT: Connection to {((IPEndPoint)Sender.RemoteEndPoint).Address} established");
            }
            else
            {
                Console.WriteLine("RESULT: Connection to remote failed");
                Sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                State = ConnectionState.Listening;
                InfoDisplay.Show("Could not connect to user");
                return;
            }

            Console.WriteLine("STATUS: Sending request data to remote user");
            NetworkData request = new NetworkData(LocalUser, NetworkDataType.Request);
            bool sent = await NetworkService.SendDataAsync(Sender, request);

            if (sent)
            {
                Console.WriteLine("RESULT: Sending of request data successful");
            }
            else
            {
                Console.WriteLine("RESULT: Sending of request data failed");
                Sender.Shutdown(SocketShutdown.Both);
                InfoDisplay.Show("Could not send request to user");
                State = ConnectionState.Listening;
            }
        }

        public async Task ReceiveNetworkData()
        {
            if (State == ConnectionState.Listening || 
                State == ConnectionState.Connecting)
            {
                Console.WriteLine("ERROR: Not ready to receive network data");
                return;
            }

            bool receiving = true;
            NetworkData networkData = null;

            while (receiving)
            {
                Console.WriteLine("STATUS: Waiting to receive network data");
                networkData = await NetworkService.ReceiveDataAsync(Receiver);

                if (networkData != null && networkData.DataType == NetworkDataType.Response)
                {
                    Console.WriteLine($"RESULT: Received response data from {networkData.User.UserName}");
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProcessResponse(networkData);
                    });
                }
                else if (networkData != null && networkData.DataType == NetworkDataType.Message)
                {
                    Console.WriteLine($"RESULT: Received message data from {networkData.User.UserName}");
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProcessMessage(networkData);
                    });
                }
                else if (networkData != null && networkData.DataType == NetworkDataType.Request)
                { 
                    Console.WriteLine("RESULT: Wrong type of data received");
                }
                else
                {
                    Console.WriteLine("RESULT: Receiving of network data failed");
                    receiving = false;
                }

                if (State == ConnectionState.Listening ||
                    State == ConnectionState.Connecting)
                {
                    Console.WriteLine("STATUS: Receiving of network data canceled");
                    receiving = false;
                }
            }
        }

        public async Task SendNetworkData(NetworkData networkData)
        {
            Console.WriteLine("STATUS: Trying to send network data");
            if (Sender != null && Sender.Connected)
            {
                Console.WriteLine($"STATUS: Sending network data of type {networkData.DataType} to remote user");
                bool sent = await NetworkService.SendDataAsync(Sender, networkData);

                if (sent)
                {
                    Console.WriteLine("RESULT: Sending of network data successful");
                }
                else
                {
                    Console.WriteLine("RESULT: Sending of network data failed");
                }
            }
            else
            {
                Console.WriteLine("RESULT: Sender has no connection");
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

        public void CloseConnection()
        {
            Console.WriteLine("STATUS: Closing listener socket");
            Listener.Close();

            if (Sender != null)
            {
                try
                {
                    Console.WriteLine("STATUS: Shutdown sender socket");
                    Sender.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException)
                {
                    Console.WriteLine("STATUS: Closing sender socket");
                    Sender.Close();
                }
            }

            if (Receiver != null)
            {
                try
                {
                    Console.WriteLine("STATUS: Shutdown receiver socket");
                    Receiver.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException)
                {
                    Console.WriteLine("STATUS: Closing receiver socket");
                    Receiver.Close();
                }
            }
        }

        #endregion

        private void ProcessResponse(NetworkData networkData)
        {
            Console.WriteLine("STATUS: Processing response");
            Console.WriteLine($"RESULT: Received a response of type {networkData.ResponseType}");

            switch (networkData.ResponseType)
            {
                case ResponseType.Exit:
                    ConnectionState tempState = State;
                    State = ConnectionState.Listening;
                    if (tempState == ConnectionState.Chatting)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ExitChat();
                        });
                        InfoDisplay.Show($"{RemoteUser.UserName} left the chat");
                    }
                    else
                    {
                        InfoDisplay.Show($"{RemoteUser.UserName} canceled their chat request");
                    }
                    break;

                case ResponseType.Accept:
                    State = ConnectionState.Chatting;
                    InfoDisplay.Show($"{RemoteUser.UserName} accepted your chat request");
                    break;

                case ResponseType.Decline:
                    State = ConnectionState.Listening;
                    InfoDisplay.Show($"{RemoteUser.UserName} declined your chat request");
                    break;

                case ResponseType.Disconnect:
                    State = ConnectionState.Listening;
                    InfoDisplay.Show($"{RemoteUser.UserName} disconnected from application");
                    break;

                case ResponseType.None:
                    Console.WriteLine("ERROR: Network data is not a response");
                    break;
            }
        }

        private void ProcessMessage(NetworkData networkData)
        {
            Console.WriteLine("STATUS: Processing message");
            Console.WriteLine($"RESULT: Received message > {networkData.Message}");
            Application.Current.Dispatcher.Invoke(() =>
            {
                AddRemoteMessage(networkData, true);
            });
        }

        private void UpdateConnectionState()
        {
            Console.WriteLine("STATUS: Updating connection state");

            switch (State)
            {
                case ConnectionState.Listening:
                    CloseConnection();
                    SetupSockets();
                    Task.Run(() => ListenForRemoteConnection());
                    StateInfo = "No active chat";
                    UpdateMenuButtons();
                    break;

                case ConnectionState.Connecting:
                    StateInfo = "Connecting...";
                    UpdateMenuButtons();
                    break;

                case ConnectionState.Waiting:
                    Task.Run(() => ReceiveNetworkData());
                    StateInfo = "Waiting...";
                    UpdateMenuButtons();
                    break;

                case ConnectionState.Responding:
                    Task.Run(() => ReceiveNetworkData());
                    StateInfo = RemoteUser.UserName;
                    UpdateMenuButtons();
                    break;

                case ConnectionState.Chatting:
                    StateInfo = RemoteUser.UserName;
                    UpdateMenuButtons();
                    break;

                default:
                    Console.WriteLine("ERROR: Connection has no state");
                    break;
            }
        }

        
    }
}
