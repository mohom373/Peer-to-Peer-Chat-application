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
    class RequestService
    {
        private static Socket requestListener;
        private static Socket requestHandler;

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

            string reqMessage = "";

            if (requestHandler.Connected)
            {
                try
                {
                    bytesRec = requestHandler.Receive(bytes);
                    reqMessage = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                }
                catch(SocketException)
                {
                    Console.WriteLine("Receive canceled");
                    return null;
                }
                return new Request(null, 0, reqMessage);
            }
            else
            {
                return null;
            }
        }

        public static void SetupSockets(IPAddress localIp, int portNumber)
        {
            requestHandler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestListener.Bind(new IPEndPoint(localIp, portNumber));
            requestListener.Listen(100);
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
    }
}
