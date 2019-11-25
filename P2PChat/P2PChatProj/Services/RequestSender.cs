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
    public class RequestSender
    {
        public static void SendRequest(Request request)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(request.IpAddress, request.PortNumber);
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(remoteEndPoint);
                sender.Send(Encoding.UTF8.GetBytes(request.UserName));
            }
            catch(SocketException)
            {
                MessageBox.Show($"Could not connect to user!",
                    "Connection failed", MessageBoxButton.OK);
            }
            
        }
    }
}
