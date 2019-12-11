using Newtonsoft.Json;
using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Services
{
    /// <summary>
    /// A service that can connect to other users, send and 
    /// receive data all using sockets and Newtonsoft JSON 
    /// </summary>
    public static class NetworkService
    {
        /// <summary>
        /// Accepts a connection and creates a handler for each connection
        /// </summary>
        /// <param name="listener">A listener socket</param>
        /// <returns>A handler socket with a connection</returns>
        public static async Task<Socket> AcceptConnectionAsync(Socket listener)
        { 
            return await Task.Run(() =>
            {
                try
                {
                    return listener.Accept();
                }
                catch (SocketException)
                {
                    Console.WriteLine("ERROR: Listener failed to accept connection");
                    return null;
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("ERROR: Listener failed to accept connection");
                    return null;
                }
            });
        }

        /// <summary>
        /// Connects to another users endpoint
        /// </summary>
        /// <param name="sender">A socket used to make the connection</param>
        /// <param name="remoteUser">Information about the remote user</param>
        /// <returns>Returns the sender with a connection</returns>
        public static async Task<Socket> ConnectToRemoteAsync(Socket sender, string remoteIpAddress, int remotePortNumber)
        {
            return await Task.Run(() =>
            {
                try
                {
                    sender.Connect(new IPEndPoint(IPAddress.Parse(remoteIpAddress), 
                                                                  remotePortNumber));
                }
                catch (SocketException)
                {
                    Console.WriteLine("ERROR: Sender failed to make a connection");
                    return null;
                }
                return sender;
            });
        }

        /// <summary>
        /// Receives data and converts it using JSON
        /// </summary>
        /// <param name="receiver">Socket used for receiving data</param>
        /// <returns>NetworkData object containig data and information about the data</returns>
        public static async Task<NetworkData> ReceiveDataAsync(Socket receiver)
        {
            byte[] bytes = new byte[1024];
            int bytesReceived = 0;
            string dataReceived = "";

            return await Task.Run(() =>
            {
                try
                {
                    bool receiving = true;
                    while (receiving)
                    {
                        bytesReceived = receiver.Receive(bytes);
                        dataReceived += Encoding.UTF8.GetString(bytes, 0, bytesReceived);
                        if (dataReceived.IndexOf("<EOF>") > -1)
                        {
                            receiving = false;
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("ERROR: Receiver failed to receive data");
                    Console.WriteLine($"EXCEPTION: {ex.ToString()}");
                    return null;
                }
                dataReceived.TrimEnd();
                dataReceived = dataReceived.Replace("<EOF>", "");
                return JsonConvert.DeserializeObject<NetworkData>(dataReceived);
            });
        }

        /// <summary>
        /// Converts and sends data to a remote user
        /// </summary>
        /// <param name="sender">Socket used to send data</param>
        /// <param name="networkData">NetworkData object containig data and information about the data</param>
        /// <returns>Boolean describing successful send</returns>
        public static async Task<bool> SendDataAsync(Socket sender, NetworkData networkData)
        {
            string sendData = JsonConvert.SerializeObject(networkData);

            return await Task.Run(() =>
            {
                try
                {
                    sender.Send(Encoding.UTF8.GetBytes(sendData + "<EOF>"));
                }
                catch (SocketException)
                {
                    Console.WriteLine("ERROR: Sender failed to send data");
                    return false;
                }
                return true;
            });
        }
    }
}
