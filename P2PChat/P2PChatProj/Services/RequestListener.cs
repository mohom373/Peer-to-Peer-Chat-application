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
        public static void ListenForRequests(IPAddress localIp, int portNumber, IProgress<Request> addRequest)
        {
            SetupListener(localIp, portNumber);
            
            bool listening = true;
            byte[] bytes = new byte[1024];
            int bytesRec;

            while (listening)
            {
                Console.WriteLine("IM INSIDE THE WHILE LOOP");
                Socket handler = requestListener.Accept();

                if (handler.Connected)
                {
                    bytesRec = handler.Receive(bytes);
                    string reqMessage = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    addRequest.Report(new Request(null, 0, reqMessage));
                }
            }
        }

        public static void SetupListener(IPAddress localIp, int portNumber)
        {
            requestListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestListener.Bind(new IPEndPoint(localIp, portNumber));
            requestListener.Listen(100);
        }
    }
}
