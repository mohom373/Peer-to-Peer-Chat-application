using Newtonsoft.Json;
using P2PChatProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace P2PChatProj.Services
{
    public class ChatService
    {
        private static Socket connectionListener;
        private static Socket receiver;
        private static Socket sender;

        public static void SetupSockets(IPAddress localIp, int chatPort)
        {
            Console.WriteLine("Listening for Sender on:");
            Console.WriteLine("IP: " + localIp.ToString());
            Console.WriteLine("Port: " + chatPort.ToString());

            connectionListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            receiver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectionListener.Bind(new IPEndPoint(localIp, chatPort));
            connectionListener.Listen(100);
        }

        public static async Task<bool> ConnectToSender()
        {
            bool connected = await Task.Run(() =>
            {
                try
                {
                    receiver = connectionListener.Accept();
                }
                catch (SocketException)
                {
                    return false;
                }
                return true;
            });

            return connected;
        }

        public static async Task<bool> ConnectToReceiver(User remoteUser)
        {
            Console.WriteLine("Connecting to Receiver on:");
            Console.WriteLine("IP: " + remoteUser.IpAddress);
            Console.WriteLine("Port: " + (remoteUser.PortNumber + 1).ToString());

            bool connected = await Task.Run(() =>
            {
                try
                {
                    sender.Connect(new IPEndPoint(IPAddress.Parse(remoteUser.IpAddress), (remoteUser.PortNumber + 1)));
                }
                catch(SocketException)
                {
                    return false;
                }
                return true;
            });
            return connected;
        }

        public static void CloseSockets()
        {
            connectionListener.Close();

            try
            {
                Console.WriteLine("Shutdown sender");
                sender.Shutdown(SocketShutdown.Both);

            }
            catch (SocketException)
            {
                Console.WriteLine("Close sender");
                sender.Close();
            }

            try
            {
                Console.WriteLine("Shutdown receiver");
                receiver.Shutdown(SocketShutdown.Both);

            }
            catch (SocketException)
            {
                Console.WriteLine("Close receiver");
                receiver.Close();
            }
        }

        public static async Task ListenForMessages(IProgress<ChatMessage> messageReporter)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = 0;
            string dataRec = "";
            ChatMessage message = null;

            bool listening = true;

            while(listening)
            {
                bool received = await Task.Run(() =>
                {
                    try
                    {
                        bytesRec = receiver.Receive(bytes);
                    }
                    catch (SocketException)
                    {
                        return false;
                    }
                    return true;
                });

                if (received)
                {
                    dataRec = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    message = JsonConvert.DeserializeObject<ChatMessage>(dataRec);
                    messageReporter.Report(message);
                }
            }

        }

        public static async Task<bool> SendMessage(ChatMessage message)
        {
            return true;
        }
        
    }
}
