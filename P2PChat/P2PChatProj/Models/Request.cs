using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public class Request
    {
        private string ipAddress;
        private int portNumber;
        private string userName;

        public Request(string ipAddress = "", int portNumber = 0, string userName = "")
        {
            this.ipAddress = ipAddress;
            this.portNumber = portNumber;
            this.userName = userName;
        }

        public string IpAddress 
        {
            get { return ipAddress; } 
            set { ipAddress = value; } 
        }

        public int PortNumber 
        {
            get { return portNumber; }
            set { portNumber = value; } 
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

    }
}
