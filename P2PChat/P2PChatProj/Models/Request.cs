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
        private IPAddress ipAddress;
        private int portNumber;

        public Request(IPAddress ipAddress = null, int portNumber = 0)
        {
            this.ipAddress = ipAddress;
            this.portNumber = portNumber;
        }

        public IPAddress IpAddress 
        {
            get { return ipAddress; } 
            set { ipAddress = value; } 
        }

        public int PortNumber 
        {
            get { return portNumber; }
            set { portNumber = value; } 
        }

    }
}
