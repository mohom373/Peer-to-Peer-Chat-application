using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // LISTEN
            int port = 8080;
            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = hostInfo.AddressList[3];
            IPEndPoint localEndPoint = new IPEndPoint(ip, port);

            Socket listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(100);

            Console.WriteLine("IP: " + ip.ToString());
            Console.WriteLine("Listening to port " + port.ToString());

            Socket handler = listener.Accept();

            Console.WriteLine("Have gotten connection with " + handler.RemoteEndPoint.ToString());

           
            Console.ReadLine(); // Used to keep console running in the end
        }
    }
}
