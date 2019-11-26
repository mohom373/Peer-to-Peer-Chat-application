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
        public static bool SendRequest(Request request)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(request.IpAddress), request.PortNumber);
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string jsonRequest = JsonConvert.SerializeObject(request);
            Console.WriteLine(jsonRequest);

            Request fromJsonRequest = JsonConvert.DeserializeObject<Request>(jsonRequest);
            Console.WriteLine(fromJsonRequest.IpAddress);
            Console.WriteLine(fromJsonRequest.PortNumber.ToString());
            Console.WriteLine(fromJsonRequest.UserName);

            try
            {
                sender.Connect(remoteEndPoint);
                sender.Send(Encoding.UTF8.GetBytes(request.UserName));
            }
            catch(SocketException)
            {
                MessageBox.Show($"Could not connect to user!",
                    "Connection failed", MessageBoxButton.OK);
                return false;
            }
            return true;
        }
    }
}
