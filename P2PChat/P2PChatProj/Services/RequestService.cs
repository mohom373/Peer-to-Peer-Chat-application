using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using P2PChatProj.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Windows;
using P2PChatProj.ViewModels;

namespace P2PChatProj.Services
{
    class RequestService
    {
        private static Socket requestListener;
        private static Socket requestHandler;


        public static void SetupSockets(IPAddress localIp, int portNumber)
        {
            requestHandler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestListener.Bind(new IPEndPoint(localIp, portNumber));
            requestListener.Listen(100);
        }

        public static Request StartListening(IPAddress localIp, int portNumber)
        {
            SetupSockets(localIp, portNumber);

            byte[] bytes = new byte[1024];
            int bytesRec;

            try
            {
                requestHandler = requestListener.Accept();
            }
            catch(SocketException)
            {
                Console.WriteLine("Accept canceled");
                return null;
            }

            string messageReceived = "";

            if (requestHandler.Connected)
            {
                try
                {
                    bytesRec = requestHandler.Receive(bytes);
                    
                }
                catch(SocketException)
                {
                    Console.WriteLine("Receive canceled");
                    return null;
                }
                messageReceived = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                Request requestReceived  = JsonConvert.DeserializeObject<Request>(messageReceived);

                return requestReceived;
            }
            else
            {
                return null;
            }
        }

        public static void StopListening()
        {
            requestListener.Close();
            
            try
            {
                requestHandler.Shutdown(SocketShutdown.Both);
            }
            catch(SocketException)
            {
                Console.WriteLine("handler never got a connection");
                requestHandler.Close();
            }
        }

        public static async Task<bool> SendRequestAsync(Request request, IProgress<MenuViewModel.State> updateState)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(request.IpAddress), request.PortNumber);
            requestHandler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string jsonRequest = JsonConvert.SerializeObject(request);

            //Request fromJsonRequest = JsonConvert.DeserializeObject<Request>(jsonRequest);
            //Console.WriteLine(fromJsonRequest.IpAddress);
            //Console.WriteLine(fromJsonRequest.PortNumber.ToString());
            //Console.WriteLine(fromJsonRequest.UserName);

            bool sendFailed = await Task.Run(() =>
            {
                try
                {
                    requestHandler.Connect(remoteEndPoint);
                    requestHandler.Send(Encoding.UTF8.GetBytes(jsonRequest));
                }
                catch (SocketException)
                {
                    MessageBox.Show($"Could not connect to user!",
                    "Connection failed", MessageBoxButton.OK);
                    StopListening();
                    updateState.Report(MenuViewModel.State.Listening);
                    return true;
                }
                return false;
            });
    
            if (sendFailed)
            {
                return false;
            }
            
            updateState.Report(MenuViewModel.State.Waiting);
            StopListening();
            return true;
        }
    }
}
