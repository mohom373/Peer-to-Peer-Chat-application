using System;
using System.Net;
using System.Net.Sockets;

namespace SocketTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // LISTEN
            //int port = 8080;
            //IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ip = hostInfo.AddressList[2];
            //IPEndPoint localEndPoint = new IPEndPoint(ip, port);

            //Socket listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //listener.Bind(localEndPoint);
            //listener.Listen(100);

            //Console.WriteLine("IP: " + ip.ToString());
            //Console.WriteLine("Listening to port " + port.ToString());

            //Socket handler = listener.Accept();

            //Console.WriteLine("Have gotten connection with " + handler.RemoteEndPoint.ToString());


            // CONNECT
            Socket connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string ipString = "10.253.244.1";
            IPAddress ip = IPAddress.Parse(ipString);
            IPEndPoint ipep = new IPEndPoint(ip, 8080);

            try
            {
                connectSocket.Connect(ipep);
            }
            catch (ArgumentNullException ae)
            {
                Console.WriteLine("ArgumentNullException : {0}", ae.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

            if (connectSocket.Connected)
            {
                Console.WriteLine("We are connected");
            }

            Console.ReadLine(); // Used to keep console running in the end
        }
    }
}
