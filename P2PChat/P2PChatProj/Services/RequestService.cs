using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using P2PChatProj.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Windows;
using P2PChatProj.ViewModels;
using P2PChatProj.ViewModels.Enums;

namespace P2PChatProj.Services
{
    class RequestService
    {
        private static Socket requestListener;
        private static Socket requestHandler;

        public static void SetupSockets(IPAddress localIp, int portNumber)
        {
            requestHandler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            requestListener.Bind(new IPEndPoint(localIp, portNumber));
            requestListener.Listen(100);
        }

        public static async Task<Request> ListenForRequestAsync(IPAddress localIp, 
            int portNumber)
        {
            SetupSockets(localIp, portNumber);

            byte[] bytes = new byte[1024];
            int bytesRec = 0;

            bool cancelListening = await Task.Run(() =>
            {
                try
                {
                    requestHandler = requestListener.Accept();
                }
                catch (SocketException)
                {
                    Console.WriteLine("Accept canceled");
                    return true;
                }
                return false;
            });

            if (cancelListening)
            {
                return null;
            }

            string dataReceived = "";

            if (requestHandler.Connected)
            {
                cancelListening = await Task.Run(() =>
                {
                    try
                    {
                        bytesRec = requestHandler.Receive(bytes);

                    }
                    catch (SocketException)
                    {
                        Console.WriteLine("Receive canceled");
                        return true;
                    }
                    return false;
                });

                if (cancelListening)
                {
                    return null;
                }

                dataReceived = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                Request requestReceived = JsonConvert.DeserializeObject<Request>(dataReceived);
                return requestReceived;
            }
            else
            {
                return null;
            }
        }

        public static void CancelRequestListener()
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

        public static async Task<RequestResponse> ListenForResponseAsync()
        {
            byte[] bytes = new byte[1024];
            int bytesRec = 0;
            string dataReceived;

            bool responseReceived = await Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("WAITING TO RECEIVE RESPONSE");
                    bytesRec = requestHandler.Receive(bytes);

                }
                catch (SocketException)
                {
                    Console.WriteLine("response receive cancel");
                    return false;
                }
                return true;
            });

            Console.WriteLine("RECEIVE IS DONE");
            Console.WriteLine("RESULT: " + responseReceived.ToString());

            if (responseReceived)
            {
                dataReceived = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                Console.WriteLine("=======================");
                Console.WriteLine(dataReceived);
                RequestResponse response = JsonConvert.DeserializeObject<RequestResponse>(dataReceived);
                return response;
            }
            else 
            {
                return null;
            }

        }

        public static async Task<bool> SendRequestAsync(Request request)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(request.IpAddress), request.PortNumber);
            requestHandler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string jsonRequest = JsonConvert.SerializeObject(request);

            bool sendSuccess = await Task.Run(() =>
            {
                try
                {
                    requestHandler.Connect(remoteEndPoint);
                    requestHandler.Send(Encoding.UTF8.GetBytes(jsonRequest));
                }
                catch (SocketException)
                {
                    return false;
                }
                return true;
            });
    
            return sendSuccess;
        }

        public static async Task<bool> SendResponse(RequestResponse res)
        {
            string jsonResponse = JsonConvert.SerializeObject(res);

            bool sendSuccess = await Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("WAITING TO SEND RESPONSE");
                    requestHandler.Send(Encoding.UTF8.GetBytes(jsonResponse));
                }
                catch (SocketException)
                {
                    return false;
                }
                return true;
            });

            Console.WriteLine("SEND IS DONE");
            Console.WriteLine("RESULT: " + sendSuccess.ToString());

            return sendSuccess;
        }
    }
}
