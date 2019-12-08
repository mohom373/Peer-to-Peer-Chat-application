using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    /// <summary>
    /// Holds information about the user
    /// </summary>
    public class User : BaseModel
    {
        // Private variables
        private string userName;
        private string ipAddress;
        private int portNumber;

        #region Properties
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
                RaisePropertyChanged("IpAddress");
            }
        }

        public int PortNumber
        {
            get
            {
                return portNumber;
            }
            set
            {
                portNumber = value;
                RaisePropertyChanged("PortNumber");
            }
        }
        #endregion

        /// <summary>
        /// User constructor, sets ip to local IP address if omitted
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <param name="portNumber">Port number of the user</param>
        /// <param name="ipAddress">IP address of the user</param>
        public User(string userName, int portNumber, string ipAddress = "")
        {
            UserName = userName;
            PortNumber = portNumber;

            if (ipAddress == "")
            {
                // If ipAddress is an empty string it is set to the local IP address 
                IpAddress = GetLocalIp();
            }
            else
            {
                IpAddress = ipAddress;
            }
        }

        /// <summary>
        /// Makes a UDP connection and extracts the local IP address
        /// </summary>
        /// <returns>A local IPv4 address as a string</returns>
        public string GetLocalIp()
        {
            IPAddress LocalIp;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = (IPEndPoint)socket.LocalEndPoint;
                LocalIp = endPoint.Address;
            }
            return LocalIp.ToString();
        }
    }
}
