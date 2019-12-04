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
        public Request(string ipAddress = "", int portNumber = 0, string userName = "")
        {
            IpAddress = ipAddress;
            PortNumber = portNumber;
            UserName = userName;
        }
    }
}
