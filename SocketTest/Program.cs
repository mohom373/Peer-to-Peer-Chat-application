using System;
using System.Net;
using System.Net.Sockets;

namespace SocketTest
{
    class Program
    {

        static void Main(string[] args)
        {
            ///////////////////////// SETUP //////////////////////////
            string userName;
            int localPort;
            
            Console.WriteLine("Welcome to Socket Chat!\n");
            
            Console.Write("Enter a name: ");
            userName = Console.ReadLine();

            Console.Write("Enter a port: ");
            localPort = Convert.ToInt32(Console.ReadLine());


            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIp = hostInfo.AddressList[0];

            foreach (IPAddress address in hostInfo.AddressList) 
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = address;
                    break;
                }
            }
          
            IPEndPoint localEndPoint = new IPEndPoint(localIp, localPort);

            Socket listener = new Socket(localIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(100);

            Console.WriteLine("\nWelcome " + userName + "!");
            Console.WriteLine("IP: " + localIp);
            Console.WriteLine("Listening to port: " + localPort.ToString() + "\n");
            ///////////////////////// SETUP END //////////////////////////

            
            
            /////////////////////////// MENU ////////////////////////////
            int selection;
            bool isRunning = true;

            while (isRunning) 
            {
                Console.WriteLine("====== Menu ======");
                Console.WriteLine("0 - Send Request");
                Console.WriteLine("1 - Accept Request");
                Console.WriteLine("2 - Quit");
                Console.WriteLine("====== Menu ======");

                Console.Write("\nEnter menu selection: ");
                selection = Convert.ToInt32(Console.ReadLine());

                if (selection == 0)
                {
                    Console.WriteLine("Send Request Mode...");
                } 
                else if (selection == 1) 
                {
                    Console.WriteLine("Accept Request Mode...");
                }
                else if (selection == 2)
                {
                    isRunning = false;
                }
                else
                {
                    Console.WriteLine("ERROR: Wrong input!");
                }
            }
            ///////////////////////// MENU END //////////////////////////


            // LISTEN
            //int port = 8080;
            //IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ip = hostInfo.AddressList[1];
            //IPEndPoint localEndPoint = new IPEndPoint(ip, port);

            //Socket listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //listener.Bind(localEndPoint);
            //listener.Listen(100);

            //Console.WriteLine("IP: " + ip.ToString());
            //Console.WriteLine("Listening to port " + port.ToString());

            //Socket handler = listener.Accept();

            //Console.WriteLine("Have gotten connection with " + handler.RemoteEndPoint.ToString());


            // CONNECT
            //Socket connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //string ipString = "10.253.244.1";
            //IPAddress ip = IPAddress.Parse(ipString);
            //IPEndPoint ipep = new IPEndPoint(ip, 8080);

            //try
            //{
            //    connectSocket.Connect(ipep);
            //}
            //catch (ArgumentNullException ae)
            //{
            //    Console.WriteLine("ArgumentNullException : {0}", ae.ToString());
            //}
            //catch (SocketException se)
            //{
            //    Console.WriteLine("SocketException : {0}", se.ToString());
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Unexpected exception : {0}", e.ToString());
            //}

            //if (connectSocket.Connected)
            //{
            //    Console.WriteLine("We are connected");
            //}
        }
    }
}
