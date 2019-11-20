using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SocketTest
{
    class Program
    {
        private static string userName;
        private static int localPort;
        private static IPAddress localIp;
        private static IPEndPoint localEndPoint;
        private static Socket listener;
        private static Socket connector;
        private static List<Request> receivedRequests;
        private static List<string> messageHistory;

        static void Main(string[] args)
        {
            ////////////////////////// SETUP ///////////////////////////
            
            // Welcome message
            Console.WriteLine("Welcome to Socket Chat!\n");
            
            // Setup local end point
            SetupLocalEndPoint();

            // Create a listener socket and start listening
            SetupListener();
            Task.Run(() => StartListening());

            // Starting information message
            Console.WriteLine("\nWelcome " + userName + "!");
            Console.WriteLine("IP: " + localIp);
            Console.WriteLine("Listening to port: " + localPort.ToString() + "\n");
            
            //////////////////////// SETUP END //////////////////////////



            /////////////////////////// MENU ////////////////////////////
            
            int selection;
            bool isRunning = true;

            while (isRunning) 
            {
                PrintMenu();
                selection = GetSelection();

                switch (selection)
                {
                    case 0:
                        SendRequest();
                        break;
                    case 1:
                        AcceptRequests();
                        break;
                    case 2:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("ERROR: Wrong input");
                        break;
                } 
            }
            ///////////////////////// MENU END //////////////////////////



            ///////////////////////// TEARDOWN //////////////////////////

            // Cancel listening
            listener.Close();
            connector.Close();

            /////////////////////// TEARDOWN END ////////////////////////
        }

        private static void SendRequest()
        {
            IPAddress sendIp;
            int sendPort;
            IPEndPoint sendEndPoint;
            Console.WriteLine("\n===============");
            Console.WriteLine(" Send Requests ");
            Console.WriteLine("===============\n");
            
            Console.Write("Enter ip:");
            sendIp = IPAddress.Parse(Console.ReadLine());

            Console.Write("Enter a port: ");
            sendPort = Convert.ToInt32(Console.ReadLine());

            connector = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sendEndPoint = new IPEndPoint(sendIp, sendPort);

            connector.Connect(sendEndPoint);

            if (connector.Connected)
            {
                connector.Send(Encoding.UTF8.GetBytes(userName));
                runChat();
            }
            else 
            {
                Console.WriteLine("ERROR: Connection failed");
            }
        }

        private static void runChat(Request accRequest = null)
        {
            string recName = "";
            Socket chatSocket;

            Console.WriteLine("\n===============");
            Console.WriteLine("      Chat     ");
            Console.WriteLine("===============\n");

            if (accRequest == null) 
            {
                chatSocket = connector;
                Console.WriteLine("Waiting for request to accepted...\n");
                
                byte[] bytes = new byte[1024];
                int bytesRec = connector.Receive(bytes);

                Console.WriteLine(Encoding.UTF8.GetString(bytes, 0, bytesRec) + " accepted your request");
            }
            else 
            {
                chatSocket = accRequest.Socket;
                recName = accRequest.Name;
                chatSocket.Send(Encoding.UTF8.GetBytes(userName));

            }

            bool running = true;
            string message;
            messageHistory = new List<string>();

            Task.Run(() => ListenForMessages());

            while (running)
            {
                Console.Write("\nMessage: ");
                message = Console.ReadLine();

                if (message == "q")
                {
                    running = false;
                    continue;
                }
                else
                {
                    message = userName + ": " + message;
                    chatSocket.Send(Encoding.UTF8.GetBytes(message));
                    messageHistory.Add(message);
                    PrintMessages();
                }
            }
        }

        private static void PrintMessages()
        {
            Console.WriteLine();

            foreach (string message in messageHistory)
            {
                Console.WriteLine(message);
            }
        }

        private static void ListenForMessages();
        {
            
        }

        private static void AcceptRequests()
        {
            Console.WriteLine("\n===============");
            Console.WriteLine("Accept Requests");
            Console.WriteLine("===============\n");

            for ( int i = 0; i < receivedRequests.Count; i++ )
            {
                Console.Write(i.ToString() + ": ");
                Console.WriteLine(receivedRequests[i].Name);
            }

            Console.Write("\nEnter request to accept: ");
            int selection = Convert.ToInt32(Console.ReadLine());

            runChat(receivedRequests[selection]);
        }

        static void SetupLocalEndPoint()
        {
            Console.Write("Enter a name: ");
            userName = Console.ReadLine();

            Console.Write("Enter a port: ");
            localPort = Convert.ToInt32(Console.ReadLine());

            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in hostInfo.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = address;
                    break;
                }
            }

            localEndPoint = new IPEndPoint(localIp, localPort);
        }

        static void SetupListener()
        {
            receivedRequests = new List<Request>();
            listener = new Socket(localIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(100);
        }

        static void StartListening()
        {
            bool listening = true;

            while (listening)
            {
                Socket handler = listener.Accept();
                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                string reqName = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                receivedRequests.Add(new Request(handler, reqName));
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine("====== Menu ======");
            Console.WriteLine("0 - Send Request");
            Console.WriteLine("1 - Accept Request");
            Console.WriteLine("2 - Quit");
            Console.WriteLine("====== Menu ======");
        }

        static int GetSelection()
        {
            Console.Write("\nEnter menu selection: ");
            return Convert.ToInt32(Console.ReadLine());
        }

    }
}
