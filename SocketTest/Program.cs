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
        private static Socket requestListener;
        private static Socket chatListener;
        private static Socket connectorSender;
        private static Socket messageReceiver;
        private static List<Request> receivedRequests;
        private static List<string> messageHistory;

        static void Main(string[] args)
        {
            ////////////////////////// SETUP ///////////////////////////

            // Welcome message
            Console.WriteLine("Welcome to Socket Chat!\n");

            // Setup sockets
            chatListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectorSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            messageReceiver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            messageHistory = new List<string>();

            // Setup local end point
            SetupLocalEndPoint();

            // Create a listener socket and start listening
            SetupRequestListener();
            Task.Run(() => ListenForRequests());

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
            requestListener.Close();
            chatListener.Close();
            messageReceiver.Close();
            connectorSender.Close();

            /////////////////////// TEARDOWN END ////////////////////////
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

        static void SetupRequestListener()
        {
            receivedRequests = new List<Request>();
            requestListener = new Socket(localIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            requestListener.Bind(localEndPoint);
            requestListener.Listen(100);
        }

        static void ListenForRequests()
        {
            bool listening = true;

            while (listening)
            {
                Socket handler = requestListener.Accept();
                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                string reqMessage = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                string reqName = reqMessage.Split(':')[0];
                int reqPort = Convert.ToInt32(reqMessage.Split(':')[1]);
                receivedRequests.Add(new Request(handler, reqName, reqPort));
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

        private static void SendRequest()
        {
            IPAddress sendIp;
            int sendPort;

            Console.WriteLine("\n===============");
            Console.WriteLine(" Send Requests ");
            Console.WriteLine("===============\n");

            Console.Write("Enter ip:");
            sendIp = IPAddress.Parse(Console.ReadLine());

            Console.Write("Enter a port: ");
            sendPort = Convert.ToInt32(Console.ReadLine());

            connectorSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint sendEndPoint = new IPEndPoint(sendIp, sendPort);
            connectorSender.Connect(sendEndPoint);

            if (connectorSender.Connected)
            {
                byte[] bytes = new byte[1024];

                chatListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                chatListener.Bind(new IPEndPoint(localIp, localPort + 1));
                chatListener.Listen(100);

                string sendMessage = userName + ":" + (localPort + 1).ToString();
                connectorSender.Send(Encoding.UTF8.GetBytes(sendMessage));
                Console.WriteLine("\nWaiting for request to be accepted...\n");

                messageReceiver = chatListener.Accept();

                Console.WriteLine("Request Accepted");

                runChat();
            }
            else
            {
                Console.WriteLine("ERROR: Connection failed");
            }
        }

        private static void AcceptRequests()
        {
            Console.WriteLine("\n===============");
            Console.WriteLine("Accept Requests");
            Console.WriteLine("===============\n");

            for (int i = 0; i < receivedRequests.Count; i++)
            {
                Console.Write(i.ToString() + ": ");
                Console.WriteLine(receivedRequests[i].Name);
            }

            Console.Write("\nEnter request to accept: ");
            int selection = Convert.ToInt32(Console.ReadLine());

            messageReceiver = receivedRequests[selection].Socket;
            IPAddress remoteIp = ((IPEndPoint)(messageReceiver.RemoteEndPoint)).Address;
            connectorSender.Connect(new IPEndPoint(remoteIp, receivedRequests[selection].ChatPort));

            runChat();
        }


        private static void runChat()
        {
            Console.WriteLine("\n===============");
            Console.WriteLine("      Chat     ");
            Console.WriteLine("===============\n");

            bool running = true;
            string message;
            messageHistory = new List<string>();

            Task.Run(() => ListenForMessages());

            while (running)
            {
                Console.WriteLine();
                message = Console.ReadLine();

                if (message == "q")
                {
                    running = false;
                    continue;
                }
                else
                {
                    message = userName + ": " + message;
                    connectorSender.Send(Encoding.UTF8.GetBytes(message));
                    messageHistory.Add(message);
                    PrintMessages();
                }
            }
        }

        private static void ListenForMessages()
        {
            bool listening = true;

            while (listening)
            {
                byte[] bytes = new byte[1024];
                int bytesRec = messageReceiver.Receive(bytes);
                string messageRec = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                messageHistory.Add(messageRec);
                PrintMessages();

            }
        }

        private static void PrintMessages()
        {
            Console.WriteLine();

            foreach (string message in messageHistory)
            {
                Console.WriteLine(message);
            }

            Console.WriteLine();
        }

    }

}
