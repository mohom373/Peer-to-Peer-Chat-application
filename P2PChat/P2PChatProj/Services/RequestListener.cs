using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using P2PChatProj.Models;
using System.Collections.ObjectModel;

namespace P2PChatProj.Services
{
    class RequestListener
    {
        private static Socket requestListener ;
        public static void ListenForRequests(ObservableCollection<string> receivedRequestsList, int portNumber)
        {
            SetupListener(portNumber);
            
            bool listening = true;
            byte[] bytes = new byte[1024];
            int bytesRec;

            while (listening)
            {
                Console.WriteLine("IM INSIDE THE WHILE LOOP");
                Socket handler = requestListener.Accept();

                if (handler.Connected)
                {
                    Console.WriteLine("VI ÄR CONNECTED");
                }

                bytesRec = handler.Receive(bytes);
                string reqMessage = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                receivedRequestsList.Add(reqMessage);
            }
        }

        public static void SetupListener(int portNumber)
        {
            requestListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress localIpAddress = IPAddress.Parse("10.253.242.214"); //getLocalIp();

            //Console.WriteLine(localIpAddress.ToString());
            Console.WriteLine(portNumber);

            requestListener.Bind(new IPEndPoint(localIpAddress, portNumber));
            requestListener.Listen(100);
        }

        public static IPAddress getLocalIp()
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in hostInfo.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(address);
                        
                }
            }
            return null;
        }
    }
}
