using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PChatProj.Models
{
    public class Request : NetworkData
    {
        public Request(string ipAddress, int portNumber, string userName, string requestIp, int requestPort)
        {
            IpAddress = ipAddress;
            PortNumber = portNumber;
            UserName = userName;
            RequestIp = requestIp;
            RequestPort = requestPort;
        }

        public string RequestIp { get; set; }

        public int RequestPort { get; set; }
    }
}
